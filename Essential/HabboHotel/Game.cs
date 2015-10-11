using System;
using System.Data;
using System.Threading.Tasks;
using Essential.HabboHotel.Misc;
using Essential.Core;
using Essential.HabboHotel.Navigators;
using Essential.HabboHotel.Catalogs;
using Essential.HabboHotel.Support;
using Essential.HabboHotel.Roles;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Pets;
using Essential.HabboHotel.Achievements;
using Essential.HabboHotel.RoomBots;
using Essential.HabboHotel.Quests;
using Essential.Util;
using Essential.Storage;
using System.Threading;
using Essential.HabboHotel.Games;
using Essential.HabboHotel.Games.SnowWar;
using Essential.HabboHotel.Guides;
namespace Essential.HabboHotel
{
	internal sealed class Game
	{
		private GameClientManager ClientManager;
		private ModerationBanManager BanManager;
		private RoleManager RoleManager;
		private HelpTool HelpTool;
		private Catalog Catalog;
		private Navigator Navigator;
		private ItemManager ItemManager;
		private RoomManager RoomManager;
		private PixelManager PixelManager;
		private AchievementManager AchievementManager;
		private ModerationTool ModerationTool;
		private BotManager BotManager;
		private Task task_0;
		private NavigatorCache NavigatorCache;
		private Marketplace Marketplace;
		private QuestManager QuestManager;
		private EssentialEnvironment EssentialEnvironment;
		private Groups Groups;
        private GamesManager GamesManager;
        private Task GameLoop;
        private bool GameLoopActive;
        private bool GameLoopEnded = true;
        private const int GameLoopSleepTime = 25;
        private WarsData StormWars;
        private GuideManager guideManager;
		public Game(int conns)
		{
			this.ClientManager = new GameClientManager(conns);

			if (Essential.GetConfig().data["client.ping.enabled"] == "1")
			{
				this.ClientManager.StartPingTask();
			}

			DateTime now = DateTime.Now;

			Logging.Write("Connecting to the database.. ");

            try
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    Logging.WriteLine("completed!", ConsoleColor.Green);

                    Essential.Game = this;
                    this.LoadServerSettings(dbClient);
                    this.BanManager = new ModerationBanManager();
                    this.RoleManager = new RoleManager();
                    this.HelpTool = new HelpTool();
                    this.Catalog = new Catalog();
                    this.Navigator = new Navigator();
                    this.ItemManager = new ItemManager();
                    this.RoomManager = new RoomManager();
                    this.PixelManager = new PixelManager();
                    this.AchievementManager = new AchievementManager();
                    this.ModerationTool = new ModerationTool();
                    this.BotManager = new BotManager();
                    this.Marketplace = new Marketplace();
                    this.QuestManager = new QuestManager();
                    this.EssentialEnvironment = new EssentialEnvironment();
                    this.GamesManager = new GamesManager();
                    this.Groups = new Groups();
                    this.StormWars = new WarsData();
                    EssentialEnvironment.LoadExternalTexts(dbClient);

                    this.BanManager.Initialise(dbClient);

                    this.RoleManager.Initialize(dbClient);

                    this.HelpTool.method_0(dbClient);
                    this.HelpTool.method_3(dbClient);

                    this.ModerationTool.method_1(dbClient);
                    this.ModerationTool.method_2(dbClient);
                    this.ItemManager.Initialize(dbClient);
                    this.Catalog.Initialize(dbClient);
                    this.Catalog.InitializeCache();
                    PetRace.Init(dbClient);
                    this.Navigator.Initialize(dbClient);
                    this.RoomManager.method_8(dbClient);
                    this.RoomManager.method_0();
                    this.NavigatorCache = new NavigatorCache();
                    this.RoomManager.LoadMagicTiles(dbClient);
                    this.RoomManager.LoadBillboards(dbClient);
                    this.BotManager.Initialize(dbClient);
                    AchievementManager.Load(dbClient);
                    this.PixelManager.Initialize();
                    ChatCommandHandler.Initialize(dbClient);
                    this.QuestManager.Initialize();
                   // this.GamesManager.LoadGameLocales(dbClient);
                    // this.GamesManager.LoadPowerupPackages(dbClient);
                    Groups.Initialize(dbClient);
                    guideManager = new GuideManager(dbClient);
                    this.RestoreStatistics(dbClient, 1);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                Logging.WriteLine("failed!", ConsoleColor.Red);
                Logging.WriteLine(e.Message + " Check the given configuration details in config.conf\r\n", ConsoleColor.Yellow);
                Essential.Destroy("", true, true);

                return;
            }

			this.task_0 = new Task(new Action(LowPriorityWorker.Work));
			this.task_0.Start();

            StartGameLoop();
		}

		public void RestoreStatistics(DatabaseClient dbClient, int status)
		{
			Logging.Write(EssentialEnvironment.GetExternalText("emu_cleandb"));
			bool flag = true;
			try
			{
				if (int.Parse(Essential.GetConfig().data["debug"]) == 1)
				{
					flag = false;
				}
			}
			catch
			{
			}
			if (flag)
			{
				dbClient.ExecuteQuery("UPDATE users SET online = '0' WHERE online != '0'");
				dbClient.ExecuteQuery("UPDATE rooms SET users_now = '0' WHERE users_now != '0'");
				dbClient.ExecuteQuery("UPDATE user_roomvisits SET exit_timestamp = UNIX_TIMESTAMP() WHERE exit_timestamp <= 0");
                dbClient.ExecuteQuery("UPDATE users SET websocket='0'");
                dbClient.ExecuteQuery(string.Concat(new object[]
				{
					"UPDATE server_status SET status = '",
					status,
					"', users_online = '0', rooms_loaded = '0', server_ver = '",
					Essential.PrettyVersion,
					"', stamp = UNIX_TIMESTAMP() LIMIT 1;"
				}));
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}

		public void ContinueLoading()
		{
			if (this.task_0 != null)
			{
				this.task_0 = null;
			}


                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    this.RestoreStatistics(dbClient, 0);
                }

			if (this.GetClientManager() != null)
			{
				this.GetClientManager().StopPingTask();
			}

			if (this.GetPixelManager() != null)
			{
				this.PixelManager.KeepAlive = false;
			}

			this.ClientManager = null;
			this.BanManager = null;
			this.RoleManager = null;
			this.HelpTool = null;
			this.Catalog = null;
			this.Navigator = null;
			this.ItemManager = null;
			this.RoomManager = null;
			this.PixelManager = null;
		}

		public GameClientManager GetClientManager()
		{
			return this.ClientManager;
		}

		public ModerationBanManager GetBanManager()
		{
			return this.BanManager;
		}

		public RoleManager GetRoleManager()
		{
			return this.RoleManager;
		}

		public HelpTool GetHelpTool()
		{
			return this.HelpTool;
		}

		public Catalog GetCatalog()
		{
			return this.Catalog;
		}

		public Navigator GetNavigator()
		{
			return this.Navigator;
		}

		public ItemManager GetItemManager()
		{
			return this.ItemManager;
		}

		public RoomManager GetRoomManager()
		{
			return this.RoomManager;
		}

		public PixelManager GetPixelManager()
		{
			return this.PixelManager;
		}

		public AchievementManager GetAchievementManager()
		{
			return this.AchievementManager;
		}

        public GamesManager GetGamesManager()
        {
            return this.GamesManager;
        }
		public ModerationTool GetModerationTool()
		{
			return this.ModerationTool;
		}

		public BotManager GetBotManager()
		{
			return this.BotManager;
		}

		internal NavigatorCache GetNavigatorCache()
		{
			return this.NavigatorCache;
		}

		public QuestManager GetQuestManager()
		{
			return this.QuestManager;
		}
        internal WarsData GetStormWars()
        {
            return this.StormWars;
        }
        internal GuideManager GetGuideManager()
        {
            return this.guideManager;
        }
		public void LoadServerSettings(DatabaseClient class6_0)
		{
			Logging.Write("Loading your settings..");

			DataRow dataRow = class6_0.ReadDataRow("SELECT * FROM server_settings LIMIT 1");

			ServerConfiguration.RoomUserLimit = (int)dataRow["MaxRoomsPerUser"];

			ServerConfiguration.MOTD = (string)dataRow["motd"];

			ServerConfiguration.CreditingInterval = (int)dataRow["timer"];

			ServerConfiguration.CreditingAmount = (int)dataRow["credits"];
			ServerConfiguration.PointingAmount = (int)dataRow["pixels"];
			ServerConfiguration.PixelingAmount = (int)dataRow["points"];

			ServerConfiguration.PixelLimit = (int)dataRow["pixels_max"];
			ServerConfiguration.CreditLimit = (int)dataRow["credits_max"];
			ServerConfiguration.PointLimit = (int)dataRow["points_max"];

			ServerConfiguration.PetsPerRoomLimit = (int)dataRow["MaxPetsPerRoom"];

			ServerConfiguration.MarketplacePriceLimit = (int)dataRow["MaxMarketPlacePrice"];
			ServerConfiguration.MarketplaceTax = (int)dataRow["MarketPlaceTax"];

			ServerConfiguration.DDoSProtectionEnabled = Essential.StringToBoolean(dataRow["enable_antiddos"].ToString());

			ServerConfiguration.HabboClubForClothes = Essential.StringToBoolean(dataRow["vipclothesforhcusers"].ToString());

			ServerConfiguration.EnableChatlog = Essential.StringToBoolean(dataRow["enable_chatlogs"].ToString());
			ServerConfiguration.EnableCommandLog = Essential.StringToBoolean(dataRow["enable_cmdlogs"].ToString());
			ServerConfiguration.EnableRoomLog = Essential.StringToBoolean(dataRow["enable_roomlogs"].ToString());

			ServerConfiguration.EnableExternalLinks = (string)dataRow["enable_externalchatlinks"];

			ServerConfiguration.EnableSSO = Essential.StringToBoolean(dataRow["enable_securesessions"].ToString());

			ServerConfiguration.AllowFurniDrops = Essential.StringToBoolean(dataRow["allow_friendfurnidrops"].ToString());

			ServerConfiguration.EnableRedeemCredits = Essential.StringToBoolean(dataRow["enable_cmd_redeemcredits"].ToString());
            ServerConfiguration.EnableRedeemPixels = Essential.StringToBoolean(dataRow["enable_cmd_redeempixels"].ToString());
            ServerConfiguration.EnableRedeemShells = Essential.StringToBoolean(dataRow["enable_cmd_redeemshells"].ToString());

			ServerConfiguration.UnloadCrashedRooms = Essential.StringToBoolean(dataRow["unload_crashedrooms"].ToString());

			ServerConfiguration.ShowUsersAndRoomsInAbout = Essential.StringToBoolean(dataRow["ShowUsersAndRoomsInAbout"].ToString());

			ServerConfiguration.SleepTimer = (int)dataRow["idlesleep"];
			ServerConfiguration.KickTimer = (int)dataRow["idlekick"];

			ServerConfiguration.IPLastBan = Essential.StringToBoolean(dataRow["ip_lastforbans"].ToString());

            ServerConfiguration.StaffPicksID = (int)dataRow["StaffPicksCategoryID"];

            ServerConfiguration.VIPHotelAlertInterval = (double)dataRow["vipha_interval"];
            ServerConfiguration.VIPHotelAlertLinkInterval =  (double)dataRow["viphal_interval"];
          //  ServerConfiguration.BasejumpMaintenance = Essential.StringToBoolean(dataRow["basejump_maintenance"].ToString());

            ServerConfiguration.PreventDoorPush = Essential.StringToBoolean(dataRow["DisableOtherUsersToMovingOtherUsersToDoor"].ToString());
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}

        internal void StartGameLoop()
        {
            GameLoopEnded = false;
            GameLoopActive = true;
            GameLoop = new Task(MainGameLoop);
            GameLoop.Start();
        }

        internal void StopGameLoop()
        {
            GameLoopActive = false;

            while (!GameLoopEnded)
            {
                Thread.Sleep(GameLoopSleepTime);
            }
        }

        private void MainGameLoop()
        {
            while (GameLoopActive)
            {
                try
                {
                    RoomManager.OnCycle();
                }
                catch
                {
                }
                Thread.Sleep(GameLoopSleepTime);
            }

            GameLoopEnded = true;
        }
	}
}
