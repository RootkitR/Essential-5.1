using System;
using System.Collections.Generic;
using Essential.Communication.Messages.Avatar;
using Essential.Communication.Messages.Wired;
using Essential.Communication.Messages.Sound;
using Essential.Communication.Messages.Register;
using Essential.Communication.Messages.Inventory.Badges;
using Essential.Communication.Messages.Recycler;
using Essential.Communication.Messages.Users;
using Essential.Communication.Messages.Inventory.Trading;
using Essential.Communication.Messages.Help;
using Essential.Communication.Messages.Rooms.Action;
using Essential.Communication.Messages.Rooms.Furniture;
using Essential.Communication.Messages.Rooms.Avatar;
using Essential.Communication.Messages.Rooms.Chat;
using Essential.Communication.Messages.Rooms.Engine;
using Essential.Communication.Messages.Rooms.Pets;
using Essential.Communication.Messages.Rooms.Session;
using Essential.Communication.Messages.Rooms.Settings;
using Essential.Communication.Messages.Navigator;
using Essential.Communication.Messages.Handshake;
using Essential.Communication.Messages.Messenger;
using Essential.Communication.Messages.Catalog;
using Essential.Communication.Messages.Marketplace;
using Essential.Communication.Messages.Inventory.AvatarFX;
using Essential.Communication.Messages.Inventory.Furni;
using Essential.Communication.Messages.Inventory.Purse;
using Essential.Communication.Messages.Inventory.Achievements;
using Essential.Communication.Messages.Quest;
using Essential.Communication.Messages.Games;
using Essential.Communication.Messages.Rooms.Polls;
using Essential.Communication.Messages.SoundMachine;
using Essential.Communication.Messages.FriendStream;
using Essential.Communication.Headers;
using Essential.Communication.Messages;
using Essential.Communication.Messages.TalentTrack;
using Essential.Communication.Messages.Guilds;
using Essential.Communication.Messages.Relationships;
using Essential.Communication.Messages.Games.Snowstorm;
using Essential.Communication.Messages.Rooms.Bots;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Communication.Messages.Guide;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication
{
	internal sealed class PacketManager
	{
		private Dictionary<uint, Interface> RequestHandlers;
		public PacketManager()
		{
			this.RequestHandlers = new Dictionary<uint, Interface>();
            Incoming.Init();
		}
		public bool Handle(uint PacketId, out Interface Event)
		{
			if (this.RequestHandlers.ContainsKey(PacketId))
			{
                Event = this.RequestHandlers[PacketId];
                //Console.WriteLine("known id #" + PacketId);
				return true;
            }
			else
			{
                //Console.WriteLine("unknown id #" + PacketId);
				Event = null;
				return false;
			}
		}

        /*
         * Cleaned up by Leon
         */
        public int Count
        {
            get
            {
                return RequestHandlers.Count;
            }
        }
        public void Load()
        {
            #region "Handshake"
            this.RequestHandlers.Add(Incoming.SecretKey, new GenerateSecretKeyMessageEvent()); 
            //this.RequestHandlers.Add(Incoming, new GetSessionParametersMessageEvent());
            this.RequestHandlers.Add(Incoming.UserInformation, new InfoRetrieveMessageEvent()); 
            this.RequestHandlers.Add(Incoming.InitCrypto, new InitCryptoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Pong, new PongMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SSOTicket, new SSOTicketMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.SSOTicket, new TryLoginMessageEvent());
            this.RequestHandlers.Add(Incoming.UniqueMachineID, new UniqueIDMessageEvent());// no reference..
            this.RequestHandlers.Add(Incoming.CheckReleaseMessageEvent, new VersionCheckMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ClientVars, new ClientVariableMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoginWithCredentials, new LoginWithUsernameAndPasswordEvent()); //NEW FOR PHONE SUPPORT
            #endregion
            #region "Messenger"
            this.RequestHandlers.Add(Incoming.AcceptRequest, new AcceptBuddyMessageEvent()); 
            this.RequestHandlers.Add(Incoming.DeclineRequest, new DeclineBuddyMessageEvent()); 
            this.RequestHandlers.Add(Incoming.FollowFriend, new FollowFriendMessageEvent()); 
            this.RequestHandlers.Add(Incoming.UpdateFriendsState, new FriendsListUpdateEvent()); 
            //this.RequestHandlers.Add(233, new GetBuddyRequestsMessageEvent()); no reference....
            this.RequestHandlers.Add(Incoming.SearchFriend, new HabboSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.DeleteFriend, new RemoveBuddyMessageEvent()); 
            this.RequestHandlers.Add(Incoming.FriendRequest, new RequestBuddyMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendInstantMessenger, new SendMsgMessageEvent()); 
            this.RequestHandlers.Add(Incoming.InviteFriendsToMyRoom, new SendRoomInviteMessageEvent()); 
            #endregion
            #region "Navigator"
            this.RequestHandlers.Add(Incoming.AddFavourite, new AddFavouriteRoomMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.CancelEvent, new CancelEventMessageEvent());
            //this.RequestHandlers.Add(Incoming.CanCreateEvent, new CanCreateEventMessageEvent());
            this.RequestHandlers.Add(Incoming.CanCreateRoom, new CanCreateRoomMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.CreateRoomEvent, new CreateEventMessageEvent());
            this.RequestHandlers.Add(Incoming.CreateRoom, new CreateFlatMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveFavourite, new DeleteFavouriteRoomMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.EditEvent, new EditEventMessageEvent());
            //this.RequestHandlers.Add(Incoming, new GetGuestRoomMessageEvent());
            this.RequestHandlers.Add(Incoming.LoadFeaturedRooms, new GetOfficialRoomsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadPopularTags, new GetPopularRoomTagsMessageEvent()); 
            //this.RequestHandlers.Add(388, new GetPublicSpaceCastLibsMessageEvent()); no reference...
            this.RequestHandlers.Add(Incoming.LoadCategorys, new GetUserFlatCatsMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.LatestEvents, new LatestEventsSearchMessageEvent());
            this.RequestHandlers.Add(Incoming.MyFavs, new MyFavouriteRoomsSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RoomsOfMyFriends, new MyFriendsRoomsSearchMessageEvent());  
            this.RequestHandlers.Add(Incoming.RecentRooms, new MyRoomHistorySearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadMyRooms, new MyRoomsSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadAllRooms, new PopularRoomsSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GiveRoomScore, new RateFlatMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RoomsWhereMyFriends, new RoomsWhereMyFriendsAreSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.HighRatedRooms, new RoomsWithHighestScoreSearchMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.TagSearch, new RoomTagSearchMessageEvent());
            this.RequestHandlers.Add(Incoming.SearchRoomByName, new RoomTextSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ToggleStaffPick, new ToggleStaffPickMessageEvent());
            this.RequestHandlers.Add(Incoming.SetHome, new UpdateNavigatorSettingsMessageEvent()); 
            //this.RequestHandlers.Add(386, new UpdateRoomThumbnailMessageEvent()); // Retired
            this.RequestHandlers.Add(Incoming.PopularGuilds, new GetPopularGroups()); 
            this.RequestHandlers.Add(Incoming.EventTracker, new EventLog());
            #endregion
            #region "RoomsAction"
            //this.RequestHandlers.Add(440, new CallGuideBotMessageEvent()); // Retired
            this.RequestHandlers.Add(Incoming.GiveRights, new AssignRightsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveRightsFrom, new RemoveRightsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveAllRights, new RemoveAllRightsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.KickUserOfRoom, new KickUserMessageEvent()); 
            // this.RequestHandlers.Add(Incoming.BotDisposer, new KickBotMessageEvent()); 
            this.RequestHandlers.Add(Incoming.BanUserOfRoom, new BanUserMessageEvent()); 
            this.RequestHandlers.Add(Incoming.AnswerDoorBell, new LetUserInMessageEvent()); 
            #endregion
            #region "RoomsAvatar"
            this.RequestHandlers.Add(Incoming.ApplySign, new SignMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ApplyAction, new WaveMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ApplyDance, new DanceMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LookTo, new LookToMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveHanditem, new DropItemMessageEvent()); 
            /* this.RequestHandlers.Add(Incoming., new ChangeUserNameMessageEvent());
             this.RequestHandlers.Add(Incoming.ConfrimName, new ChangeUserNameMessageEvent());*/
            this.RequestHandlers.Add(Incoming.Sit, new SitMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GiveObject, new GiveItemToUserMessageEvent()); // Rootkit
            #endregion
            #region "RoomsChat"
            this.RequestHandlers.Add(Incoming.Talk, new ChatMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Shout, new ShoutMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Whisp, new WhisperMessageEvent()); 
            this.RequestHandlers.Add(Incoming.StartTyping, new StartTypingMessageEvent()); 
            this.RequestHandlers.Add(Incoming.StopTyping, new CancelTypingMessageEvent()); 
            #endregion
            #region "RoomsEngine"
            //this.RequestHandlers.Add(Incoming.ClothingChangeData, new SetClothingChangeDataMessageEvent());
            //this.RequestHandlers.Add(Incoming.GetFurnitureAliases, new GetFurnitureAliasesMessageEvent());
            this.RequestHandlers.Add(Incoming.AddUserToRoom, new GetRoomEntryDataMessageEvent()); 
            this.RequestHandlers.Add(Incoming.AddUserToRoom2, new GetRoomEntryDataMessageEvent()); 
            this.RequestHandlers.Add(3443, new LoadHomeRoomlel());
            this.RequestHandlers.Add(Incoming.LoadUser2, new LoadUserRoomMessageEvent()); //NEW
            this.RequestHandlers.Add(Incoming.PickupItem, new PickupObjectMessageEvent()); 
            //   this.RequestHandlers.Add(2242, new GetRoomEntryDataMessageEvent());
            this.RequestHandlers.Add(Incoming.MoveOrRotate, new MoveObjectMessageEvent()); 
            this.RequestHandlers.Add(Incoming.MoveWall, new MoveWallItemMessageEvent()); 
            this.RequestHandlers.Add(Incoming.AddFloorItem, new PlaceObjectMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenPostIt, new GetItemDataMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SavePostIt, new SetItemDataMessageEvent()); 
            this.RequestHandlers.Add(Incoming.DeletePostIt, new RemoveItemMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Move, new MoveAvatarMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SaveBranding, new SetObjectDataMessageEvent()); // WORKS!
            this.RequestHandlers.Add(Incoming.ApplySpace, new ApplyRoomEffect()); 
            //this.RequestHandlers.Add(Incoming.Intersitial, new GetInterstitialMessageEvent());
            #endregion
            #region "RoomsFurniture"
            //this.RequestHandlers.Add(Incoming., new UseFurnitureMessageEvent()); // Ilotulitus?
            this.RequestHandlers.Add(Incoming.HandleWallItem, new UseFurnitureMessageEvent()); 
            // this.RequestHandlers.Add(Incoming.HandleWallItem, new UseFurnitureMessageEvent()); // Handle wall item
            this.RequestHandlers.Add(Incoming.HandleItem, new UseFurnitureMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenDice, new UseFurnitureMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OneWayGate, new UseFurnitureMessageEvent()); 
            //this.RequestHandlers.Add(76, new UseFurnitureMessageEvent());
            this.RequestHandlers.Add(Incoming.RedeemExchangeFurni, new CreditFurniRedeemMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenGift, new PresentOpenMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RunDice, new DiceOffMessageEvent());  
            this.RequestHandlers.Add(Incoming.StartMoodlight, new RoomDimmerGetPresetsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ApplyMoodlightChanges, new RoomDimmerSavePresetMessageEvent()); 
            this.RequestHandlers.Add(Incoming.TurnOnMoodlight, new RoomDimmerChangeStateMessageEvent()); 
            this.RequestHandlers.Add(Incoming.AddPostIt, new PlacePostItMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Mannequinshit, new SaveMannequinMessageEvent());
            #endregion
            #region "RoomsPets"
            this.RequestHandlers.Add(Incoming.PetInfo, new GetPetInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.PlacePet, new PlacePetMessageEvent()); 
            this.RequestHandlers.Add(Incoming.PickupPet, new RemovePetFromFlatMessageEvent()); 
            this.RequestHandlers.Add(Incoming.TrainPet, new GetPetCommandsMessageEvent());
            this.RequestHandlers.Add(Incoming.RespetPet, new RespectPetMessageEvent()); 
            #endregion
            #region "RoomsBots"
            this.RequestHandlers.Add(Incoming.BotDisposer, new PickupBotMessageEvent()); 
            this.RequestHandlers.Add(Incoming.BotComposer, new PlaceBotMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SerializeBotInformation, new EditBotInformations()); 
            this.RequestHandlers.Add(Incoming.BotSpeechList, new SerializeBotSpeeches()); 
            #endregion
            #region "RoomsPolls"
            /*this.RequestHandlers.Add(Incoming.ShowRoomPoll, new ShowRoomPoll());
            this.RequestHandlers.Add(Incoming.PollAnswer, new GetRoomPollAnswers());*/
            this.RequestHandlers.Add(Incoming.AnswerInfoBusPoll, new AnswerInfobusPoll()); 
            #endregion
            #region "RoomsSession"
            this.RequestHandlers.Add(Incoming.GoToHotelView, new QuitMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadFirstRoomData, new OpenFlatConnectionMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadHeightMap, new GetHeightmapMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ReqLoadByDoorBell, new ReqLoadByDoorBellMessageEvent());
            #endregion
            #region "RoomsSettings"
            this.RequestHandlers.Add(Incoming.GetRoomData, new GetRoomSettingsMessageEvent());
            this.RequestHandlers.Add(Incoming.SaveRoomData, new SaveRoomSettingsMessageEvent());
            this.RequestHandlers.Add(Incoming.RemoveRoom, new DeleteRoomMessageEvent());
            this.RequestHandlers.Add(Incoming.GetFlatControls, new GetFlatControlsMessageEvent());
            #endregion
            #region "Catalog"
            this.RequestHandlers.Add(Incoming.OpenCatalog, new GetCatalogIndexEvent()); 
            this.RequestHandlers.Add(Incoming.OpenCatalogPage, new GetCatalogPageEvent()); 
            //this.RequestHandlers.Add(3031, new GetClubOffersMessageEvent()); useless
            this.RequestHandlers.Add(Incoming.CatalogData2, new GetGiftWrappingConfigurationEvent()); //UDATED
            //this.RequestHandlers.Add(3038, new GetHabboBasicMembershipExtendOfferEvent());
            //this.RequestHandlers.Add(3035, new GetHabboClubExtendOfferMessageEvent());
            //this.RequestHandlers.Add(3030, new GetIsOfferGiftableEvent());
            this.RequestHandlers.Add(Incoming.CatalogGetRace, new GetSellablePetBreedsEvent()); // Update
            //this.RequestHandlers.Add(3034, new MarkCatalogNewAdditionsPageOpenedEvent());
            //this.RequestHandlers.Add(Incoming.OpenGuildPage, new GetCatalogGroupWidgetData()); // GroupBadges
            //this.RequestHandlers.Add(3037, new PurchaseBasicMembershipExtensionEvent());
            this.RequestHandlers.Add(Incoming.PurchaseGift, new PurchaseFromCatalogAsGiftEvent()); 
            this.RequestHandlers.Add(Incoming.PurchaseCatalogItem, new PurchaseFromCatalogEvent()); 
            //this.RequestHandlers.Add(3036, new PurchaseVipMembershipExtensionEvent());
            this.RequestHandlers.Add(Incoming.RedeemVoucher, new RedeemVoucherMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenGuildPage, new OpenGuildPageMessageEvent()); 
            this.RequestHandlers.Add(Incoming.BuyGroup, new BuyGuildMessageEvent()); 
            //this.RequestHandlers.Add(475, new SelectClubGiftEvent());
            #endregion
            #region "Marketplace"
            //this.RequestHandlers.Add(3013, new BuyMarketplaceTokensMessageEvent()); // Retired
            this.RequestHandlers.Add(Incoming.MarketplacePurchase, new BuyOfferMessageEvent()); 
            //this.RequestHandlers.Add(2805, new CancelOfferMessageEvent());
            //this.RequestHandlers.Add(2699, new GetMarketplaceCanMakeOfferEvent());
            //this.RequestHandlers.Add(2054, new GetMarketplaceItemStatsEvent());
            this.RequestHandlers.Add(Incoming.MarketplaceGetOffers, new GetOffersMessageEvent()); 
            //this.RequestHandlers.Add(521, new GetOwnOffersMessageEvent());
            //this.RequestHandlers.Add(2465, new MakeOfferMessageEvent());
            //this.RequestHandlers.Add(1753, new RedeemOfferCreditsMessageEvent());
            //this.RequestHandlers.Add(1937, new GetMarketplaceConfigurationMessageEvent());
            //Most of them useless because of my Search function.. :D
            #endregion
            #region "Recycler"
            this.RequestHandlers.Add(Incoming.CatalogData1, new GetRecyclerPrizesMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RecycleAgain, new GetRecyclerStatusMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RecycleItem, new RecycleItemsMessageEvent()); 
            #endregion
            #region "Quest"
            
            this.RequestHandlers.Add(Incoming.ActiveQuests, new AcceptQuestMessageEvent());
            this.RequestHandlers.Add(Incoming.OpenQuests, new GetQuestsMessageEvent());
            this.RequestHandlers.Add(Incoming.ActiveEndedQuest, new OpenQuestTrackerMessageEvent());
            this.RequestHandlers.Add(Incoming.CancelQuests, new RejectQuestMessageEvent());
            #endregion
            #region "InventoryFurni"
            this.RequestHandlers.Add(Incoming.PetInventary, new GetPetInventoryEvent()); 
            this.RequestHandlers.Add(Incoming.BotsInventary, new GetBotInventoryEvent()); 
            this.RequestHandlers.Add(Incoming.OpenInventory, new RequestFurniInventoryEvent()); 
            #endregion
            #region"InventoryBadges"
            this.RequestHandlers.Add(Incoming.BadgesInventary, new GetBadgesEvent()); 
            this.RequestHandlers.Add(Incoming.ApplyBadge, new SetActivatedBadgesEvent()); 
            #endregion
            #region "InventoryTrading"
            this.RequestHandlers.Add(Incoming.UnacceptTrade, new UnacceptTradingEvent()); 
            this.RequestHandlers.Add(Incoming.AcceptTrade, new AcceptTradingEvent()); 
            this.RequestHandlers.Add(Incoming.StartTrade, new OpenTradingEvent()); 
            this.RequestHandlers.Add(Incoming.SendOffer, new AddItemToTradeEvent()); 
            this.RequestHandlers.Add(Incoming.ConfirmTrade, new ConfirmAcceptTradingEvent()); 
            //this.RequestHandlers.Add(Incoming.Trade, new ConfirmDeclineTradingEvent());
            this.RequestHandlers.Add(Incoming.CancelTrade, new ConfirmDeclineTradingEvent()); 
            this.RequestHandlers.Add(Incoming.CancelOffer, new RemoveItemFromTradeEvent()); 
            #endregion
            #region "InventoryAvatarEffects"
            this.RequestHandlers.Add(Incoming.StartEffect, new AvatarEffectSelectedEvent()); 
            this.RequestHandlers.Add(Incoming.EnableEffect, new AvatarEffectActivatedEvent()); 
            #endregion
            #region "InventoryAchievements"
            this.RequestHandlers.Add(Incoming.OpenAchievements, new GetAchievementsEvent()); 
            #endregion
            #region "Avatar"
            this.RequestHandlers.Add(Incoming.ChangeMotto, new ChangeMottoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetWardrobe, new GetWardrobeMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SaveWardrobe, new SaveWardrobeOutfitMessageEvent()); 
            #endregion
            #region "Register"
            this.RequestHandlers.Add(Incoming.ChangeLook, new UpdateFigureDataMessageEvent()); 
            #endregion
            #region "Users"
            this.RequestHandlers.Add(Incoming.SerializeClub, new ScrGetUserInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.CheckPetName, new ApproveNameMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetUserTags, new GetUserTagsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetUserBadges, new GetSelectedBadgesMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.GetGuildFavorite, new GetHabboGroupBadgesMessageEvent());
            //this.RequestHandlers.Add(Incoming, new GetHabboGroupDetailsMessageEvent());
            //this.RequestHandlers.Add(Incoming., new LoadUserGroupsEvent());
            this.RequestHandlers.Add(Incoming.IgnoreUser, new IgnoreUserMessageEvent()); 
            this.RequestHandlers.Add(Incoming.UnignoreUser, new UnignoreUserMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendRespects, new RespectUserMessageEvent()); 
            /*this.RequestHandlers.Add(Incoming.ExitGuild, new ExitGuildEvent()); // NEW
			this.RequestHandlers.Add(Incoming.SendRequestGuild, new JoinGuildEvent());
            //this.RequestHandlers.Add(Incoming.Guil, new GetGuildFavorite());
            this.RequestHandlers.Add(Incoming.RemoveGuildFavorite, new RemoveGuildFavorite());*/
            this.RequestHandlers.Add(Incoming.LoadProfile, new GetUserProfileEvent()); 
            //this.RequestHandlers.Add(Incoming.Gu, new GetGuildMembers()); // New
            #endregion
            #region "TalentTrack"
            this.RequestHandlers.Add(Incoming.LoadTalents, new GetTalentTrack());
            this.RequestHandlers.Add(Incoming.StartQuiz, new StartHabboWay());
            this.RequestHandlers.Add(Incoming.EndQuiz, new PostQuizAnswers());
            #endregion
            #region "Help"
            this.RequestHandlers.Add(Incoming.PanicButton, new CallForHelpMessageEvent());
            this.RequestHandlers.Add(Incoming.CreateTicket, new CallForHelpMessageEvent()); 
            this.RequestHandlers.Add(Incoming.CloseIssue, new CloseIssuesMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.Help, new DeletePendingCallsForHelpMessageEvent());
            this.RequestHandlers.Add(Incoming.IssueChatlog, new GetCfhChatlogMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ToolForThisRoom, new GetModeratorRoomInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ToolForUser, new GetModeratorUserInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenRoomChatlog, new GetRoomChatlogMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetRoomVisits, new GetRoomVisitsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.UserChatlog, new GetUserChatlogMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendMessageByTemplate, new ModAlertMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ModActionBanUser, new ModBanMessageEvent()); 
            this.RequestHandlers.Add(Incoming.PerformRoomAction, new ModerateRoomMessageEvent());
            this.RequestHandlers.Add(Incoming.SubmitHelpTicket, new CallForHelpMessageEvent());
            this.RequestHandlers.Add(Incoming.SendRoomAlert, new ModeratorActionMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ModActionKickUser, new ModKickMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ModActionMuteUser, new ModMuteUserMessageEvent());
            this.RequestHandlers.Add(Incoming.SendUserMessage, new ModMessageMessageEvent()); 
            this.RequestHandlers.Add(Incoming.PickIssue, new PickIssuesMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ReleaseIssue, new ReleaseIssuesMessageEvent()); 
            //this.RequestHandlers.Add(419, new SearchFaqsMessageEvent());
            this.RequestHandlers.Add(Incoming.OpenHelpTool, new GetHelpToolMessageEvent()); 
            // this.RequestHandlers.Add(1683, new IDONTKNOWWHATTHISISFOR());
            #endregion
            #region "Sound"
            this.RequestHandlers.Add(Incoming.volumeControl, new AdjustVolumeControlEvent());
            #endregion
            #region "Jukebox"
            this.RequestHandlers.Add(Incoming.AddNewCdToJuke, new AddNewJukeboxCD());
            this.RequestHandlers.Add(Incoming.RemoveCdToJuke, new RemoveCDToJukebox());
            #endregion
            #region "Wired"
            this.RequestHandlers.Add(Incoming.SaveWiredEffect, new UpdateTriggerMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SaveWiredTrigger, new UpdateActionMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SaveConditional, new UpdateConditionMessageEvent()); 
            //this.RequestHandlers.Add(Incoming, new ApplyFurniToSetConditions());
            #endregion
            #region "FriendStream"
            this.RequestHandlers.Add(Incoming.InitStream, new GetEventStreamComposer()); 
            //     this.RequestHandlers.Add(501, new SetEventStreamingAllowedComposer());
            this.RequestHandlers.Add(Incoming.StreamLike, new EventStreamingLikeButton()); 
            this.RequestHandlers.Add(Incoming.CreateStream, new UpdateStream()); 
            this.RequestHandlers.Add(Incoming.SearchStream, new GetUserStream()); 
            #endregion
            #region "Games"
           // this.RequestHandlers.Add(Incoming.GetGames, new GetGames());
           // this.RequestHandlers.Add(Incoming.GetGame, new GetGame());
           // this.RequestHandlers.Add(2372, new StartGameMessageEvent());
            //Snowwar :)
            this.RequestHandlers.Add(Incoming.GetGames, new GetGames());
            this.RequestHandlers.Add(Incoming.GetGame, new GetGame());
            /*this.RequestHandlers.Add(2372, new StartPanel());
            //this.RequestHandlers.Add(607, new RequestFullGameStatus());
            this.RequestHandlers.Add(Incoming.Game2LeaveGameMessageComposer, new LeaveGame());
            this.RequestHandlers.Add(Incoming.Game2GameChatMessageComposer, new TalkGame());
            //this.RequestHandlers.Add(Incoming.Game2LoadStageReadyMessageComposer, new StageReadyMessage());
            this.RequestHandlers.Add(Incoming.Game2SetUserMoveTargetMessageComposer, new WalkGame());
            this.RequestHandlers.Add(76, new StartGameMessageEvent());*/
            //this.RequestHandlers.Add(1, new LoginToFastFood());
            #endregion
            #region "Relationships"
            this.RequestHandlers.Add(Incoming.SetRelationshipStatus, new SetRelationshipsStatusMessage()); 
            this.RequestHandlers.Add(Incoming.GetRelationshipsProfile, new GetRelationshipsProfile()); 
            #endregion
            #region "Guilds"
            this.RequestHandlers.Add(Incoming.EditGuild, new EditGuildMessageEvent()); 
            this.RequestHandlers.Add(Incoming.EditIdentidad, new EditGroupNameMessageEvent()); 
            this.RequestHandlers.Add(Incoming.EditPlaca, new EditGroupHomeRoomMessageEvent()); 
            this.RequestHandlers.Add(Incoming.EditColores, new EditGroupColorsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.EditAjustes, new EditPrivilegesMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ExitGuild, new ConfirmExitGuildMessageEvent());
            this.RequestHandlers.Add(Incoming.ExitGuildConfirmed, new ExitGuildMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendRequestGuild, new SendGuildRequestEvent()); 
            this.RequestHandlers.Add(Incoming.LoadMembersPetitions, new LoadGuildRequestsEvent()); 
            this.RequestHandlers.Add(Incoming.RejectGuildMember, new DeclineGuildRequestEvent()); 
            this.RequestHandlers.Add(Incoming.AcceptMember, new AcceptGuildRequestEvent()); 
            this.RequestHandlers.Add(Incoming.UpdateUserToRankGuild, new UpdateUserToRankGuild()); 
            this.RequestHandlers.Add(Incoming.UpdateUserFromRankGuild, new UpdateUserToRankGuild()); 
            this.RequestHandlers.Add(Incoming.SendFurniGuild, new SendFurniGuildMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetGuildFavorite, new GetGuildFavoriteMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveGuildFavorite, new RemoveFavoriteGuildEvent()); 
            this.RequestHandlers.Add(Incoming.EndConfirmBuy, new GuildInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.TakeRights, new UpdateUserToRankGuild()); 
            #endregion
            #region "GuideTool"
            this.RequestHandlers.Add(Incoming.OpenGuideTool, new OpenGuideToolMessageEvent()); //3005 by Rootkit :)
            this.RequestHandlers.Add(Incoming.CallGuide, new CallGuideMessageEvent()); //2756 by Rootkit :)
            this.RequestHandlers.Add(Incoming.AcceptOrDeclineGuideRequest, new AcceptOrDeclineGuideRequestEvent()); //3971 by Rootkit :)
            this.RequestHandlers.Add(Incoming.GuideChat, new GuideChatMessageEvent()); //215 by Rootkit :)
            this.RequestHandlers.Add(Incoming.FollowUser, new FollowUserMessageEvent()); //2973 by Rootkit :)
            this.RequestHandlers.Add(Incoming.InviteUser, new InviteUserMessageEvent()); //1088 by Rootkit :)
            this.RequestHandlers.Add(Incoming.CloseGuideTicket, new CloseGuideTicketMessageEvent()); //3504 by Rootkit :)
            this.RequestHandlers.Add(Incoming.ToggleTypingState, new GuideUserStartsTypingMessageEvent()); //1261 by Rootkit :)
            this.RequestHandlers.Add(Incoming.ReportUser, new ReportUserMessageEvent());
            #endregion
        }
        #region "hi"
        public void Handshake()
		{
            this.RequestHandlers.Add(Incoming.SecretKey, new GenerateSecretKeyMessageEvent()); 
			//this.RequestHandlers.Add(Incoming, new GetSessionParametersMessageEvent());
            this.RequestHandlers.Add(Incoming.UserInformation, new InfoRetrieveMessageEvent()); 
            this.RequestHandlers.Add(Incoming.InitCrypto, new InitCryptoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Pong, new PongMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SSOTicket, new SSOTicketMessageEvent()); 
			//this.RequestHandlers.Add(Incoming.SSOTicket, new TryLoginMessageEvent());
			//this.RequestHandlers.Add(813, new UniqueIDMessageEvent()); no reference..
            this.RequestHandlers.Add(Incoming.CheckReleaseMessageEvent, new VersionCheckMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ClientVars, new ClientVariableMessageEvent()); 
		}
		public void Messenger()
		{
            this.RequestHandlers.Add(Incoming.AcceptRequest, new AcceptBuddyMessageEvent()); 
            this.RequestHandlers.Add(Incoming.DeclineRequest, new DeclineBuddyMessageEvent()); 
            this.RequestHandlers.Add(Incoming.FollowFriend, new FollowFriendMessageEvent()); 
            this.RequestHandlers.Add(Incoming.UpdateFriendsState, new FriendsListUpdateEvent()); 
			//this.RequestHandlers.Add(233, new GetBuddyRequestsMessageEvent()); no reference....
            this.RequestHandlers.Add(Incoming.SearchFriend, new HabboSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.DeleteFriend, new RemoveBuddyMessageEvent()); 
            this.RequestHandlers.Add(Incoming.FriendRequest, new RequestBuddyMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendInstantMessenger, new SendMsgMessageEvent()); 
            this.RequestHandlers.Add(Incoming.InviteFriendsToMyRoom, new SendRoomInviteMessageEvent()); 
		}
		public void Navigator()
		{
            this.RequestHandlers.Add(Incoming.AddFavourite, new AddFavouriteRoomMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.CancelEvent, new CancelEventMessageEvent());
            //this.RequestHandlers.Add(Incoming.CanCreateEvent, new CanCreateEventMessageEvent());
            this.RequestHandlers.Add(Incoming.CanCreateRoom, new CanCreateRoomMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.CreateRoomEvent, new CreateEventMessageEvent());
            this.RequestHandlers.Add(Incoming.CreateRoom, new CreateFlatMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveFavourite, new DeleteFavouriteRoomMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.EditEvent, new EditEventMessageEvent());
            //this.RequestHandlers.Add(Incoming, new GetGuestRoomMessageEvent());
            this.RequestHandlers.Add(Incoming.LoadFeaturedRooms, new GetOfficialRoomsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadPopularTags, new GetPopularRoomTagsMessageEvent()); 
			//this.RequestHandlers.Add(388, new GetPublicSpaceCastLibsMessageEvent()); no reference...
            this.RequestHandlers.Add(Incoming.LoadCategorys, new GetUserFlatCatsMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.LatestEvents, new LatestEventsSearchMessageEvent());
            this.RequestHandlers.Add(Incoming.MyFavs, new MyFavouriteRoomsSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RoomsOfMyFriends, new MyFriendsRoomsSearchMessageEvent());  
            this.RequestHandlers.Add(Incoming.RecentRooms, new MyRoomHistorySearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadMyRooms, new MyRoomsSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadAllRooms, new PopularRoomsSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GiveRoomScore, new RateFlatMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RoomsWhereMyFriends, new RoomsWhereMyFriendsAreSearchMessageEvent()); 
            this.RequestHandlers.Add(Incoming.HighRatedRooms, new RoomsWithHighestScoreSearchMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.TagSearch, new RoomTagSearchMessageEvent());
            this.RequestHandlers.Add(Incoming.SearchRoomByName, new RoomTextSearchMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.ToggleStaffPick, new ToggleStaffPickMessageEvent());
            this.RequestHandlers.Add(Incoming.SetHome, new UpdateNavigatorSettingsMessageEvent()); 
			//this.RequestHandlers.Add(386, new UpdateRoomThumbnailMessageEvent()); // Retired
            this.RequestHandlers.Add(Incoming.PopularGuilds, new GetPopularGroups()); 
            //this.RequestHandlers.Add(Incoming.EventLog, new EventLog());
		}
		public void RoomsAction()
		{
			//this.RequestHandlers.Add(440, new CallGuideBotMessageEvent()); // Retired
            this.RequestHandlers.Add(Incoming.GiveRights, new AssignRightsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveRightsFrom, new RemoveRightsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveAllRights, new RemoveAllRightsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.KickUserOfRoom, new KickUserMessageEvent()); 
           // this.RequestHandlers.Add(Incoming.BotDisposer, new KickBotMessageEvent()); 
            this.RequestHandlers.Add(Incoming.BanUserOfRoom, new BanUserMessageEvent()); 
            this.RequestHandlers.Add(Incoming.AnswerDoorBell, new LetUserInMessageEvent()); 
		}
		public void RoomsAvatar()
        {
            this.RequestHandlers.Add(Incoming.ApplySign, new SignMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ApplyAction, new WaveMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ApplyDance, new DanceMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LookTo, new LookToMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveHanditem, new DropItemMessageEvent()); 
           /* this.RequestHandlers.Add(Incoming., new ChangeUserNameMessageEvent());
            this.RequestHandlers.Add(Incoming.ConfrimName, new ChangeUserNameMessageEvent());*/
            this.RequestHandlers.Add(Incoming.Sit, new SitMessageEvent()); 
		}
		public void RoomsChat()
		{
            this.RequestHandlers.Add(Incoming.Talk, new ChatMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Shout, new ShoutMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Whisp, new WhisperMessageEvent()); 
            this.RequestHandlers.Add(Incoming.StartTyping, new StartTypingMessageEvent()); 
            this.RequestHandlers.Add(Incoming.StopTyping, new CancelTypingMessageEvent()); 
		}
		public void RoomsEngine()
		{
			//this.RequestHandlers.Add(Incoming.ClothingChangeData, new SetClothingChangeDataMessageEvent());
            //this.RequestHandlers.Add(Incoming.GetFurnitureAliases, new GetFurnitureAliasesMessageEvent());
            this.RequestHandlers.Add(Incoming.AddUserToRoom, new GetRoomEntryDataMessageEvent()); 
            this.RequestHandlers.Add(Incoming.AddUserToRoom2, new GetRoomEntryDataMessageEvent()); 
            //this.RequestHandlers.Add(3443, new LoadHomeRoomlel());
            this.RequestHandlers.Add(Incoming.LoadUser2, new LoadUserRoomMessageEvent()); //NEW
            this.RequestHandlers.Add(Incoming.PickupItem, new PickupObjectMessageEvent()); 
         //   this.RequestHandlers.Add(2242, new GetRoomEntryDataMessageEvent());
            this.RequestHandlers.Add(Incoming.MoveOrRotate, new MoveObjectMessageEvent()); 
            this.RequestHandlers.Add(Incoming.MoveWall, new MoveWallItemMessageEvent()); 
            this.RequestHandlers.Add(Incoming.AddFloorItem, new PlaceObjectMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenPostIt, new GetItemDataMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SavePostIt, new SetItemDataMessageEvent()); 
            this.RequestHandlers.Add(Incoming.DeletePostIt, new RemoveItemMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Move, new MoveAvatarMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SaveBranding, new SetObjectDataMessageEvent()); // WORKS!
            this.RequestHandlers.Add(Incoming.ApplySpace, new ApplyRoomEffect()); 
            //this.RequestHandlers.Add(Incoming.Intersitial, new GetInterstitialMessageEvent());
            
		}
		public void RoomsFurniture()
		{
            //this.RequestHandlers.Add(Incoming., new UseFurnitureMessageEvent()); // Ilotulitus?
            this.RequestHandlers.Add(Incoming.HandleWallItem, new UseFurnitureMessageEvent()); 
           // this.RequestHandlers.Add(Incoming.HandleWallItem, new UseFurnitureMessageEvent()); // Handle wall item
            this.RequestHandlers.Add(Incoming.HandleItem, new UseFurnitureMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenDice, new UseFurnitureMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OneWayGate, new UseFurnitureMessageEvent()); 
			//this.RequestHandlers.Add(76, new UseFurnitureMessageEvent());
            this.RequestHandlers.Add(Incoming.RedeemExchangeFurni, new CreditFurniRedeemMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenGift, new PresentOpenMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RunDice, new DiceOffMessageEvent()); 
            this.RequestHandlers.Add(Incoming.StartMoodlight, new RoomDimmerGetPresetsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ApplyMoodlightChanges, new RoomDimmerSavePresetMessageEvent()); 
            this.RequestHandlers.Add(Incoming.TurnOnMoodlight, new RoomDimmerChangeStateMessageEvent()); 
            this.RequestHandlers.Add(Incoming.AddPostIt, new PlacePostItMessageEvent()); 
            this.RequestHandlers.Add(Incoming.Mannequinshit, new SaveMannequinMessageEvent());
		}
		public void RoomsPets()
		{
            this.RequestHandlers.Add(Incoming.PetInfo, new GetPetInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.PlacePet, new PlacePetMessageEvent()); 
            this.RequestHandlers.Add(Incoming.PickupPet, new RemovePetFromFlatMessageEvent()); 
            this.RequestHandlers.Add(Incoming.TrainPet, new GetPetCommandsMessageEvent());
            this.RequestHandlers.Add(Incoming.RespetPet, new RespectPetMessageEvent()); 
		}
        public void RoomsBots()
        {
            this.RequestHandlers.Add(Incoming.BotDisposer, new PickupBotMessageEvent()); 
            this.RequestHandlers.Add(Incoming.BotComposer, new PlaceBotMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SerializeBotInformation, new EditBotInformations()); 
            this.RequestHandlers.Add(Incoming.BotSpeechList, new SerializeBotSpeeches()); 
        }
        public void RoomsPools()
        {
            /*this.RequestHandlers.Add(Incoming.ShowRoomPoll, new ShowRoomPoll());
            this.RequestHandlers.Add(Incoming.PollAnswer, new GetRoomPollAnswers());*/
            this.RequestHandlers.Add(Incoming.AnswerInfoBusPoll, new AnswerInfobusPoll()); 
        }
		public void RoomsSession()
		{
            this.RequestHandlers.Add(Incoming.GoToHotelView, new QuitMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadFirstRoomData, new OpenFlatConnectionMessageEvent()); 
            this.RequestHandlers.Add(Incoming.LoadHeightMap, new GetHeightmapMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ReqLoadByDoorBell, new ReqLoadByDoorBellMessageEvent());
            //this.RequestHandlers.Add(Incoming.AddUserToRoom, new AddUserToRoomMessageEvent());
            //this.RequestHandlers.Add(Incoming.InitParams, new GetInitParamsMessageEvent()); // Fixing
            // this.RequestHandlers.Add(3873, new AddUserToRoomMessageEvent());
            // this.RequestHandlers.Add(Incoming.OpenConnection, new OpenConnectionMessageEvent());
            //this.RequestHandlers.Add(Incoming.GoToFlat, new GoToFlatMessageEvent());
		}
		public void RoomsSettings()
		{
            this.RequestHandlers.Add(Incoming.GetRoomData, new GetRoomSettingsMessageEvent());
            this.RequestHandlers.Add(Incoming.SaveRoomData, new SaveRoomSettingsMessageEvent());
            this.RequestHandlers.Add(Incoming.RemoveRoom, new DeleteRoomMessageEvent());
            this.RequestHandlers.Add(Incoming.GetFlatControls, new GetFlatControlsMessageEvent());
		}
		public void Catalog()
		{
            this.RequestHandlers.Add(Incoming.OpenCatalog, new GetCatalogIndexEvent()); 
            this.RequestHandlers.Add(Incoming.OpenCatalogPage, new GetCatalogPageEvent()); 
			//this.RequestHandlers.Add(3031, new GetClubOffersMessageEvent()); useless
            this.RequestHandlers.Add(Incoming.CatalogData2, new GetGiftWrappingConfigurationEvent()); //UDATED
			//this.RequestHandlers.Add(3038, new GetHabboBasicMembershipExtendOfferEvent());
			//this.RequestHandlers.Add(3035, new GetHabboClubExtendOfferMessageEvent());
			//this.RequestHandlers.Add(3030, new GetIsOfferGiftableEvent());
            this.RequestHandlers.Add(Incoming.CatalogGetRace, new GetSellablePetBreedsEvent()); // Update
			//this.RequestHandlers.Add(3034, new MarkCatalogNewAdditionsPageOpenedEvent());
            //this.RequestHandlers.Add(Incoming.OpenGuildPage, new GetCatalogGroupWidgetData()); // GroupBadges
			//this.RequestHandlers.Add(3037, new PurchaseBasicMembershipExtensionEvent());
            this.RequestHandlers.Add(Incoming.PurchaseGift, new PurchaseFromCatalogAsGiftEvent()); 
            this.RequestHandlers.Add(Incoming.PurchaseCatalogItem, new PurchaseFromCatalogEvent()); 
			//this.RequestHandlers.Add(3036, new PurchaseVipMembershipExtensionEvent());
            this.RequestHandlers.Add(Incoming.RedeemVoucher, new RedeemVoucherMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenGuildPage, new OpenGuildPageMessageEvent()); 
            this.RequestHandlers.Add(Incoming.BuyGroup, new BuyGuildMessageEvent()); 
            //this.RequestHandlers.Add(475, new SelectClubGiftEvent());
		}
		public void Marketplace()
		{
			//this.RequestHandlers.Add(3013, new BuyMarketplaceTokensMessageEvent()); // Retired
            this.RequestHandlers.Add(Incoming.MarketplacePurchase, new BuyOfferMessageEvent()); 
            //this.RequestHandlers.Add(2805, new CancelOfferMessageEvent());
            //this.RequestHandlers.Add(2699, new GetMarketplaceCanMakeOfferEvent());
            //this.RequestHandlers.Add(2054, new GetMarketplaceItemStatsEvent());
            this.RequestHandlers.Add(Incoming.MarketplaceGetOffers, new GetOffersMessageEvent()); 
            //this.RequestHandlers.Add(521, new GetOwnOffersMessageEvent());
            //this.RequestHandlers.Add(2465, new MakeOfferMessageEvent());
            //this.RequestHandlers.Add(1753, new RedeemOfferCreditsMessageEvent());
            //this.RequestHandlers.Add(1937, new GetMarketplaceConfigurationMessageEvent());
            //Most of them useless because of my Search function.. :D
		}
		public void Recycler()
		{
            this.RequestHandlers.Add(Incoming.CatalogData1, new GetRecyclerPrizesMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RecycleAgain, new GetRecyclerStatusMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RecycleItem, new RecycleItemsMessageEvent()); 
		}
		public void Quest()
		{
            this.RequestHandlers.Add(Incoming.ActiveQuests, new AcceptQuestMessageEvent());
            this.RequestHandlers.Add(Incoming.OpenQuests, new GetQuestsMessageEvent());
            //this.RequestHandlers.Add(Incoming.ActiveQuests, new OpenQuestTrackerMessageEvent());
            this.RequestHandlers.Add(Incoming.CancelQuests, new RejectQuestMessageEvent());
            //STILL TODO BUT.. NOPE
		}
		public void InventoryPurse()
		{
           // this.RequestHandlers.Add(Incoming, new GetCreditsInfoEvent());
		}
		public void InventoryFurni()
		{
            this.RequestHandlers.Add(Incoming.PetInventary, new GetPetInventoryEvent()); 
            this.RequestHandlers.Add(Incoming.BotsInventary, new GetBotInventoryEvent()); 
            this.RequestHandlers.Add(Incoming.OpenInventory, new RequestFurniInventoryEvent()); 
		}
		public void InventoryBadges()
		{
            //this.RequestHandlers.Add(Incoming.Ba, new GetBadgePointLimitsEvent());
            this.RequestHandlers.Add(Incoming.BadgesInventary, new GetBadgesEvent()); 
            this.RequestHandlers.Add(Incoming.ApplyBadge, new SetActivatedBadgesEvent()); 
		}
		public void InventoryTrading()
		{
            this.RequestHandlers.Add(Incoming.UnacceptTrade, new UnacceptTradingEvent()); 
            this.RequestHandlers.Add(Incoming.AcceptTrade, new AcceptTradingEvent()); 
            this.RequestHandlers.Add(Incoming.StartTrade, new OpenTradingEvent()); 
            this.RequestHandlers.Add(Incoming.SendOffer, new AddItemToTradeEvent()); 
            this.RequestHandlers.Add(Incoming.ConfirmTrade, new ConfirmAcceptTradingEvent()); 
            //this.RequestHandlers.Add(Incoming.Trade, new ConfirmDeclineTradingEvent());
            this.RequestHandlers.Add(Incoming.CancelTrade, new ConfirmDeclineTradingEvent()); 
            this.RequestHandlers.Add(Incoming.CancelOffer, new RemoveItemFromTradeEvent()); 
		}
		public void InventoryAvatarFX()
		{
            this.RequestHandlers.Add(Incoming.StartEffect, new AvatarEffectSelectedEvent()); 
            this.RequestHandlers.Add(Incoming.EnableEffect, new AvatarEffectActivatedEvent()); 
		}
		public void InventoryAchievements()
		{
            this.RequestHandlers.Add(Incoming.OpenAchievements, new GetAchievementsEvent()); 
		}
#endregion
		public void Avatar()
		{
            this.RequestHandlers.Add(Incoming.ChangeMotto, new ChangeMottoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetWardrobe, new GetWardrobeMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SaveWardrobe, new SaveWardrobeOutfitMessageEvent()); 
		}
		public void Register()
		{
            this.RequestHandlers.Add(Incoming.ChangeLook, new UpdateFigureDataMessageEvent()); 
		}
		public void Users()
		{
            this.RequestHandlers.Add(Incoming.SerializeClub, new ScrGetUserInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.CheckPetName, new ApproveNameMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetUserTags, new GetUserTagsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetUserBadges, new GetSelectedBadgesMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.GetGuildFavorite, new GetHabboGroupBadgesMessageEvent());
            //this.RequestHandlers.Add(Incoming, new GetHabboGroupDetailsMessageEvent());
            //this.RequestHandlers.Add(Incoming., new LoadUserGroupsEvent());
            this.RequestHandlers.Add(Incoming.IgnoreUser, new IgnoreUserMessageEvent()); 
            this.RequestHandlers.Add(Incoming.UnignoreUser, new UnignoreUserMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendRespects, new RespectUserMessageEvent()); 
            /*this.RequestHandlers.Add(Incoming.ExitGuild, new ExitGuildEvent()); // NEW
			this.RequestHandlers.Add(Incoming.SendRequestGuild, new JoinGuildEvent());
            //this.RequestHandlers.Add(Incoming.Guil, new GetGuildFavorite());
            this.RequestHandlers.Add(Incoming.RemoveGuildFavorite, new RemoveGuildFavorite());*/
            this.RequestHandlers.Add(Incoming.LoadProfile, new GetUserProfileEvent()); 
            //this.RequestHandlers.Add(Incoming.Gu, new GetGuildMembers()); // New
		}
        public void TalentTrack()
        {
            this.RequestHandlers.Add(Incoming.LoadTalents, new GetTalentTrack());
            this.RequestHandlers.Add(Incoming.StartQuiz, new StartHabboWay());
            this.RequestHandlers.Add(Incoming.EndQuiz, new PostQuizAnswers());
        }
		public void Help()
		{
            this.RequestHandlers.Add(Incoming.CreateTicket, new CallForHelpMessageEvent()); 
            this.RequestHandlers.Add(Incoming.CloseIssue, new CloseIssuesMessageEvent()); 
			//this.RequestHandlers.Add(Incoming.Help, new DeletePendingCallsForHelpMessageEvent());
            this.RequestHandlers.Add(Incoming.IssueChatlog, new GetCfhChatlogMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ToolForThisRoom, new GetModeratorRoomInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ToolForUser, new GetModeratorUserInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.OpenRoomChatlog, new GetRoomChatlogMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetRoomVisits, new GetRoomVisitsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.UserChatlog, new GetUserChatlogMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendMessageByTemplate, new ModAlertMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ModActionBanUser, new ModBanMessageEvent()); 
            //this.RequestHandlers.Add(Incoming.SendUserMessage, new ModerateRoomMessageEvent());
            this.RequestHandlers.Add(Incoming.SendRoomAlert, new ModeratorActionMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ModActionKickUser, new ModKickMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendUserMessage, new ModMessageMessageEvent()); 
            this.RequestHandlers.Add(Incoming.PickIssue, new PickIssuesMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ReleaseIssue, new ReleaseIssuesMessageEvent()); 
			//this.RequestHandlers.Add(419, new SearchFaqsMessageEvent());
            this.RequestHandlers.Add(Incoming.OpenHelpTool, new GetHelpToolMessageEvent()); 
           // this.RequestHandlers.Add(1683, new IDONTKNOWWHATTHISISFOR());
		}
		public void Sound()
		{
            /*this.RequestHandlers.Add(Incoming.GetSoundMachinePlayList, new GetSoundMachinePlayListMessageEvent());
            this.RequestHandlers.Add(Incoming.GetSongInfo, new GetSongInfoMessageEvent());
            this.RequestHandlers.Add(Incoming.GetNowPlaying, new GetNowPlayingMessageEvent());
            this.RequestHandlers.Add(Incoming.GetPlaylists, new GetJukeboxPlayListMessageEvent());
            /* handlers.Add(Incoming.ListenPreview, new StaticRequestHandler(SharedPacketLib.GetMusicData));
            handlers.Add(Incoming.LoadInvSongs, new StaticRequestHandler(SharedPacketLib.LoadInvSongs));
            handlers.Add(Incoming.LoadJukeSongs, new StaticRequestHandler(SharedPacketLib.LoadJukeSongs));
            handlers.Add(Incoming.AddNewCdToJuke, new StaticRequestHandler(SharedPacketLib.AddNewCdToJuke));
            handlers.Add(Incoming.RemoveCdToJuke, new StaticRequestHandler(SharedPacketLib.RemoveCdToJuke));
            */
           // this.RequestHandlers.Add(1493, new GetUserSongDisksMessageEvent());*
            this.RequestHandlers.Add(Incoming.volumeControl, new AdjustVolumeControlEvent());
		}
        public void Jukebox()
        {
            this.RequestHandlers.Add(Incoming.AddNewCdToJuke, new AddNewJukeboxCD());
            this.RequestHandlers.Add(Incoming.RemoveCdToJuke, new RemoveCDToJukebox());
        }
		public void Wired()
		{
            this.RequestHandlers.Add(Incoming.SaveWiredEffect, new UpdateTriggerMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SaveWiredTrigger, new UpdateActionMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SaveConditional, new UpdateConditionMessageEvent()); 
            //this.RequestHandlers.Add(Incoming, new ApplyFurniToSetConditions());
		}
        public void FriendStream()
        {
            this.RequestHandlers.Add(Incoming.InitStream, new GetEventStreamComposer()); 
       //     this.RequestHandlers.Add(501, new SetEventStreamingAllowedComposer());
            this.RequestHandlers.Add(Incoming.StreamLike, new EventStreamingLikeButton()); 
            this.RequestHandlers.Add(Incoming.CreateStream, new UpdateStream()); 
            this.RequestHandlers.Add(Incoming.SearchStream, new GetUserStream()); 
        }
        public void Games()
        {
            this.RequestHandlers.Add(Incoming.GetGames, new GetGames());
            this.RequestHandlers.Add(Incoming.GetGame, new GetGame());
            //Snowwar :)
            //this.RequestHandlers.Add(Incoming.GetGames, new GetGames());
            //this.RequestHandlers.Add(Incoming.GetGame, new GetGame());
            this.RequestHandlers.Add(2372, new StartPanel());
            //this.RequestHandlers.Add(607, new RequestFullGameStatus());
            this.RequestHandlers.Add(Incoming.Game2LeaveGameMessageComposer, new LeaveGame());
            this.RequestHandlers.Add(Incoming.Game2GameChatMessageComposer, new TalkGame());
            //this.RequestHandlers.Add(Incoming.Game2LoadStageReadyMessageComposer, new StageReadyMessage());
            this.RequestHandlers.Add(Incoming.Game2SetUserMoveTargetMessageComposer, new WalkGame());
            //FastFood :)))

        }
        public void Relationships()
        {
            this.RequestHandlers.Add(Incoming.SetRelationshipStatus, new SetRelationshipsStatusMessage()); 
            this.RequestHandlers.Add(Incoming.GetRelationshipsProfile, new GetRelationshipsProfile()); 
        }
        public void Guilds()
        {
            this.RequestHandlers.Add(Incoming.EditGuild, new EditGuildMessageEvent()); 
            this.RequestHandlers.Add(Incoming.EditIdentidad, new EditGroupNameMessageEvent()); 
            this.RequestHandlers.Add(Incoming.EditPlaca, new EditGroupHomeRoomMessageEvent()); 
            this.RequestHandlers.Add(Incoming.EditColores, new EditGroupColorsMessageEvent()); 
            this.RequestHandlers.Add(Incoming.EditAjustes, new EditPrivilegesMessageEvent()); 
            this.RequestHandlers.Add(Incoming.ExitGuild, new ExitGuildMessageEvent()); 
            this.RequestHandlers.Add(Incoming.SendRequestGuild, new SendGuildRequestEvent()); 
            this.RequestHandlers.Add(Incoming.LoadMembersPetitions, new LoadGuildRequestsEvent()); 
            this.RequestHandlers.Add(Incoming.RejectGuildMember, new DeclineGuildRequestEvent()); 
            this.RequestHandlers.Add(Incoming.AcceptMember, new AcceptGuildRequestEvent()); 
            this.RequestHandlers.Add(Incoming.UpdateUserToRankGuild, new UpdateUserToRankGuild()); 
            this.RequestHandlers.Add(Incoming.UpdateUserFromRankGuild, new UpdateUserToRankGuild()); 
            this.RequestHandlers.Add(Incoming.SendFurniGuild, new SendFurniGuildMessageEvent()); 
            this.RequestHandlers.Add(Incoming.GetGuildFavorite, new GetGuildFavoriteMessageEvent()); 
            this.RequestHandlers.Add(Incoming.RemoveGuildFavorite, new RemoveFavoriteGuildEvent()); 
            this.RequestHandlers.Add(Incoming.EndConfirmBuy, new GuildInfoMessageEvent()); 
            this.RequestHandlers.Add(Incoming.TakeRights, new UpdateUserToRankGuild());
        }
        public void GuideTool()
        {
            this.RequestHandlers.Add(Incoming.OpenGuideTool, new OpenGuideToolMessageEvent()); //3005 by Rootkit :)
            this.RequestHandlers.Add(Incoming.CallGuide, new CallGuideMessageEvent()); //2756 by Rootkit :)
            this.RequestHandlers.Add(Incoming.AcceptOrDeclineGuideRequest, new AcceptOrDeclineGuideRequestEvent()); //3971 by Rootkit :)
            this.RequestHandlers.Add(Incoming.GuideChat, new GuideChatMessageEvent()); //215 by Rootkit :)
            this.RequestHandlers.Add(Incoming.FollowUser, new FollowUserMessageEvent()); //2973 by Rootkit :)
            this.RequestHandlers.Add(Incoming.InviteUser, new InviteUserMessageEvent()); //1088 by Rootkit :)
            this.RequestHandlers.Add(Incoming.CloseGuideTicket, new CloseGuideTicketMessageEvent()); //3504 by Rootkit :)
            this.RequestHandlers.Add(Incoming.ToggleTypingState, new GuideUserStartsTypingMessageEvent()); //1261 by Rootkit :)
        }
		public void Clear()
		{
			this.RequestHandlers.Clear();
		}
	}
    class LoadHomeRoomlel : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Room room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().HomeRoomId);
            if (room != null)
            {
                ServerMessage Message = new ServerMessage(Outgoing.RoomForward);
                Message.AppendBoolean(room.IsPublic);
                Message.AppendUInt(room.Id);
                Session.SendMessage(Message);
            }
        }
    }
}
