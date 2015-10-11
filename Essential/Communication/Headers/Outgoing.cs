using System;
namespace Essential
{
    internal sealed class Outgoing
    {
        //I did a lot of copy & Paste from Swift for the packets, every Header I updated (copied from Swift) is marked with //Rootkit
        public const uint SendBannerMessageComposer = 889; //Rootkit
        public const uint SecretKeyComposer = 3489; //Rootkit
        public const uint Ping = 2940; //Rootkit
        public const uint AuthenticationOK = 2008; //Rootkit
        public const uint HomeRoom = 854; //Rootkit
        public const uint FavouriteRooms = 2429; //Rootkit
        public const uint FavsUpdate = 2015; //Rootkit
        public const uint Fuserights = 325; //Rootkit
        //public const uint CurrentMinimails = 1286;
        public const uint ConfirmLeaveGroup = 2196; //Rootkit
        public const uint AvailabilityStatus = 2326; //Rootkit
        //public const uint InfoFeedEnable = 1343;
        public const uint ActivityPoints = 1271; //Rootkit
        public const uint CreditBalance = 3412; //Rootkit
        public const uint UniqueID = 1212; //Rootkit
        public const uint HabboInfomation = 178; //Rootkit
        public const uint ProfileInformation = 3330; //Rootkit
        public const uint Allowances = 709; //Rootkit
        public const uint AchievementPoints = 2657; //Rootkit 
        public const uint CitizenshipPanel = 3372; //Rootkit
        public const uint OpenShop = 3845; //Rootkit
        public const uint OpenShopPage = 3379; //Rootkit
        public const uint ShopData1 = 747; //Rootkit
        public const uint ShopData2 = 2642; //Rootkit
        public const uint Offer = 366; //Rootkit
        public const uint SendAllowances = 2680; //Rootkit
        public const uint PetRace = 714; //Rootkit
        public const uint CheckPetName = 204; //Rootkit
        public const uint UpdateShop = 2879; //Rootkit
        public const uint AchievementList = 1437; //Rootkit
        public const uint AchievementProgress = 1581; //Rootkit
        public const uint UnlockAchievement = 652; //Rootkit
        public const uint OpenModTools = 3563; //Rootkit
        public const uint SendNotif = 700;//193; //Rootkit
        public const uint ModResponse = 1991; //Rootkit
        public const uint BroadcastMessage = 193; //Rootkit
        public const uint GiftError = 2612; //Rootkit
        public const uint UpdateInventary = 1507; //Rootkit
        public const uint NavigatorPacket = 1019; //Rootkit
        public const uint PublicCategories = 1969; //Rootkit
        public const uint EffectsInventary = 631; //Rootkit
        public const uint AddEffectToInventary = 2347; //Rootkit
        public const uint EnableEffect = 2803; //Rootkit
        public const uint StopEffect = 1176; //Rootkit
        public const uint OpenHelpTool = 3466; //Rootkit
        public const uint CanCreateRoom = 3710; //Rootkit
        public const uint OnCreateRoomInfo = 2897; //Rootkit
        public const uint RoomSettingsData = 3367; //Rootkit
        public const uint UpdateRoomOne = 3713; //Rootkit
        public const uint RoomAds = 1475; //Rootkit
        public const uint PrepareRoomForUsers = 444; //Rootkit
        public const uint RoomErrorToEnter = 1819; //Rootkit
        public const uint OutOfRoom = 1256; //Rootkit
        public const uint DoorBellNoPerson = 436; //Rootkit
        public const uint Doorbell = 2063; //Rootkit
        //public const uint InvalidDoorBell = 2458;
        public const uint InitFriends = 1800; //Rootkit
        public const uint InitRequests = 3540; //Rootkit
        public const uint SendFriendRequest = 1964; //Rootkit
        public const uint FriendUpdate = 1710; //Rootkit
        public const uint InstantChat = 2747; //Rootkit
        public const uint InstantInvite = 2925; //Rootkit
        public const uint InstantChatError = 3945; //Rootkit
        public const uint InitialRoomInformation = 206; // Rootkit
        public const uint FollowBuddyError = 809; //Rootkit
        public const uint SearchFriend = 1788; //Rootkit
        public const uint TradeStart = 2185; //Rootkit
        public const uint TradeUpdate = 1386; //Rootkit
        public const uint TradeAcceptUpdate = 299; //Rootkit
        public const uint TradeComplete = 2557; //Rootkit
        public const uint TradeCloseClean = 387; //Rootkit
        public const uint TradeClose = 2024; //Rootkit
        public const uint RoomDecoration = 2993; //Rootkit
        public const uint RoomRightsLevel = 8; //Rootkit
        public const uint HasOwnerRights = 1812; //Rootkit
        public const uint RateRoom = 3710; //Rootkit
        public const uint ScoreMeter = 978; //Rootkit
        public const uint SendGroup = 2119; //Rootkit
        public const uint CanCreateEvent = 2099; //Rootkit
        public const uint RoomEvent = 743; //Rootkit
        public const uint HeightMap = 3942; //Rootkit
        public const uint FloorHeightMap = 2443; //Rootkit
        public const uint OpenGift = 1208; //Rootkit
        //public const uint Objects = 362;
        public const uint SerializeWallItems = 1348; //Rootkit
        public const uint SerializeFloorItems = 2087; //Rootkit
        public const uint FloodFilter = 3410; //Rootkit
        public const uint SetRoomUser = 610; //Rootkit
        public const uint ConfigureWallandFloor = 752; //Rootkit
        public const uint SerializeClub = 319; //Rootkit
        public const uint ClubComposer = 2677; //Rootkit
        public const uint PopularTags = 257; //Rootkit
        public const uint FlatCats = 224; //Rootkit
        public const uint SerializePublicRooms = 1969; //Rootkit
        public const uint Talk = 1510; //Rootkit
        public const uint Shout = 2408; //Rootkit
        public const uint Whisp = 2732; //Rootkit
        public const uint UpdateIgnoreStatus = 2288; //Rootkit
        public const uint GiveRespect = 123; //Rootkit
        public const uint PrepareCampaing = 769; //Rootkit
        public const uint SendCampaingData = 2680; //Rootkit
        public const uint UpdateUserInformation = 2859; //Rootkit
        public const uint Dance = 732; //Rootkit
        public const uint Action = 3984; //Rootkit
        public const uint ApplyCarryItem = 3395; //Rootkit
        public const uint IdleStatus = 3689; //Rootkit
        public const uint Inventory = 3572; //Rootkit
        //public const uint Inventory2 = 3306;
        public const uint PetInventory = 2304; //Rootkit
        public const uint PlaceBot = 610; //Rootkit
        public const uint PetInformation = 2135; //Rootkit
        public const uint RespectPet = 3395; //Rootkit
        public const uint AddExperience = 1559; //Rootkit
        public const uint BadgesInventory = 2356; //Rootkit
        public const uint UpdateBadges = 833; //Rootkit
        public const uint GetUserBadges = 833; //Rootkit
        public const uint GetUserTags = 1574; //Rootkit
        public const uint GivePowers = 3158; //Rootkit
        public const uint RemovePowers = 3702; //Rootkit
        public const uint QuitRights = 3185; //Rootkit
        public const uint FlatControllerAdded = 2532; //Rootkit
        public const uint TypingStatus = 3218; //Rootkit
        public const uint RemoveObjectFromInventory = 238; //Rootkit
        public const uint AddWallItemToRoom = 1382; //Rootkit
        public const uint UpdateWallItemOnRoom = 893; //Rootkit
        public const uint PickUpWallItem = 1317; //Rootkit
        public const uint UserLeftRoom = 3359; //Rootkit
        public const uint RoomTool = 2954; //Rootkit
        public const uint UserTool = 3282; //Rootkit
        public const uint RoomChatlog = 3619; //Rootkit
        public const uint UserChatlog = 2981; //Rootkit
        public const uint RoomVisits = 683; //Rootkit
        public const uint IssueChatlog = 951; //Rootkit
        public const uint SerializeIssue = 2740; //Rootkit
        public const uint UpdateIssue = 993; //Rootkit
        public const uint ApplyEffects = 3194; //Rootkit
        public const uint ObjectOnRoller = 106; //Rootkit
        public const uint DimmerData = 418; //Rootkit
        public const uint OpenPostIt = 3086; //Rootkit
        public const uint UpdateFreezeLives = 839; //Rootkit
        public const uint WardrobeData = 3255; //Rootkit
        public const uint HelpRequest = 2553; //Rootkit
        public const uint SerializeCompetitionWinners = 2945; //Rootkit
        //public const uint FavoriteRoomFailed = 33;
        public const uint RecyclePrizes = 1878; //Rootkit
        //public const uint PetRespectNotification = 3692;
        //public const uint PetAddedToInventory = 3947;
        //public const uint MOTDNotification = 3677;
        public const uint Recycle = 3668; //Rootkit
        // Wired
        public const uint WiredFurniTrigger = 639; //Rootkit
        public const uint WiredEffect = 1843; //Rootkit
        public const uint WiredCondition = 3987; //Rootkit
        public const uint SaveWired = 2992; //Rootkit

        // Floor item IDs
        public const uint ObjectAdd = 3076; //Rootkit
        public const uint ObjectRemove = 1891; //Rootkit
        public const uint ObjectUpdate = 3510; //Rootkit
        public const uint ObjectDataUpdate = 1194; //Rootkit

        // For multiple items
        //public const uint ObjectsDataUpdate = 1208;
        //2295 _-1qv _-09Y DiceValue
        //2357 _-4Ng _-1-M OneWayDoorStatus

        // Interaction
        //public const uint OneWayDoorStatus = 2357;

        public const uint UserUpdate = 1387; //Rootkit
        public const uint RoomData = 2456; //Rootkit
        // Room enter
        public const uint RoomEntryInfo = 1873; //Rootkit
        //public const uint NoSuchFlat = 1378;
        public const uint FurnitureAliases = 1765; // not used.
        public const uint GetGuestRoomResult = 542; // idk what the new header is.

        public const uint RoomForward = 1959; //Rootkit
        public const uint SearchFriendsMessage = 3665; // no use

        // Door bell
        public const uint FlatAccessible = 400; //Rootkit

        // Disconnection
        //public const uint DisconnectReason = 4000;

        // Groups/guilds
        public const uint PurchaseGuildInfo = 3341;
        public const uint GuildEditorData = 1725;
        public const uint HabboGroupJoinFailed = 2407;
        public const uint GroupInfo = 2602;
        public const uint OwnGuilds = 1463;
        public const uint GroupCreated = 1327;

        // User
        public const uint SoundSettings = 89;
        //public const uint WelcomeBack = 3015;

        // Pets
        public const uint PetCommands = 2775; //Rootkit

        // Messenger
        public const uint MessengerError = 260; //Rootkit

        // Engine
        public const uint GenericError = 1671; //Rootkit

        // Vouchers
        public const uint VoucherRedeemError = 2442; //Rootkit
        public const uint VoucherRedeemOk = 3089; //Rootkit

        // Inventory
        public const uint PetRemovedFromInventory = 604; //Rootkit

        // Catalog
        //public const uint NotEnoughBalance = 3269;
        //public const uint PurchaseError = 277;
        public const uint PurchaseOK = 2434; //Rootkit
        public const uint UnseenItems = 469; //Rootkit
        //public const uint ClubPage = 625;
        //public const uint OfferGiftable = 622;
        //public const uint CanMakeOffer = 2444;
        //public const uint MarketplaceItemStats = 1713;

        //Games
        public const uint GameCredits = 16;
        public const uint GameID = 2718;
        public const uint Game2WeeklyLeaderboard = 3355;
        public const uint Game2AccountGameStatus = 1010;
        public const uint InitGame = 3784;
        public const uint BestOfWeek = 163;
        public const uint PowerUps = 14;
        public const uint LoginFastFood = 11;
        public const uint UpdatePlayer = 4;
        public const uint PlateDown = 5;
        public const uint ActivateShield = 12;
        public const uint SerializeLobby = 3;

        //Handshake
        public const uint SessionParams = 257;
        public const uint IgnoringList = 1794; //Rootkit

        //Help
        //public const uint CallForHelp = 1693;

        public const uint ServerError = 649;
        public const uint BadgeError = 2427;
        public const uint ReceptionTimer = 3597;
        //public const uint RecycleItems = 2470;
        public const uint FigureData = 27;
        public const uint ChangeUsernameError = 3500;
        public const uint ChangeUsername1 = 469;
        public const uint InfobusMessage = 2319;
        public const uint TradeAllowed = 1460;
        public const uint Item1 = 460;
        public const uint Item2 = 1208; //Rootkit
        public const uint InfobusPoll = 33; //Rootkit
        public const uint RoomModel = 3299;//public rooms. We don't need them :)
        public const uint SongInfo = 1882;
        public const uint SoundMachinePlaylist = 1211;
        public const uint UpdateFavGuild = 1806;
        public const uint Guilds = 3834;
        public const uint Guild = 1621;
        public const uint UserGuilds = 915;
        public const uint LimitedSoldOut = 1290;
        //  public const uint CanMakeOffer = 279;
        public const uint Marketplace = 2272; //Rootkit
        public const uint Offerstats = 3734;
        public const uint Disconnect = 573; // IDK IF DISCONNECT OR NOT
        public const uint NotifyBan = 1947;
        public const uint WiredAtScore = 650;
        public const uint Wired2 = 651;
        public const uint QuestCount = 2243; //Rootkit
        public const uint QuestCancel = 1358; //Rootkit
        public const uint ActivateQuest = 1690; //Rootkit
        public const uint Quests = 3396;
        public const uint Quests2 = 518;
        public const uint UpdateUser = 481;
        public const uint UpdateRespect = 440; //
        public const uint KickMessage = 527;
        public const uint UpdateHeightmap = 1623;
        public const uint RoomCompetition = 3660;
        public const uint NewPoll = 252;
        public const uint InfobusPoll3 = 91; //Rootkit
        public const uint Logging = 370;
        public const uint CompleteQuests = 2947; //Rootkit
        public const uint LoadQuests = 2243; //Rootkit
        public const uint Playlist = 2799;
        public const uint Playing = 8;
        public const uint SongInventory = 901;
        public const uint RoomVisits2 = 2730;
        public const uint AddBadge = 832;
        public const uint BotInventory = 1294; //Rootkit
        public const uint Messenger1 = 261; //maybe old...
        public const uint ParkBusDoor = 2860;
        //public const uint Billboard = 199;
        //public const uint Unkown = 131;
        //public const uint Invisible = 1474; // unused
        public const uint InfobusPoll2 = 556; //Rootkit
        public const uint MusAnswer = 1337; //Rootkit
        public const uint EventStream = 1973; //Rootkit
        public const uint TalentTrackEarned = 6589; //Rootkit
        public const uint TalentTrack = 1795; //Rootkit
        public const uint CheckQuiz = 3623; //Rootkit
        public const uint StartQuiz = 3539; //Rootkit
        public const uint SetCommandsView = 367; //Rootkit
        //1554

        public const uint SendGuildParts = 1061; //Rootkit
        public const uint SendGestionGroup = 858; //Rootkit
        public const uint SendGuildElements = 29; //Rootkit
        public const uint SerializePurchaseInformation = 2434; //Rootkit
        public const uint SendHtmlColors = 2049; //Rootkit
        public const uint UpdateRoom = 1066; //Rootkit
        public const uint SendRoomAndGroup = 1644; //Rootkit
        public const uint SendPurchaseAlert = 469; //Rootkit
        public const uint RecycleState = 3997; //Rootkit
        public const uint SendAdvGroupInit = 3225; //Rootkit
        public const uint SendMembersAndPetitions = 326; //Rootkit
        public const uint AddNewMember = 2418; //Rootkit
        public const uint UpdatePetitionsGuild = 1647; //Rootkit
        public const uint RemoveGuildFavorite = 1662; //Rootkit
        public const uint HelpRequestAlert = 3027; //Rootkit
        public const uint ProfileRelationships = 3570; //Rootkit
        public const uint RoomDataEdit = 3367; //Rootkit
        public const uint GetPowerList = 2532; //Rootkit
        public const uint OpenGamesTab = 664; //Rootkit
        public const uint GetRankInGame = 1414; //Rootkit
        public const uint Game2GameStatusMessageEvent = 275; //Rootkit
        public const uint CreateWar = 3849; //Rootkit
        public const uint StartCounter = 430; //Rootkit
        public const uint SetStep1 = 2656; //Rootkit
        public const uint Game2EnterArenaMessageEvent = 1864; //Rootkit
        public const uint Game2ArenaEnteredMessageEvent = 133; //Rootkit
        public const uint Game2StageLoadMessageEvent = 2502; //Rootkit
        public const uint Game2StageStillLoadingMessageEvent = 1283; //Rootkit
        public const uint Game2StageStartingMessageEvent = 2900; //Rootkit
        public const uint Game2PlayerExitedGameArenaMessageEvent = 1956; //Rootkit
        public const uint Game2StageRunningMessageEvent = 1184; //Rootkit
        public const uint AddToNewGame = 1674; //Rootkit
        public const uint Game2GameChatFromPlayerMessageEvent = 1577; //Rootkit
        public const uint Game2FullGameStatusMessageEvent = 2962; //Rootkit
        public const uint SerializeSpeechList = 20; //Rootkit
        public const uint SerializeGuideTool = 1417; //Rootkit
        public const uint GuideSessionStarted = 2120; //Rootkit
        public const uint GuideSessionAttached = 2713; //Rootkit
        public const uint GuideSessionMessage = 323; //Rootkit
        public const uint GuideSessionInvitedToGuideRoom = 2506; //Rootkit
        public const uint ToggleGuideTicketTyping = 1917; //Rootkit
        public const uint FollowBuddy = 1959; //Rootkit
        public const uint LoadGame = 3036; //Rootkit
        public const uint InvalidUsername = 3456; //Rootkit for Mobile
        public const uint InvalidPassword = 3457; //Rootkit for Mobile
    }
}
