#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketBot.Logic.Common;
using PokemonGo.RocketBot.Logic.Logging;
using PokemonGo.RocketBot.Logic.State;
using PokemonGo.RocketBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

#endregion

namespace PokemonGo.RocketBot.Logic
{
    public class AuthSettings
    {
        [JsonIgnore] private string _filePath;
        [DefaultValue("msm8996")] public string AndroidBoardName;
        [DefaultValue("1.0.0.0000")] public string AndroidBootloader;

        public AuthType AuthType;
        [DefaultValue("HTC")] public string DeviceBrand;
        [DefaultValue("8525f5d8201f78b5")] public string DeviceId;
        [DefaultValue("HTC 10")] public string DeviceModel;
        [DefaultValue("qcom")] public string DeviceModelBoot;
        [DefaultValue("pmewl_00531")] public string DeviceModelIdentifier;
        // device data
        [DefaultValue("random")] public string DevicePackageName;
        [DefaultValue("pmewl_00531")] public string FirmwareBrand;

        [DefaultValue("htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys")] public string
            FirmwareFingerprint;

        [DefaultValue("release-keys")] public string FirmwareTags;
        [DefaultValue("user")] public string FirmwareType;
        public string GoogleApiKey;
        public string GooglePassword;
        public string GoogleUsername;
        [DefaultValue("HTC")] public string HardwareManufacturer;
        [DefaultValue("HTC 10")] public string HardwareModel;
        public string PtcPassword;
        public string PtcUsername;
        public bool UseProxy;
        public bool UseProxyAuthentication;
        public string UseProxyHost;
        public string UseProxyPassword;
        public string UseProxyPort;
        public string UseProxyUsername;

        public AuthSettings()
        {
            InitializePropertyDefaultValues(this);
        }

        public void InitializePropertyDefaultValues(object obj)
        {
            var fields = obj.GetType().GetFields();

            foreach (var field in fields)
            {
                var d = field.GetCustomAttribute<DefaultValueAttribute>();

                if (d != null)
                    field.SetValue(obj, d.Value);
            }
        }

        public void Load(string path)
        {
            try
            {
                _filePath = path;

                if (File.Exists(_filePath))
                {
                    //if the file exists, load the settings
                    var input = File.ReadAllText(_filePath);

                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                    JsonConvert.PopulateObject(input, this, settings);
                }
                // Do some post-load logic to determine what device info to be using - if 'custom' is set we just take what's in the file without question
                if (!DevicePackageName.Equals("random", StringComparison.InvariantCultureIgnoreCase) &&
                    !DevicePackageName.Equals("custom", StringComparison.InvariantCultureIgnoreCase))
                {
                    // User requested a specific device package, check to see if it exists and if so, set it up - otherwise fall-back to random package
                    var keepDevId = DeviceId;
                    SetDevInfoByKey();
                    DeviceId = keepDevId;
                }
                if (DevicePackageName.Equals("random", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Random is set, so pick a random device package and set it up - it will get saved to disk below and re-used in subsequent sessions
                    var rnd = new Random();
                    var rndIdx = rnd.Next(0, DeviceInfoHelper.DeviceInfoSets.Keys.Count - 1);
                    DevicePackageName = DeviceInfoHelper.DeviceInfoSets.Keys.ToArray()[rndIdx];
                    SetDevInfoByKey();
                }
                if (string.IsNullOrEmpty(DeviceId) || DeviceId == "8525f5d8201f78b5")
                    DeviceId = RandomString(16, "0123456789abcdef");
                // changed to random hex as full alphabet letters could have been flagged

                // Jurann: Note that some device IDs I saw when adding devices had smaller numbers, only 12 or 14 chars instead of 16 - probably not important but noted here anyway

                Save(_filePath);
            }
            catch (JsonReaderException exception)
            {
                if (exception.Message.Contains("Unexpected character") && exception.Message.Contains("PtcUsername"))
                    Logger.Write("JSON Exception: You need to properly configure your PtcUsername using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") && exception.Message.Contains("PtcPassword"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your PtcPassword using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") &&
                         exception.Message.Contains("GoogleUsername"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your GoogleUsername using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") &&
                         exception.Message.Contains("GooglePassword"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your GooglePassword using quotations.",
                        LogLevel.Error);
                else
                    Logger.Write("JSON Exception: " + exception.Message, LogLevel.Error);
            }
        }

        public void Save(string fullPath)
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                Converters = new JsonConverter[] {new StringEnumConverter {CamelCaseText = true}}
            };

            var output = JsonConvert.SerializeObject(this, jsonSerializeSettings);

            var folder = Path.GetDirectoryName(fullPath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                Save(_filePath);
            }
        }

        public void CheckProxy()
        {
            using (var tempWebClient = new NecroWebClient())
            {
                var unproxiedIp = WebClientExtensions.DownloadString(tempWebClient,
                    new Uri("https://api.ipify.org/?format=text"));
                if (UseProxy)
                {
                    tempWebClient.Proxy = InitProxy();
                    var proxiedIPres = WebClientExtensions.DownloadString(tempWebClient,
                        new Uri("https://api.ipify.org/?format=text"));
                    var proxiedIp = proxiedIPres == null ? "INVALID PROXY" : proxiedIPres;
                    Logger.Write(
                        $"Your IP is: {unproxiedIp} / Proxy IP is: {proxiedIp}",
                        LogLevel.Info, unproxiedIp == proxiedIp ? ConsoleColor.Red : ConsoleColor.Green);

                    if (unproxiedIp == proxiedIp || proxiedIPres == null)
                    {
                        Logger.Write("Press any key to exit so you can fix your proxy settings...",
                            LogLevel.Info, ConsoleColor.Red);
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Logger.Write(
                        $"Your IP is: {unproxiedIp}",
                        LogLevel.Info, ConsoleColor.Red);
                }
            }
        }

        private string RandomString(int length, string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789")
        {
            var outOfRange = byte.MaxValue + 1 - (byte.MaxValue + 1)%alphabet.Length;

            return string.Concat(
                Enumerable
                    .Repeat(0, int.MaxValue)
                    .Select(e => RandomByte())
                    .Where(randomByte => randomByte < outOfRange)
                    .Take(length)
                    .Select(randomByte => alphabet[randomByte%alphabet.Length])
                );
        }

        private byte RandomByte()
        {
            using (var randomizationProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[1];
                randomizationProvider.GetBytes(randomBytes);
                return randomBytes.Single();
            }
        }

        private void SetDevInfoByKey()
        {
            if (DeviceInfoHelper.DeviceInfoSets.ContainsKey(DevicePackageName))
            {
                AndroidBoardName = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["AndroidBoardName"];
                AndroidBootloader = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["AndroidBootloader"];
                DeviceBrand = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["DeviceBrand"];
                DeviceId = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["DeviceId"];
                DeviceModel = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["DeviceModel"];
                DeviceModelBoot = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["DeviceModelBoot"];
                DeviceModelIdentifier =
                    DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["DeviceModelIdentifier"];
                FirmwareBrand = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["FirmwareBrand"];
                FirmwareFingerprint =
                    DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["FirmwareFingerprint"];
                FirmwareTags = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["FirmwareTags"];
                FirmwareType = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["FirmwareType"];
                HardwareManufacturer =
                    DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["HardwareManufacturer"];
                HardwareModel = DeviceInfoHelper.DeviceInfoSets[DevicePackageName]["HardwareModel"];
            }
            else
            {
                throw new ArgumentException(
                    "Invalid device info package! Check your auth.config file and make sure a valid DevicePackageName is set. For simple use set it to 'random'. If you have a custom device, then set it to 'custom'.");
            }
        }

        private WebProxy InitProxy()
        {
            if (!UseProxy) return null;

            var prox = new WebProxy(new Uri($"http://{UseProxyHost}:{UseProxyPort}"), false, null);

            if (UseProxyAuthentication)
                prox.Credentials = new NetworkCredential(UseProxyUsername, UseProxyPassword);

            return prox;
        }
    }

    public class GlobalSettings
    {
        //console options
        [DefaultValue(10)] public int AmountOfPokemonToDisplayOnStart;
        [DefaultValue(5)] public int AmountOfTimesToUpgradeLoop;
        [JsonIgnore] public AuthSettings Auth = new AuthSettings();
        [DefaultValue(false)] public bool AutoFavoritePokemon;
        //powerup
        [DefaultValue(false)] public bool AutomaticallyLevelUpPokemon;
        //autoupdate
        [DefaultValue(true)] public bool AutoUpdate;
        //pokemon
        [DefaultValue(true)] public bool CatchPokemon;
        [DefaultValue(90)] public int CurveThrowChance;
        [DefaultValue(40.785091)] public double DefaultLatitude;
        [DefaultValue(-73.968285)] public double DefaultLongitude;
        //delays
        [DefaultValue(500)] public int DelayBetweenPlayerActions;
        [DefaultValue(100)] public int DelayBetweenPokemonCatch;
        [DefaultValue(false)] public bool DelayBetweenRecycleActions;
        [DefaultValue(true)] public bool DetailedCountsBeforeRecycling;
        //position
        [DefaultValue(false)] public bool DisableHumanWalking;
        //dump stats
        [DefaultValue(false)] public bool DumpPokemonStats;
        //customizable catch
        [DefaultValue(false)] public bool EnableHumanizedThrows;
        //evolve
        [DefaultValue(95)] public float EvolveAboveIvValue;
        [DefaultValue(false)] public bool EvolveAllPokemonAboveIv;
        [DefaultValue(true)] public bool EvolveAllPokemonWithEnoughCandy;
        [DefaultValue(90.0)] public double EvolveKeptPokemonsAtStorageUsagePercentage;
        [DefaultValue(10)] public int ExcellentThrowChance;
        //softban related
        [DefaultValue(false)] public bool FastSoftBanBypass;
        //favorite
        [DefaultValue(95)] public float FavoriteMinIvPercentage;
        [DefaultValue(1500)] public int ForceExcellentThrowOverCp;
        [DefaultValue(95.00)] public double ForceExcellentThrowOverIv;
        [DefaultValue(1000)] public int ForceGreatThrowOverCp;
        [DefaultValue(90.00)] public double ForceGreatThrowOverIv;
        [JsonIgnore] public string GeneralConfigPath;
        [DefaultValue(5000)] public int GetMinStarDustForLevelUp;
        [DefaultValue(true)] public bool GetOnlyVerifiedSniperInfoFromPokezz;
        [DefaultValue(true)] public bool GetSniperInfoFromPokeSnipers;
        [DefaultValue(true)] public bool GetSniperInfoFromPokeWatchers;
        [DefaultValue(true)] public bool GetSniperInfoFromPokezz;
        [DefaultValue("GPXPath.GPX")] public string GpxFile;
        [DefaultValue(30)] public int GreatThrowChance;

        [JsonIgnore] public bool IsGui;

        public List<KeyValuePair<ItemId, int>> ItemRecycleFilter = new List<KeyValuePair<ItemId, int>>
        {
            new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemLuckyEgg, 200),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseOrdinary, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseSpicy, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseCool, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseFloral, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemTroyDisk, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXAttack, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXDefense, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXMiracle, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemSpecialCamera, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasicUnlimited, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasic, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemPokemonStorageUpgrade, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemItemStorageUpgrade, 100)
        };

        //keeping
        [DefaultValue(1250)] public int KeepMinCp;
        [DefaultValue(0)] public int KeepMinDuplicatePokemon;
        [DefaultValue(90)] public float KeepMinIvPercentage;
        [DefaultValue(6)] public int KeepMinLvl;
        [DefaultValue("or")] public string KeepMinOperator;
        [DefaultValue(false)] public bool KeepPokemonsThatCanEvolve;
        [DefaultValue("iv")] public string LevelUpByCPorIv;

        [DefaultValue(3)] public int MaxBerriesToUsePerPokemon;
        //amounts
        [DefaultValue(6)] public int MaxPokeballsPerPokemon;


        public int MaxSpawnLocationOffset;
        [DefaultValue(1000)] public int MaxTravelDistanceInMeters;
        [DefaultValue(60000)] public int MinDelayBetweenSnipes;
        [DefaultValue(20)] public int MinPokeballsToSnipe;
        [DefaultValue(0)] public int MinPokeballsWhileSnipe;
        [DefaultValue(40)] public int NiceThrowChance;
        [DefaultValue(true)] public bool OnlyUpgradeFavorites;


        public List<PokemonId> PokemonsNotToTransfer = new List<PokemonId>
        {
            //criteria: from SS Tier to A Tier + Regional Exclusive
            PokemonId.Venusaur,
            PokemonId.Charizard,
            PokemonId.Blastoise,
            //PokemonId.Nidoqueen,
            //PokemonId.Nidoking,
            PokemonId.Clefable,
            //PokemonId.Vileplume,
            //PokemonId.Golduck,
            //PokemonId.Arcanine,
            //PokemonId.Poliwrath,
            //PokemonId.Machamp,
            //PokemonId.Victreebel,
            //PokemonId.Golem,
            //PokemonId.Slowbro,
            //PokemonId.Farfetchd,
            PokemonId.Muk,
            //PokemonId.Exeggutor,
            //PokemonId.Lickitung,
            PokemonId.Chansey,
            //PokemonId.Kangaskhan,
            //PokemonId.MrMime,
            //PokemonId.Tauros,
            PokemonId.Gyarados,
            //PokemonId.Lapras,
            PokemonId.Ditto,
            //PokemonId.Vaporeon,
            //PokemonId.Jolteon,
            //PokemonId.Flareon,
            //PokemonId.Porygon,
            PokemonId.Snorlax,
            PokemonId.Articuno,
            PokemonId.Zapdos,
            PokemonId.Moltres,
            PokemonId.Dragonite,
            PokemonId.Mewtwo,
            PokemonId.Mew
        };

        public List<PokemonId> PokemonsToEvolve = new List<PokemonId>
        {
            /*NOTE: keep all the end-of-line commas exept for the last one or an exception will be thrown!
            criteria: 12 candies*/
            PokemonId.Caterpie,
            PokemonId.Weedle,
            PokemonId.Pidgey,
            /*criteria: 25 candies*/
            //PokemonId.Bulbasaur,
            //PokemonId.Charmander,
            //PokemonId.Squirtle,
            PokemonId.Rattata
            //PokemonId.NidoranFemale,
            //PokemonId.NidoranMale,
            //PokemonId.Oddish,
            //PokemonId.Poliwag,
            //PokemonId.Abra,
            //PokemonId.Machop,
            //PokemonId.Bellsprout,
            //PokemonId.Geodude,
            //PokemonId.Gastly,
            //PokemonId.Eevee,
            //PokemonId.Dratini,
            /*criteria: 50 candies commons*/
            //PokemonId.Spearow,
            //PokemonId.Ekans,
            //PokemonId.Zubat,
            //PokemonId.Paras,
            //PokemonId.Venonat,
            //PokemonId.Psyduck,
            //PokemonId.Slowpoke,
            //PokemonId.Doduo,
            //PokemonId.Drowzee,
            //PokemonId.Krabby,
            //PokemonId.Horsea,
            //PokemonId.Goldeen,
            //PokemonId.Staryu
        };

        public List<PokemonId> PokemonsToIgnore = new List<PokemonId>
        {
            //criteria: most common
            PokemonId.Caterpie,
            PokemonId.Weedle,
            PokemonId.Pidgey,
            PokemonId.Rattata,
            PokemonId.Spearow,
            PokemonId.Zubat,
            PokemonId.Doduo
        };

        public List<PokemonId> PokemonsToLevelUp = new List<PokemonId>
        {
            //criteria: most common
            PokemonId.Caterpie,
            PokemonId.Weedle,
            PokemonId.Pidgey,
            PokemonId.Rattata,
            PokemonId.Spearow,
            PokemonId.Zubat,
            PokemonId.Doduo
        };

        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter = new Dictionary<PokemonId, TransferFilter>
        {
            //criteria: based on NY Central Park and Tokyo variety + sniping optimization
            {PokemonId.Golduck, new TransferFilter(1800, 6, false, 95, "or", 1)},
            {PokemonId.Farfetchd, new TransferFilter(1250, 6, false, 80, "or", 1)},
            {PokemonId.Krabby, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Kangaskhan, new TransferFilter(1500, 6, false, 60, "or", 1)},
            {PokemonId.Horsea, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Staryu, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.MrMime, new TransferFilter(1250, 6, false, 40, "or", 1)},
            {PokemonId.Scyther, new TransferFilter(1800, 6, false, 80, "or", 1)},
            {PokemonId.Jynx, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Electabuzz, new TransferFilter(1250, 6, false, 80, "or", 1)},
            {PokemonId.Magmar, new TransferFilter(1500, 6, false, 80, "or", 1)},
            {PokemonId.Pinsir, new TransferFilter(1800, 6, false, 95, "or", 1)},
            {PokemonId.Tauros, new TransferFilter(1250, 6, false, 90, "or", 1)},
            {PokemonId.Magikarp, new TransferFilter(200, 6, false, 95, "or", 1)},
            {PokemonId.Gyarados, new TransferFilter(1250, 6, false, 90, "or", 1)},
            {PokemonId.Lapras, new TransferFilter(1800, 6, false, 80, "or", 1)},
            {PokemonId.Eevee, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Vaporeon, new TransferFilter(1500, 6, false, 90, "or", 1)},
            {PokemonId.Jolteon, new TransferFilter(1500, 6, false, 90, "or", 1)},
            {PokemonId.Flareon, new TransferFilter(1500, 6, false, 90, "or", 1)},
            {PokemonId.Porygon, new TransferFilter(1250, 6, false, 60, "or", 1)},
            {PokemonId.Snorlax, new TransferFilter(2600, 6, false, 90, "or", 1)},
            {PokemonId.Dragonite, new TransferFilter(2600, 6, false, 90, "or", 1)}
        };

        public SnipeSettings PokemonToSnipe = new SnipeSettings
        {
            Locations = new List<Location>
            {
                new Location(38.55680748646112, -121.2383794784546), //Dratini Spot
                new Location(-33.85901900, 151.21309800), //Magikarp Spot
                new Location(47.5014969, -122.0959568), //Eevee Spot
                new Location(51.5025343, -0.2055027) //Charmender Spot
            },
            Pokemon = new List<PokemonId>
            {
                PokemonId.Venusaur,
                PokemonId.Charizard,
                PokemonId.Blastoise,
                PokemonId.Beedrill,
                PokemonId.Raichu,
                PokemonId.Sandslash,
                PokemonId.Nidoking,
                PokemonId.Nidoqueen,
                PokemonId.Clefable,
                PokemonId.Ninetales,
                PokemonId.Golbat,
                PokemonId.Vileplume,
                PokemonId.Golduck,
                PokemonId.Primeape,
                PokemonId.Arcanine,
                PokemonId.Poliwrath,
                PokemonId.Alakazam,
                PokemonId.Machamp,
                PokemonId.Golem,
                PokemonId.Rapidash,
                PokemonId.Slowbro,
                PokemonId.Farfetchd,
                PokemonId.Muk,
                PokemonId.Cloyster,
                PokemonId.Gengar,
                PokemonId.Exeggutor,
                PokemonId.Marowak,
                PokemonId.Hitmonchan,
                PokemonId.Lickitung,
                PokemonId.Rhydon,
                PokemonId.Chansey,
                PokemonId.Kangaskhan,
                PokemonId.Starmie,
                PokemonId.MrMime,
                PokemonId.Scyther,
                PokemonId.Magmar,
                PokemonId.Electabuzz,
                PokemonId.Jynx,
                PokemonId.Gyarados,
                PokemonId.Lapras,
                PokemonId.Ditto,
                PokemonId.Vaporeon,
                PokemonId.Jolteon,
                PokemonId.Flareon,
                PokemonId.Porygon,
                PokemonId.Kabutops,
                PokemonId.Aerodactyl,
                PokemonId.Snorlax,
                PokemonId.Articuno,
                PokemonId.Zapdos,
                PokemonId.Moltres,
                PokemonId.Dragonite,
                PokemonId.Mewtwo,
                PokemonId.Mew
            }
        };

        public List<PokemonId> PokemonToUseMasterball = new List<PokemonId>
        {
            PokemonId.Articuno,
            PokemonId.Zapdos,
            PokemonId.Moltres,
            PokemonId.Mew,
            PokemonId.Mewtwo
        };

        [DefaultValue(false)] public bool PrioritizeIvOverCp;
        [JsonIgnore] public string ProfileConfigPath;
        [JsonIgnore] public string ProfilePath;
        [DefaultValue(false)] public bool RandomizeRecycle;
        [DefaultValue(5)] public int RandomRecycleValue;
        [DefaultValue(90.0)] public double RecycleInventoryAtUsagePercentage;
        [DefaultValue(true)] public bool RenameOnlyAboveIv;
        //rename
        [DefaultValue(false)] public bool RenamePokemon;
        [DefaultValue("{1}_{0}")] public string RenameTemplate;
        [DefaultValue(true)] public bool ShowVariantWalking;
        [DefaultValue(false)] public bool SnipeAtPokestops;
        [DefaultValue(false)] public bool SnipeIgnoreUnknownIv;
        [DefaultValue("localhost")] public string SnipeLocationServer;
        [DefaultValue(16969)] public int SnipeLocationServerPort;
        [DefaultValue(false)] public bool SnipePokemonNotInPokedex;
        [DefaultValue(true)] public bool SnipeWithSkiplagged;
        [DefaultValue(0.005)] public double SnipingScanOffset;
        //pressakeyshit
        [DefaultValue(false)] public bool StartupWelcomeDelay;
        [DefaultValue(null)] public string TelegramApiKey;
        [DefaultValue(50)] public int TotalAmountOfBerriesToKeep;
        [DefaultValue(120)] public int TotalAmountOfPokeballsToKeep;
        [DefaultValue(80)] public int TotalAmountOfPotionsToKeep;
        [DefaultValue(60)] public int TotalAmountOfRevivesToKeep;
        [DefaultValue(true)] public bool TransferConfigAndAuthOnUpdate;
        [DefaultValue(true)] public bool TransferDuplicatePokemon;
        [DefaultValue(true)] public bool TransferDuplicatePokemonOnCapture;
        //transfer
        [DefaultValue(false)] public bool TransferWeakPokemon;

        [DefaultValue("en")] public string TranslationLanguageCode;
        [DefaultValue(1000)] public float UpgradePokemonCpMinimum;
        [DefaultValue(95)] public float UpgradePokemonIvMinimum;
        [DefaultValue("and")] public string UpgradePokemonMinimumStatsOperator;
        [DefaultValue(0.20)] public double UseBerriesBelowCatchProbability;
        [DefaultValue(1000)] public int UseBerriesMinCp;
        [DefaultValue(90)] public float UseBerriesMinIv;
        [DefaultValue("or")] public string UseBerriesOperator;
        //lucky, incense and berries
        [DefaultValue(true)] public bool UseEggIncubators;
        //gpx
        [DefaultValue(false)] public bool UseGpxPathing;
        //balls
        [DefaultValue(1000)] public int UseGreatBallAboveCp;
        [DefaultValue(85.0)] public double UseGreatBallAboveIv;
        [DefaultValue(0.2)] public double UseGreatBallBelowCatchProbability;
        [DefaultValue(false)] public bool UseIncenseConstantly;
        [DefaultValue(false)] public bool UseKeepMinLvl;

        [DefaultValue(true)] public bool UseLevelUpList;
        [DefaultValue(false)] public bool UseLuckyEggConstantly;
        [DefaultValue(30)] public int UseLuckyEggsMinPokemonAmount;
        [DefaultValue(false)] public bool UseLuckyEggsWhileEvolving;
        [DefaultValue(1500)] public int UseMasterBallAboveCp;
        [DefaultValue(0.05)] public double UseMasterBallBelowCatchProbability;
        [DefaultValue(false)] public bool UsePokemonSniperFilterOnly;
        //notcatch
        [DefaultValue(false)] public bool UsePokemonToNotCatchFilter;
        //snipe
        [DefaultValue(false)] public bool UseSnipeLocationServer;
        //Telegram
        [DefaultValue(false)] public bool UseTelegramApi;
        [DefaultValue(false)] public bool UseTransferIvForSnipe;
        [DefaultValue(1250)] public int UseUltraBallAboveCp;
        [DefaultValue(95.0)] public double UseUltraBallAboveIv;
        [DefaultValue(0.1)] public double UseUltraBallBelowCatchProbability;
        [DefaultValue(true)] public bool UseWalkingSpeedVariant;
        //websockets
        [DefaultValue(false)] public bool UseWebsocket;
        //recycle
        [DefaultValue(true)] public bool VerboseRecycling;
        [DefaultValue(19.0)] public double WalkingSpeedInKilometerPerHour;
        [DefaultValue(2)] public double WalkingSpeedOffSetInKilometerPerHour;
        [DefaultValue(1.2)] public double WalkingSpeedVariant;
        [DefaultValue(14251)] public int WebSocketPort;

        public GlobalSettings()
        {
            InitializePropertyDefaultValues(this);
        }

        public static GlobalSettings Default => new GlobalSettings();

        public void InitializePropertyDefaultValues(object obj)
        {
            var fields = obj.GetType().GetFields();

            foreach (var field in fields)
            {
                var d = field.GetCustomAttribute<DefaultValueAttribute>();

                if (d != null)
                    field.SetValue(obj, d.Value);
            }
        }

        public static GlobalSettings Load(string path, bool boolSkipSave = false)
        {
            GlobalSettings settings;
            var isGui =
                AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.FullName.Contains("PoGo.NecroBot.GUI")) !=
                null;
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), path);
            var profileConfigPath = Path.Combine(profilePath, "config");
            var configFile = Path.Combine(profileConfigPath, "config.json");
            var shouldExit = false;

            if (File.Exists(configFile))
            {
                try
                {
                    //if the file exists, load the settings
                    string input;
                    var count = 0;
                    while (true)
                    {
                        try
                        {
                            input = File.ReadAllText(configFile);
                            break;
                        }
                        catch (Exception exception)
                        {
                            if (count > 10)
                            {
                                //sometimes we have to wait close to config.json for access
                                Logger.Write("configFile: " + exception.Message, LogLevel.Error);
                            }
                            count++;
                            Thread.Sleep(1000);
                        }
                    }

                    var jsonSettings = new JsonSerializerSettings();
                    jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                    jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                    jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                    settings = JsonConvert.DeserializeObject<GlobalSettings>(input, jsonSettings);

                    //This makes sure that existing config files dont get null values which lead to an exception
                    foreach (var filter in settings.PokemonsTransferFilter.Where(x => x.Value.KeepMinOperator == null))
                    {
                        filter.Value.KeepMinOperator = "or";
                    }
                    foreach (var filter in settings.PokemonsTransferFilter.Where(x => x.Value.Moves == null))
                    {
                        filter.Value.Moves = new List<PokemonMove>();
                    }
                    foreach (var filter in settings.PokemonsTransferFilter.Where(x => x.Value.MovesOperator == null))
                    {
                        filter.Value.MovesOperator = "or";
                    }
                }
                catch (JsonReaderException exception)
                {
                    Logger.Write("JSON Exception: " + exception.Message, LogLevel.Error);
                    return null;
                }
            }
            else
            {
                settings = new GlobalSettings();
                shouldExit = true;
            }


            settings.ProfilePath = profilePath;
            settings.ProfileConfigPath = profileConfigPath;
            settings.GeneralConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "config");
            settings.IsGui = isGui;

            if (!boolSkipSave || !settings.AutoUpdate)
            {
                settings.Save(configFile);
                settings.Auth.Load(Path.Combine(profileConfigPath, "auth.json"));
            }

            return shouldExit ? null : settings;
        }

        public void CheckProxy()
        {
            Auth.CheckProxy();
        }

        public static bool PromptForSetup(ITranslation translator)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartPrompt, "Y", "N"), LogLevel.Warning);

            while (true)
            {
                var readLine = Console.ReadLine();
                if (readLine != null)
                {
                    var strInput = readLine.ToLower();

                    switch (strInput)
                    {
                        case "y":
                            return true;
                        case "n":
                            Logger.Write(translator.GetTranslation(TranslationString.FirstStartAutoGenSettings));
                            return false;
                        default:
                            Logger.Write(translator.GetTranslation(TranslationString.PromptError, "Y", "N"),
                                LogLevel.Error);
                            continue;
                    }
                }
            }
        }

        public static Session SetupSettings(Session session, GlobalSettings settings, string configPath)
        {
            var newSession = SetupTranslationCode(session, session.Translation, settings);

            SetupAccountType(newSession.Translation, settings);
            SetupUserAccount(newSession.Translation, settings);
            SetupConfig(newSession.Translation, settings);
            SaveFiles(settings, configPath);

            Logger.Write(session.Translation.GetTranslation(TranslationString.FirstStartSetupCompleted), LogLevel.None);

            return newSession;
        }

        private static Session SetupTranslationCode(Session session, ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartLanguagePrompt, "Y", "N"), LogLevel.None);
            string strInput;

            var boolBreak = false;
            while (!boolBreak)
            {
                // ReSharper disable once PossibleNullReferenceException
                strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "y":
                        boolBreak = true;
                        break;
                    case "n":
                        return session;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartLanguageCodePrompt));
            strInput = Console.ReadLine();

            settings.TranslationLanguageCode = strInput;
            session = new Session(new ClientSettings(settings), new LogicSettings(settings));
            translator = session.Translation;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartLanguageConfirm, strInput));

            return session;
        }


        private static void SetupAccountType(ITranslation translator, GlobalSettings settings)
        {
            string strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAccount), LogLevel.None);
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTypePrompt, "google", "ptc"));

            while (true)
            {
                // ReSharper disable once PossibleNullReferenceException
                strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "google":
                        settings.Auth.AuthType = AuthType.Google;
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTypeConfirm, "GOOGLE"));
                        return;
                    case "ptc":
                        settings.Auth.AuthType = AuthType.Ptc;
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTypeConfirm, "PTC"));
                        return;
                    default:
                        Logger.Write(
                            translator.GetTranslation(TranslationString.FirstStartSetupTypePromptError, "google", "ptc"),
                            LogLevel.Error);
                        break;
                }
            }
        }

        private static void SetupUserAccount(ITranslation translator, GlobalSettings settings)
        {
            Console.WriteLine("");
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupUsernamePrompt), LogLevel.None);
            var strInput = Console.ReadLine();

            if (settings.Auth.AuthType == AuthType.Google)
                settings.Auth.GoogleUsername = strInput;
            else
                settings.Auth.PtcUsername = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupUsernameConfirm, strInput));

            Console.WriteLine("");
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupPasswordPrompt), LogLevel.None);
            strInput = Console.ReadLine();

            if (settings.Auth.AuthType == AuthType.Google)
                settings.Auth.GooglePassword = strInput;
            else
                settings.Auth.PtcPassword = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupPasswordConfirm, strInput));

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartAccountCompleted), LogLevel.None);
        }

        private static void SetupConfig(ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartDefaultLocationPrompt, "Y", "N"),
                LogLevel.None);

            var boolBreak = false;
            while (!boolBreak)
            {
                // ReSharper disable once PossibleNullReferenceException
                var strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "y":
                        boolBreak = true;
                        break;
                    case "n":
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartDefaultLocationSet));
                        return;
                    default:
                        // PROMPT ERROR \\
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartDefaultLocation), LogLevel.None);
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLatPrompt));
            while (true)
            {
                try
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var dblInput = double.Parse(Console.ReadLine());
                    settings.DefaultLatitude = dblInput;
                    Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLatConfirm, dblInput));
                    break;
                }
                catch (FormatException)
                {
                    Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLocationError,
                        settings.DefaultLatitude, LogLevel.Error));
                }
            }

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLongPrompt));
            while (true)
            {
                try
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var dblInput = double.Parse(Console.ReadLine());
                    settings.DefaultLongitude = dblInput;
                    Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLongConfirm, dblInput));
                    break;
                }
                catch (FormatException)
                {
                    Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLocationError,
                        settings.DefaultLongitude, LogLevel.Error));
                }
            }
        }

        private static void SaveFiles(GlobalSettings settings, string configFile)
        {
            settings.Save(configFile);
            settings.Auth.Load(Path.Combine(settings.ProfileConfigPath, "auth.json"));
        }

        public void Save(string fullPath)
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                Converters = new JsonConverter[] {new StringEnumConverter {CamelCaseText = true}}
            };

            var output = JsonConvert.SerializeObject(this, jsonSerializeSettings);

            var folder = Path.GetDirectoryName(fullPath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }
    }

    public class ClientSettings : ISettings
    {
        // Never spawn at the same position.
        private readonly Random _rand = new Random();
        private readonly GlobalSettings _settings;

        public ClientSettings(GlobalSettings settings)
        {
            _settings = settings;
        }


        public string GoogleUsername => _settings.Auth.GoogleUsername;
        public string GooglePassword => _settings.Auth.GooglePassword;

        double ISettings.DefaultLatitude
        {
            get
            {
                return _settings.DefaultLatitude + _rand.NextDouble()*((double) _settings.MaxSpawnLocationOffset/111111);
            }

            set { _settings.DefaultLatitude = value; }
        }

        double ISettings.DefaultLongitude
        {
            get
            {
                return _settings.DefaultLongitude +
                       _rand.NextDouble()*
                       ((double) _settings.MaxSpawnLocationOffset/111111/Math.Cos(_settings.DefaultLatitude));
            }

            set { _settings.DefaultLongitude = value; }
        }

        double ISettings.DefaultAltitude
        {
            get
            {
                return
                    LocationUtils.getElevation(_settings.DefaultLatitude, _settings.DefaultLongitude) +
                    _rand.NextDouble()*
                    (5/
                     Math.Cos(LocationUtils.getElevation(_settings.DefaultLatitude, _settings.DefaultLongitude)));
            }


            set { }
        }

        #region Auth Config Values

        public bool UseProxy
        {
            get { return _settings.Auth.UseProxy; }
            set { _settings.Auth.UseProxy = value; }
        }

        public string UseProxyHost
        {
            get { return _settings.Auth.UseProxyHost; }
            set { _settings.Auth.UseProxyHost = value; }
        }

        public string UseProxyPort
        {
            get { return _settings.Auth.UseProxyPort; }
            set { _settings.Auth.UseProxyPort = value; }
        }

        public bool UseProxyAuthentication
        {
            get { return _settings.Auth.UseProxyAuthentication; }
            set { _settings.Auth.UseProxyAuthentication = value; }
        }

        public string UseProxyUsername
        {
            get { return _settings.Auth.UseProxyUsername; }
            set { _settings.Auth.UseProxyUsername = value; }
        }

        public string UseProxyPassword
        {
            get { return _settings.Auth.UseProxyPassword; }
            set { _settings.Auth.UseProxyPassword = value; }
        }

        public string GoogleRefreshToken
        {
            get { return null; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                GoogleRefreshToken = null;
            }
        }

        AuthType ISettings.AuthType
        {
            get { return _settings.Auth.AuthType; }

            set { _settings.Auth.AuthType = value; }
        }

        string ISettings.GoogleUsername
        {
            get { return _settings.Auth.GoogleUsername; }

            set { _settings.Auth.GoogleUsername = value; }
        }

        string ISettings.GooglePassword
        {
            get { return _settings.Auth.GooglePassword; }

            set { _settings.Auth.GooglePassword = value; }
        }

        string ISettings.PtcUsername
        {
            get { return _settings.Auth.PtcUsername; }

            set { _settings.Auth.PtcUsername = value; }
        }

        string ISettings.PtcPassword
        {
            get { return _settings.Auth.PtcPassword; }

            set { _settings.Auth.PtcPassword = value; }
        }

        #endregion Auth Config Values

        #region Device Config Values

        private string DevicePackageName
        {
            get { return _settings.Auth.DevicePackageName; }
            set { _settings.Auth.DevicePackageName = value; }
        }

        string ISettings.DeviceId
        {
            get { return _settings.Auth.DeviceId; }
            set { _settings.Auth.DeviceId = value; }
        }

        string ISettings.AndroidBoardName
        {
            get { return _settings.Auth.AndroidBoardName; }
            set { _settings.Auth.AndroidBoardName = value; }
        }

        string ISettings.AndroidBootloader
        {
            get { return _settings.Auth.AndroidBootloader; }
            set { _settings.Auth.AndroidBootloader = value; }
        }

        string ISettings.DeviceBrand
        {
            get { return _settings.Auth.DeviceBrand; }
            set { _settings.Auth.DeviceBrand = value; }
        }

        string ISettings.DeviceModel
        {
            get { return _settings.Auth.DeviceModel; }
            set { _settings.Auth.DeviceModel = value; }
        }

        string ISettings.DeviceModelIdentifier
        {
            get { return _settings.Auth.DeviceModelIdentifier; }
            set { _settings.Auth.DeviceModelIdentifier = value; }
        }

        string ISettings.DeviceModelBoot
        {
            get { return _settings.Auth.DeviceModelBoot; }
            set { _settings.Auth.DeviceModelBoot = value; }
        }

        string ISettings.HardwareManufacturer
        {
            get { return _settings.Auth.HardwareManufacturer; }
            set { _settings.Auth.HardwareManufacturer = value; }
        }

        string ISettings.HardwareModel
        {
            get { return _settings.Auth.HardwareModel; }
            set { _settings.Auth.HardwareModel = value; }
        }

        string ISettings.FirmwareBrand
        {
            get { return _settings.Auth.FirmwareBrand; }
            set { _settings.Auth.FirmwareBrand = value; }
        }

        string ISettings.FirmwareTags
        {
            get { return _settings.Auth.FirmwareTags; }
            set { _settings.Auth.FirmwareTags = value; }
        }

        string ISettings.FirmwareType
        {
            get { return _settings.Auth.FirmwareType; }
            set { _settings.Auth.FirmwareType = value; }
        }

        string ISettings.FirmwareFingerprint
        {
            get { return _settings.Auth.FirmwareFingerprint; }
            set { _settings.Auth.FirmwareFingerprint = value; }
        }

        #endregion Device Config Values
    }

    public class LogicSettings : ILogicSettings
    {
        private readonly GlobalSettings _settings;

        public LogicSettings(GlobalSettings settings)

        {
            _settings = settings;
        }

        public string GoogleAPIKey => _settings.Auth.GoogleApiKey;
        public string ProfilePath => _settings.ProfilePath;
        public string ProfileConfigPath => _settings.ProfileConfigPath;
        public string GeneralConfigPath => _settings.GeneralConfigPath;
        public bool AutoUpdate => _settings.AutoUpdate;
        public bool TransferConfigAndAuthOnUpdate => _settings.TransferConfigAndAuthOnUpdate;
        public bool UseWebsocket => _settings.UseWebsocket;
        public bool CatchPokemon => _settings.CatchPokemon;
        public bool TransferWeakPokemon => _settings.TransferWeakPokemon;
        public bool DisableHumanWalking => _settings.DisableHumanWalking;
        public int MaxBerriesToUsePerPokemon => _settings.MaxBerriesToUsePerPokemon;
        public float KeepMinIvPercentage => _settings.KeepMinIvPercentage;
        public string KeepMinOperator => _settings.KeepMinOperator;
        public int KeepMinCp => _settings.KeepMinCp;
        public int KeepMinLvl => _settings.KeepMinLvl;
        public bool UseKeepMinLvl => _settings.UseKeepMinLvl;
        public bool AutomaticallyLevelUpPokemon => _settings.AutomaticallyLevelUpPokemon;
        public bool OnlyUpgradeFavorites => _settings.OnlyUpgradeFavorites;
        public bool UseLevelUpList => _settings.UseLevelUpList;
        public int AmountOfTimesToUpgradeLoop => _settings.AmountOfTimesToUpgradeLoop;
        public string LevelUpByCPorIv => _settings.LevelUpByCPorIv;
        public int GetMinStarDustForLevelUp => _settings.GetMinStarDustForLevelUp;
        public bool UseLuckyEggConstantly => _settings.UseLuckyEggConstantly;
        public bool UseIncenseConstantly => _settings.UseIncenseConstantly;
        public int UseBerriesMinCp => _settings.UseBerriesMinCp;
        public float UseBerriesMinIv => _settings.UseBerriesMinIv;
        public double UseBerriesBelowCatchProbability => _settings.UseBerriesBelowCatchProbability;
        public string UseBerriesOperator => _settings.UseBerriesOperator;
        public float UpgradePokemonIvMinimum => _settings.UpgradePokemonIvMinimum;
        public float UpgradePokemonCpMinimum => _settings.UpgradePokemonCpMinimum;
        public string UpgradePokemonMinimumStatsOperator => _settings.UpgradePokemonMinimumStatsOperator;
        public double WalkingSpeedInKilometerPerHour => _settings.WalkingSpeedInKilometerPerHour;
        public double WalkingSpeedOffSetInKilometerPerHour => _settings.WalkingSpeedOffSetInKilometerPerHour;
        public bool UseWalkingSpeedVariant => _settings.UseWalkingSpeedVariant;
        public double WalkingSpeedVariant => _settings.WalkingSpeedVariant;
        public bool ShowVariantWalking => _settings.ShowVariantWalking;
        public bool FastSoftBanBypass => _settings.FastSoftBanBypass;
        public bool EvolveAllPokemonWithEnoughCandy => _settings.EvolveAllPokemonWithEnoughCandy;
        public bool KeepPokemonsThatCanEvolve => _settings.KeepPokemonsThatCanEvolve;
        public bool TransferDuplicatePokemon => _settings.TransferDuplicatePokemon;
        public bool TransferDuplicatePokemonOnCapture => _settings.TransferDuplicatePokemonOnCapture;
        public bool UseEggIncubators => _settings.UseEggIncubators;
        public int UseGreatBallAboveCp => _settings.UseGreatBallAboveCp;
        public int UseUltraBallAboveCp => _settings.UseUltraBallAboveCp;
        public int UseMasterBallAboveCp => _settings.UseMasterBallAboveCp;
        public double UseGreatBallAboveIv => _settings.UseGreatBallAboveIv;
        public double UseUltraBallAboveIv => _settings.UseUltraBallAboveIv;
        public double UseMasterBallBelowCatchProbability => _settings.UseMasterBallBelowCatchProbability;
        public double UseUltraBallBelowCatchProbability => _settings.UseUltraBallBelowCatchProbability;
        public double UseGreatBallBelowCatchProbability => _settings.UseGreatBallBelowCatchProbability;
        public bool EnableHumanizedThrows => _settings.EnableHumanizedThrows;
        public int NiceThrowChance => _settings.NiceThrowChance;
        public int GreatThrowChance => _settings.GreatThrowChance;
        public int ExcellentThrowChance => _settings.ExcellentThrowChance;
        public int CurveThrowChance => _settings.CurveThrowChance;
        public double ForceGreatThrowOverIv => _settings.ForceGreatThrowOverIv;
        public double ForceExcellentThrowOverIv => _settings.ForceExcellentThrowOverIv;
        public int ForceGreatThrowOverCp => _settings.ForceGreatThrowOverCp;
        public int ForceExcellentThrowOverCp => _settings.ForceExcellentThrowOverCp;
        public int DelayBetweenPokemonCatch => _settings.DelayBetweenPokemonCatch;
        public int DelayBetweenPlayerActions => _settings.DelayBetweenPlayerActions;
        public bool UsePokemonToNotCatchFilter => _settings.UsePokemonToNotCatchFilter;
        public bool UsePokemonSniperFilterOnly => _settings.UsePokemonSniperFilterOnly;
        public int KeepMinDuplicatePokemon => _settings.KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp => _settings.PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters => _settings.MaxTravelDistanceInMeters;
        public string GpxFile => _settings.GpxFile;
        public bool UseGpxPathing => _settings.UseGpxPathing;
        public bool UseLuckyEggsWhileEvolving => _settings.UseLuckyEggsWhileEvolving;
        public int UseLuckyEggsMinPokemonAmount => _settings.UseLuckyEggsMinPokemonAmount;
        public bool EvolveAllPokemonAboveIv => _settings.EvolveAllPokemonAboveIv;
        public float EvolveAboveIvValue => _settings.EvolveAboveIvValue;
        public bool RenamePokemon => _settings.RenamePokemon;
        public bool RenameOnlyAboveIv => _settings.RenameOnlyAboveIv;
        public float FavoriteMinIvPercentage => _settings.FavoriteMinIvPercentage;
        public bool AutoFavoritePokemon => _settings.AutoFavoritePokemon;
        public string RenameTemplate => _settings.RenameTemplate;
        public int AmountOfPokemonToDisplayOnStart => _settings.AmountOfPokemonToDisplayOnStart;
        public bool DumpPokemonStats => _settings.DumpPokemonStats;
        public string TranslationLanguageCode => _settings.TranslationLanguageCode;
        public bool DetailedCountsBeforeRecycling => _settings.DetailedCountsBeforeRecycling;
        public bool VerboseRecycling => _settings.VerboseRecycling;
        public double RecycleInventoryAtUsagePercentage => _settings.RecycleInventoryAtUsagePercentage;

        public double EvolveKeptPokemonsAtStorageUsagePercentage => _settings.EvolveKeptPokemonsAtStorageUsagePercentage
            ;

        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter;
        public ICollection<PokemonId> PokemonsToEvolve => _settings.PokemonsToEvolve;
        public ICollection<PokemonId> PokemonsToLevelUp => _settings.PokemonsToLevelUp;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;

        public ICollection<PokemonId> PokemonToUseMasterball => _settings.PokemonToUseMasterball;
        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter => _settings.PokemonsTransferFilter;
        public bool StartupWelcomeDelay => _settings.StartupWelcomeDelay;
        public bool SnipeAtPokestops => _settings.SnipeAtPokestops;

        public bool UseTelegramAPI => _settings.UseTelegramApi;
        public string TelegramAPIKey => _settings.TelegramApiKey;

        public int MinPokeballsToSnipe => _settings.MinPokeballsToSnipe;
        public int MinPokeballsWhileSnipe => _settings.MinPokeballsWhileSnipe;
        public int MaxPokeballsPerPokemon => _settings.MaxPokeballsPerPokemon;

        public SnipeSettings PokemonToSnipe => _settings.PokemonToSnipe;
        public string SnipeLocationServer => _settings.SnipeLocationServer;
        public int SnipeLocationServerPort => _settings.SnipeLocationServerPort;
        public bool GetSniperInfoFromPokezz => _settings.GetSniperInfoFromPokezz;
        public bool GetOnlyVerifiedSniperInfoFromPokezz => _settings.GetOnlyVerifiedSniperInfoFromPokezz;
        public bool GetSniperInfoFromPokeSnipers => _settings.GetSniperInfoFromPokeSnipers;
        public bool GetSniperInfoFromPokeWatchers => _settings.GetSniperInfoFromPokeWatchers;
        public bool SnipeWithSkiplagged => _settings.SnipeWithSkiplagged;
        public bool UseSnipeLocationServer => _settings.UseSnipeLocationServer;
        public bool UseTransferIvForSnipe => _settings.UseTransferIvForSnipe;
        public bool SnipeIgnoreUnknownIv => _settings.SnipeIgnoreUnknownIv;
        public int MinDelayBetweenSnipes => _settings.MinDelayBetweenSnipes;
        public double SnipingScanOffset => _settings.SnipingScanOffset;
        public bool SnipePokemonNotInPokedex => _settings.SnipePokemonNotInPokedex;
        public bool RandomizeRecycle => _settings.RandomizeRecycle;
        public int RandomRecycleValue => _settings.RandomRecycleValue;
        public bool DelayBetweenRecycleActions => _settings.DelayBetweenRecycleActions;
        public int TotalAmountOfPokeballsToKeep => _settings.TotalAmountOfPokeballsToKeep;
        public int TotalAmountOfPotionsToKeep => _settings.TotalAmountOfPotionsToKeep;
        public int TotalAmountOfRevivesToKeep => _settings.TotalAmountOfRevivesToKeep;
        public int TotalAmountOfBerriesToKeep => _settings.TotalAmountOfBerriesToKeep;
    }
}