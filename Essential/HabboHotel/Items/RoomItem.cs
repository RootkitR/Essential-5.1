using System;
using System.Collections.Generic;
using System.Linq;
using Essential.Core;
using Essential.HabboHotel.Pathfinding;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Items.Interactors;
using Essential.HabboHotel.GameClients;
using Essential.Storage;
using Essential.HabboHotel.SoundMachine;
using Essential.HabboHotel.Rooms.Games;
using Essential.Util;
using System.Globalization;
using System.Threading;
namespace Essential.HabboHotel.Items
{
    internal sealed class RoomItem
    {
        internal enum Enum5
        {
            const_0,
            const_1,
            const_2
        }
        internal uint uint_0;
        internal uint uint_1;
        internal uint uint_2;
        internal string ExtraData;
        internal FreezePowerUp freezePowerUp;
        internal bool bool_0;
        internal string string_1;
        internal string string_2;
        internal string string_3;
        internal string string_4;
        internal string string_5;
        internal string string_6;
        private Dictionary<int, AffectedTile> dictionary_0;
        private int mX;
        private int mY;
        private double double_0;
        internal RoomItem.Enum5 enum5_0;
        internal int int_3;
        internal string string_7;
        internal bool bool_1;
        internal int int_4;
        internal uint InteractingUser;
        internal uint uint_4;
        internal Dictionary<RoomUser, int> dictionary_1;
        private Item Item;
        private Room class14_0;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        public int FireWorkCount;
        public RoomUser LastPlayerHitFootball;
        internal Team team;
        internal bool WiredAtTimeNeedReset;
        internal double WiredAtTimeTimer;
        internal bool WiredNeedReset;
        internal double WiredCounter;
        internal int LimitedId;
        internal int LimitetCnt;
        internal string GuildData;
        public int FootballDirection;
        public int FootballWaitTime;

        internal Dictionary<int, AffectedTile> Dictionary_0
        {
            get
            {
                return this.dictionary_0;
            }
        }
        internal int GetX
        {
            get
            {
                return this.mX;
            }
        }
        internal int Int32_1
        {
            get
            {
                return this.mY;
            }
        }
        internal double Double_0
        {
            get
            {
                return this.double_0;
            }
        }
        internal bool Boolean_0
        {
            get
            {
                return this.bool_4;
            }
        }
        internal ThreeDCoord GStruct1_0
        {
            get
            {
                return new ThreeDCoord(this.mX, this.mY);
            }
        }
        internal double Double_1
        {
            get
            {
                double result;
                if (this.GetBaseItem().Height_Adjustable.Count > 1)
                {
                    int index;
                    if (int.TryParse(this.ExtraData, out index))
                    {
                        result = this.double_0 + this.GetBaseItem().Height_Adjustable[index];
                    }
                    else
                    {
                        result = this.double_0 + this.GetBaseItem().Height;
                    }
                }
                else
                {
                    result = this.double_0 + this.GetBaseItem().Height;
                }
                return result;
            }
        }
        internal bool Boolean_1
        {
            get
            {
                return this.bool_2;
            }
        }
        internal bool Boolean_2
        {
            get
            {
                return this.bool_3;
            }
        }
        internal ThreeDCoord GStruct1_1
        {
            get
            {
                ThreeDCoord result = new ThreeDCoord(this.mX, this.mY);
                if (this.int_3 == 0)
                {
                    result.y--;
                }
                else
                {
                    if (this.int_3 == 2)
                    {
                        result.x++;
                    }
                    else
                    {
                        if (this.int_3 == 4)
                        {
                            result.y++;
                        }
                        else
                        {
                            if (this.int_3 == 6)
                            {
                                result.x--;
                            }
                        }
                    }
                }
                return result;
            }
        }
        internal ThreeDCoord GStruct1_2
        {
            get
            {
                ThreeDCoord result = new ThreeDCoord(this.mX, this.mY);
                if (this.int_3 == 0)
                {
                    result.y++;
                }
                else
                {
                    if (this.int_3 == 2)
                    {
                        result.x--;
                    }
                    else
                    {
                        if (this.int_3 == 4)
                        {
                            result.y--;
                        }
                        else
                        {
                            if (this.int_3 == 6)
                            {
                                result.x++;
                            }
                        }
                    }
                }
                return result;
            }
        }
        internal FurniInteractor Interactor
        {
            get
            {
                string text = this.GetBaseItem().InteractionType.ToLower();
                FurniInteractor result;
                if (this.GetBaseItem().PrivateTile)
                {
                    return new InteractorPrivateTile(this.GetBaseItem().Modes);
                }
                if (this.GetBaseItem().Tent)
                {
                    return new InteractorTent();
                }
                switch (text)
                {
                    case "ball":
                        result = new InteractorFootball();
                        return result;
                    case "teleport":
                        result = new InteractorTeleport();
                        return result;
                    case "instant_teleport":
                        result = new InteractorInstantTeleport();
                        return result;
                    case "bottle":
                        result = new InteractorSpinningBottle();
                        return result;
                    case "dice":
                        result = new InteractorDice();
                        return result;
                    case "habbowheel":
                        result = new InteractorHabboWheel();
                        return result;
                    case "loveshuffler":
                        result = new InteractorLoveShuffler();
                        return result;
                    case "onewaygate":
                        result = new InteractorOneWayGate();
                        return result;
                    case "alert":
                        result = new Class89();
                        return result;
                    case "vendingmachine":
                        result = new InteractorVendor();
                        return result;
                    case "gate":
                        result = new InteractorGate(this.GetBaseItem().Modes);
                        return result;
                    case "scoreboard":
                        result = new InteractorScoreboard();
                        return result;
                    case "counter":
                        result = new InteractorBanzaiScoreCounter();
                        return result;
                    case "wired":
                        result = new WiredInteractor();
                        return result;
                    case "wf_trg_onsay":
                        result = new InteractorWiredOnSay();
                        return result;
                    case "wf_trg_enterroom":
                        result = new InteractorWiredEnterRoom();
                        return result;
                    case "wf_act_saymsg":
                    case "wf_act_give_phx":
                    case "wf_cnd_phx":
                        result = new InteractorSuperWired();
                        return result;
                    case "wf_trg_furnistate":
                    case "wf_trg_onfurni":
                    case "wf_trg_offfurni":
                    case "wf_act_moveuser":
                    case "wf_act_togglefurni":
                        result = new InteractorWiredTriggerState();
                        return result;
                    case "wf_trg_gameend":
                    case "wf_trg_gamestart":
                        result = new InteractorWiredTriggerGame();
                        return result;
                    case "wf_trg_timer":
                        result = new InteractorWiredTriggerTimer();
                        return result;
                    case "wf_act_givepoints":
                        result = new InteractorWiredGivePoints();
                        return result;
                    case "wf_trg_attime":
                        result = new InteractorWiredAtTime();
                        return result;
                    case "wf_trg_atscore":
                        result = new InteractorWiredAtScore();
                        return result;
                    case "wf_act_moverotate":
                        result = new InteractorWiredMoveRotate();
                        return result;
                    case "wf_act_matchfurni":
                        result = new InteractorWiredMatchFurni();
                        return result;
                    case "wf_cnd_trggrer_on_frn":
                    case "wf_cnd_furnis_hv_avtrs":
                    case "wf_cnd_has_furni_on":
                    case "wf_cnd_not_hv_avtrs":
                        result = new InteractorWiredCondition();
                        return result;
                    case "wf_cnd_has_handitem":
                        result = new InteractorConditionUserHasHandItem();
                        return result;
                    case "wf_cnd_match_snapshot":
                    case "wf_cnd_not_match_snap":
                        result = new InteractorWiredConditionFurniStatesAndPositionsMatch();
                        return result;
                    case "wf_cnd_actor_in_team":
                        result = new InteractorConditionUserInTeam();
                        return result;
                    case "wf_cnd_not_in_team":
                        result = new InteractorConditionUserNotInTeam();
                        return result;
                    case "wf_cnd_time_more_than":
                    case "wf_cnd_time_less_than":
                        result = new InteractorWiredConditionTimeMoreOrLess();
                        return result;
                    case "puzzlebox":
                        result = new InteractorPuzzleBox();
                        return result;
                    case "mannequin":
                        result = new InteractorMannequin();
                        return result;
                    case "firework":
                        result = new InteractorFirework();
                        return result;
                    case "wf_act_kick_user":
                        result = new InteractorWiredKickUser();
                        return result;
                    case "hopper":
                        result = new InteractorHopper();
                        return result;
                    case "jukebox":
                        result = new InteractorJukebox();
                        return result;
                    case "freeze_tile":
                        result = new InteractorFreezeTile();
                        return result;
                    case "freeze_counter":
                        result = new InteractorFreezeCounter();
                        return result;
                    case "freeze_ice_block":
                        result = new InteractorFreezeIceBlock();
                        return result;
                    case "puppet":
                        result = new InteractorPuppet();
                        return result;
                    case "bancomat":
                        result = new InteractorBancomat();
                        return result;
                    case "stackfield":
                        result = new InteractorStackField();
                        return result;
                    case "badge_display":
                        result = new InteractorBadgeDisplay();
                        return result;
                    case "wf_cnd_actor_in_group":
                    case "wf_cnd_not_in_group":
                        result = new InteractorConditionActorInGroup();
                        return result;
                    case "wf_cnd_user_count_in":
                    case "wfc_cnd_not_user_count":
                        result = new InteractorConditionUserCount();
                        return result;
                    case "wf_cnd_not_furni_on":
                        result = new InteractorConditionNoFurniOnItem();
                        return result;
                    case "football_scoreboard_green":
                    case "football_scoreboard_blue":
                    case "football_scoreboard_yellow":
                    case "football_scoreboard_red":
                        result = new InteractorScoreCounter();
                        return result;
                    case "youtubetv":
                        result = new InteractorYoutubeTV();
                        return result;
                    case "atm_flappybird":
                        result = new InteractorFlappyBirdATM();
                        return result;
                    case "atm_agario":
                        result = new InteractorAgarioATM();
                        return result;
                    case "background":
                        result = new InteractorBackground();
                        return result;
                    case "wf_act_bot_follow_avatar":
                        result = new InteractorWiredFXBotFollowsUser();
                        return result;
                    case "wf_act_bot_give_handitem":
                        result = new InteractorWiredFXBotGivesHanditem();
                        return result;
                    case "wf_act_bot_talk":
                        result = new InteractorWiredFXBotTalkToUsers();
                        return result;
                    case "wf_act_bot_talk_to_avatar":
                        result = new InteractorWiredFXBotTalksToAvatar();
                        return result;
                    case "wf_act_bot_clothes":
                        result = new InteractorWiredFXBotChangesLook();
                        return result;
                    case "wf_act_yt":
                        result = new InteractorYoutubeWired();
                        return result;
                    case "wf_act_img":
                        result = new InteractorImageWired();
                        return result;
                    case "wf_cnd_has_purse":
                    case "wf_cnd_hasnot_purse":
                        result = new InteractorConditionHasPurse();
                        return result;
                    case "surprisebox":
                        result = new InteractorRandomBox();
                        return result;
                    /*case "wf_act_bot_teleport":
                        result = new InteractorWiredFXTeleportBotToFurni();
                        return result;*/
                }
                result = new InteractorDefault(this.GetBaseItem().Modes);
                return result;
            }
        }
        public RoomItem(uint uint_5, uint uint_6, uint uint_7, string string_8, int int_5, int int_6, double double_1, int int_7, string string_9, Room class14_1, int LimitedId, int LimitedCnt, string GuildData)
        {
            this.uint_0 = uint_5;
            this.uint_1 = uint_6;
            this.uint_2 = uint_7;
            this.ExtraData = string_8;
            this.mX = int_5;
            this.mY = int_6;
            this.double_0 = double_1;
            this.int_3 = int_7;
            this.string_7 = string_9;
            this.bool_1 = false;
            this.int_4 = 0;
            this.InteractingUser = 0u;
            this.uint_4 = 0u;
            this.bool_0 = false;
            this.LimitedId = LimitedId;
            this.LimitetCnt = LimitedCnt;
            this.string_1 = "none";
            this.enum5_0 = RoomItem.Enum5.const_0;
            this.string_2 = "";
            this.string_3 = "";
            this.string_4 = "";
            this.string_5 = "";
            this.string_6 = "";
            this.FireWorkCount = 0;
            this.dictionary_1 = new Dictionary<RoomUser, int>();
            this.Item = Essential.GetGame().GetItemManager().GetItemById(uint_7);
            this.class14_0 = class14_1;
            if (this.GetBaseItem() == null)
            {
                Logging.LogException("Unknown baseID: " + uint_7);
            }
            string text = this.GetBaseItem().InteractionType.ToLower();
            if (text != null)
            {
                if (!(text == "teleport"))
                {
                    if (!(text == "hopper"))
                    {
                        if (!(text == "roller"))
                        {
                            if (!(text == "blue_score"))
                            {
                                if (!(text == "green_score"))
                                {
                                    if (!(text == "red_score"))
                                    {
                                        if (text == "yellow_score")
                                        {
                                            this.string_1 = "yellow";
                                        }
                                    }
                                    else
                                    {
                                        this.string_1 = "red";
                                    }
                                }
                                else
                                {
                                    this.string_1 = "green";
                                }
                            }
                            else
                            {
                                this.string_1 = "blue";
                            }
                        }
                        else
                        {
                            this.bool_4 = true;
                            class14_1.Boolean_1 = true;
                        }
                    }
                    else
                    {
                        this.ReqUpdate(0);
                    }
                }
                else
                {
                    this.ReqUpdate(0);
                }
            }

            if (text != null)
            {
                switch (text)
                {
                    case "freeze_blue_gate":
                    case "freeze_blue_score":
                    case "bb_blue_gate":
                        this.team = Team.Blue;
                        break;
                    case "freeze_red_gate":
                    case "freeze_red_score":
                    case "bb_red_gate":
                        this.team = Team.Red;
                        break;
                    case "freeze_green_gate":
                    case "freeze_green_score":
                    case "bb_green_gate":
                        this.team = Team.Green;
                        break;
                    case "freeze_yellow_gate":
                    case "freeze_yellow_score":
                    case "bb_yellow_gate":
                        this.team = Team.Yellow;
                        break;
                    case "jukebox":
                        RoomMusicController roomMusicController = this.GetRoom().GetRoomMusicController();
                        roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(this);
                        break;
                }
            }

            this.bool_2 = (this.GetBaseItem().Type == 'i');
            this.bool_3 = (this.GetBaseItem().Type == 's');
            this.dictionary_0 = this.GetRoom().method_94(this.GetBaseItem().Length, this.GetBaseItem().Width, this.mX, this.mY, int_7);
            this.GuildData = GuildData;
        }
        internal void SetPosition(int int_5, int int_6, double double_1)
        {
            this.mX = int_5;
            this.mY = int_6;
            this.double_0 = double_1;
            this.dictionary_0 = this.GetRoom().method_94(this.GetBaseItem().Length, this.GetBaseItem().Width, this.mX, this.mY, this.int_3);
        }
        internal void setHeight(double height)
        {
            this.double_0 = height;
        }
        internal ThreeDCoord GetNextThreeDCoord(int int_5)
        {
            ThreeDCoord result = new ThreeDCoord(this.mX, this.mY);
            if (int_5 == 0)
            {
                result.y++;
            }
            else
            {
                if (int_5 == 2)
                {
                    result.x--;
                }
                else
                {
                    if (int_5 == 4)
                    {
                        result.y--;
                    }
                    else
                    {
                        if (int_5 == 6)
                        {
                            result.x++;
                        }
                    }
                }
            }
            return result;
        }
        internal void ProcessUpdates()
        {
            this.int_4--;
            if (this.int_4 <= 0)
            {
                this.bool_1 = false;
                this.int_4 = 0;
                string text = this.GetBaseItem().InteractionType.ToLower();
                switch (text)
                {
                    case "onewaygate":
                        {
                            RoomUser @class = null;
                            if (this.InteractingUser > 0u)
                            {
                                @class = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
                            }
                            if (@class != null && @class.X == this.mX && @class.Y == this.mY && this.string_2 != "tried")
                            {
                                this.ExtraData = "1";
                                this.string_2 = "tried";
                                @class.method_6();
                                @class.MoveTo(this.GStruct1_2);
                                this.ReqUpdate(0);
                                this.UpdateState(false, true);
                            }
                            else
                            {
                                if ((@class != null && ThreeDCoord.Equals(@class.Position, this.GStruct1_2)) || this.string_2 == "tried")
                                {
                                    this.string_2 = "";
                                    this.ExtraData = "0";
                                    this.InteractingUser = 0u;
                                    this.UpdateState(false, true);
                                    this.GetRoom().method_22();
                                }
                                else
                                {
                                    if (this.ExtraData == "1")
                                    {
                                        this.ExtraData = "0";
                                        this.UpdateState(false, true);
                                    }
                                }
                            }
                            if (@class == null)
                            {
                                this.InteractingUser = 0u;
                            }
                            break;
                        }
                    case "instant_teleport":
                        {
                            bool flag = false;
                            bool flag2 = false;
                            if (this.InteractingUser > 0u)
                            {
                                RoomUser @class = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
                                if (@class != null)
                                {

                                    @class.bool_1 = true;
                                    if (@class.int_19 == -1)
                                    {
                                        @class.int_19 = 2;
                                    }
                                    if (TeleHandler.IsInRoom(this.uint_0))
                                    {
                                        flag2 = true;


                                        if (@class.int_19 == 0)
                                        {
                                            uint num2 = TeleHandler.GetOther(this.uint_0);
                                            uint num3 = TeleHandler.GetRoomByItemId(num2);
                                            if (num3 == this.uint_1)
                                            {
                                                RoomItem class2 = this.GetRoom().method_28(num2);
                                                if (class2 == null)
                                                {
                                                    @class.method_6();
                                                }
                                                else
                                                {
                                                    @class.X = class2.GetX;
                                                    @class.Y = class2.Int32_1;
                                                    @class.double_0 = class2.Double_1;
                                                    @class.UpdateNeeded = true;
                                                    class2.ExtraData = "2";
                                                    class2.UpdateState(false, true);
                                                    class2.uint_4 = this.InteractingUser;
                                                }
                                            }
                                            else
                                            {
                                                if (!@class.IsBot)
                                                {
                                                    Essential.GetGame().GetRoomManager().method_5(new TeleUserData(@class.GetClient().GetClientMessageHandler(), @class.GetClient().GetHabbo(), num3, num2));
                                                }
                                            }
                                            this.InteractingUser = 0u;
                                        }
                                        else
                                        {
                                            @class.int_19--;
                                        }
                                    }
                                    else
                                    {
                                        @class.method_6();
                                        this.InteractingUser = 0u;
                                        @class.MoveTo(this.GStruct1_1);
                                    }


                                }
                                else
                                {
                                    this.InteractingUser = 0u;
                                }
                            }
                            if (this.uint_4 > 0u)
                            {
                                RoomUser class3 = this.GetRoom().GetRoomUserByHabbo(this.uint_4);
                                if (class3 != null)
                                {
                                    flag = true;
                                    class3.method_6();
                                    if (ThreeDCoord.Equals(class3.Position, this.GStruct1_0))
                                    {
                                        class3.MoveTo(this.GStruct1_1);
                                    }
                                }
                                this.uint_4 = 0u;
                            }
                            if (flag)
                            {
                                if (this.ExtraData != "1")
                                {
                                    this.ExtraData = "1";
                                    this.UpdateState(false, true);
                                }
                            }
                            else
                            {
                                if (flag2)
                                {
                                    if (this.ExtraData != "2")
                                    {
                                        this.ExtraData = "2";
                                        this.UpdateState(false, true);
                                    }
                                }
                                else
                                {
                                    if (this.ExtraData != "0")
                                    {
                                        this.ExtraData = "0";
                                        this.UpdateState(false, true);
                                    }
                                }
                            }
                            this.ReqUpdate(1);
                            break;
                        }
                    case "teleport":
                        {
                            bool flag = false;
                            bool flag2 = false;
                            if (this.InteractingUser > 0)
                            {
                                RoomUser @class = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
                                if (@class != null)
                                {
                                    if (ThreeDCoord.Equals(@class.Position, this.GStruct1_0))
                                    {
                                        @class.bool_1 = false;
                                        if (@class.int_19 == -1)
                                        {
                                            @class.int_19 = 1;
                                        }
                                        if (TeleHandler.IsInRoom(this.uint_0))
                                        {
                                            flag2 = true;

                                            if (this.GetBaseItem().Name == "xmas10_fireplace")
                                            {
                                                string look = @class.GetClient().GetHabbo().Figure;

                                                string[] lissut = look.Split('.');

                                                if (look.Contains("ha-"))
                                                {
                                                    look = look.Replace("" + lissut[Array.FindIndex(lissut, row => row.Contains("ha-"))], "ha-1006-62");

                                                }
                                                else
                                                {
                                                    look = look + ".ha-1006-62";
                                                }
                                                @class.GetClient().GetHabbo().Figure = Essential.FilterString(look.ToLower());

                                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                {
                                                    dbClient.AddParamWithValue("look", look);
                                                    dbClient.ExecuteQuery("UPDATE users SET look =  @look WHERE id = " + @class.GetClient().GetHabbo().Id);
                                                }

                                                ServerMessage serverMessage = new ServerMessage(Outgoing.UpdateUserInformation);
                                                serverMessage.AppendInt32(-1);
                                                serverMessage.AppendStringWithBreak(@class.GetClient().GetHabbo().Figure);
                                                serverMessage.AppendStringWithBreak(@class.GetClient().GetHabbo().Gender.ToLower());
                                                serverMessage.AppendStringWithBreak(@class.GetClient().GetHabbo().Motto);
                                                serverMessage.AppendInt32(@class.GetClient().GetHabbo().AchievementScore);
                                                //serverMessage.AppendStringWithBreak("");
                                                @class.GetClient().SendMessage(serverMessage);
                                            }

                                            if (@class.int_19 == 0)
                                            {
                                                uint num2 = TeleHandler.GetOther(this.uint_0);
                                                uint num3 = TeleHandler.GetRoomByItemId(num2);
                                                if (num3 == this.uint_1)
                                                {
                                                    RoomItem class2 = this.GetRoom().method_28(num2);
                                                    if (class2 == null)
                                                    {
                                                        @class.method_6();
                                                    }
                                                    else
                                                    {
                                                        @class.method_7(class2.GetX, class2.Int32_1, class2.Double_0);
                                                        @class.method_9(class2.int_3);
                                                        class2.ExtraData = "2";
                                                        class2.UpdateState(false, true);
                                                        class2.uint_4 = this.InteractingUser;
                                                    }
                                                }
                                                else
                                                {
                                                    if (!@class.IsBot)
                                                    {
                                                        Essential.GetGame().GetRoomManager().method_5(new TeleUserData(@class.GetClient().GetClientMessageHandler(), @class.GetClient().GetHabbo(), num3, num2));
                                                    }
                                                }
                                                this.InteractingUser = 0u;
                                            }
                                            else
                                            {
                                                @class.int_19--;
                                            }
                                        }
                                        else
                                        {
                                            @class.method_6();
                                            this.InteractingUser = 0u;
                                            @class.MoveTo(this.GStruct1_1);
                                        }
                                    }
                                    else
                                    {
                                        if (ThreeDCoord.Equals(@class.Position, this.GStruct1_1) && @class.RoomItem_0 == this)
                                        {
                                            @class.bool_1 = true;
                                            flag = true;
                                            if (@class.bool_6 && (@class.int_10 != this.mX || @class.int_11 != this.mY))
                                            {
                                                @class.method_3(true);
                                            }
                                            @class.bool_0 = false;
                                            @class.bool_1 = true;
                                            @class.MoveTo(this.GStruct1_0);
                                        }
                                        else
                                        {
                                            this.InteractingUser = 0u;
                                        }
                                    }
                                }
                                else
                                {
                                    this.InteractingUser = 0u;
                                }
                            }
                            if (this.uint_4 > 0u)
                            {
                                RoomUser class3 = this.GetRoom().GetRoomUserByHabbo(this.uint_4);
                                if (class3 != null)
                                {
                                    flag = true;
                                    class3.method_6();
                                    if (ThreeDCoord.Equals(class3.Position, this.GStruct1_0))
                                    {
                                        class3.MoveTo(this.GStruct1_1);
                                    }
                                }
                                this.uint_4 = 0u;
                            }
                            if (flag)
                            {
                                if (this.ExtraData != "1")
                                {
                                    this.ExtraData = "1";
                                    this.UpdateState(false, true);
                                }
                            }
                            else
                            {
                                if (flag2)
                                {
                                    if (this.ExtraData != "2")
                                    {
                                        this.ExtraData = "2";
                                        this.UpdateState(false, true);
                                    }
                                }
                                else
                                {
                                    if (this.ExtraData != "0")
                                    {
                                        this.ExtraData = "0";
                                        this.UpdateState(false, true);
                                    }
                                }
                            }
                            this.ReqUpdate(1);
                            break;
                        }
                    case "hopper":
                        {
                            bool flag = false;
                            bool flag2 = false;
                            if (this.InteractingUser > 0u)
                            {
                                RoomUser @class = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
                                if (@class != null)
                                {
                                    if (ThreeDCoord.Equals(@class.Position, this.GStruct1_0))
                                    {
                                        @class.bool_1 = false;
                                        if (@class.int_19 == -1)
                                        {
                                            @class.int_19 = 1;
                                        }
                                        if (HopperHandler.IsInRoom(this.uint_0))
                                        {
                                            flag2 = true;
                                            if (@class.int_19 == 0)
                                            {
                                                uint num2 = HopperHandler.GetOtherId(this.uint_0);
                                                uint num3 = HopperHandler.GetRoomId(num2);
                                                if (num3 == this.uint_1)
                                                {
                                                    RoomItem class2 = this.GetRoom().method_28(num2);
                                                    if (class2 == null)
                                                    {
                                                        @class.method_6();
                                                    }
                                                    else
                                                    {
                                                        @class.method_7(class2.GetX, class2.Int32_1, class2.Double_0);
                                                        @class.method_9(class2.int_3);
                                                        class2.ExtraData = "2";
                                                        class2.UpdateState(false, true);
                                                        class2.uint_4 = this.InteractingUser;
                                                    }
                                                }
                                                else
                                                {
                                                    if (!@class.IsBot)
                                                    {
                                                        Essential.GetGame().GetRoomManager().method_5(new TeleUserData(@class.GetClient().GetClientMessageHandler(), @class.GetClient().GetHabbo(), num3, num2));
                                                    }
                                                }
                                                this.InteractingUser = 0u;
                                            }
                                            else
                                            {
                                                @class.int_19--;
                                            }
                                        }
                                        else
                                        {
                                            @class.method_6();
                                            this.InteractingUser = 0u;
                                            @class.MoveTo(this.GStruct1_1);
                                        }
                                    }
                                    else
                                    {
                                        if (ThreeDCoord.Equals(@class.Position, this.GStruct1_1) && @class.RoomItem_0 == this)
                                        {
                                            @class.bool_1 = true;
                                            flag = true;
                                            if (@class.bool_6 && (@class.int_10 != this.mX || @class.int_11 != this.mY))
                                            {
                                                @class.method_3(true);
                                            }
                                            @class.bool_0 = false;
                                            @class.bool_1 = true;
                                            @class.MoveTo(this.GStruct1_0);
                                        }
                                        else
                                        {
                                            this.InteractingUser = 0u;
                                        }
                                    }
                                }
                                else
                                {
                                    this.InteractingUser = 0u;
                                }
                            }
                            if (this.uint_4 > 0u)
                            {
                                RoomUser class3 = this.GetRoom().GetRoomUserByHabbo(this.uint_4);
                                if (class3 != null)
                                {
                                    flag = true;
                                    class3.method_6();
                                    if (ThreeDCoord.Equals(class3.Position, this.GStruct1_0))
                                    {
                                        class3.MoveTo(this.GStruct1_1);
                                    }
                                }
                                this.uint_4 = 0u;
                            }
                            if (flag)
                            {
                                if (this.ExtraData != "1")
                                {
                                    this.ExtraData = "1";
                                    this.UpdateState(false, true);
                                }
                            }
                            else
                            {
                                if (flag2)
                                {
                                    if (this.ExtraData != "2")
                                    {
                                        this.ExtraData = "2";
                                        this.UpdateState(false, true);
                                    }
                                }
                                else
                                {
                                    if (this.ExtraData != "0")
                                    {
                                        this.ExtraData = "0";
                                        this.UpdateState(false, true);
                                    }
                                }
                            }
                            this.ReqUpdate(1);
                            break;
                        }
                    case "bottle":
                        {
                            int num = Essential.smethod_5(0, 7);
                            this.ExtraData = num.ToString();
                            this.UpdateState();
                            break;
                        }
                    case "dice":
                        try
                        {
                            RoomUser @class = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
                            if (@class.GetClient().GetHabbo().int_1 > 0)
                            {
                                this.ExtraData = @class.GetClient().GetHabbo().int_1.ToString();
                                @class.GetClient().GetHabbo().int_1 = 0;
                            }
                            else
                            {
                                int num = Essential.smethod_5(1, 6);
                                this.ExtraData = num.ToString();
                            }
                        }
                        catch
                        {
                            int num = Essential.smethod_5(1, 6);
                            this.ExtraData = num.ToString();
                        }
                        this.UpdateState();
                        break;
                    case "habbowheel":
                        {
                            int num = Essential.smethod_5(1, 10);
                            this.ExtraData = num.ToString();
                            this.UpdateState();
                            break;
                        }
                    case "loveshuffler":
                        if (this.ExtraData == "0")
                        {
                            int num = Essential.smethod_5(1, 4);
                            this.ExtraData = num.ToString();
                            this.ReqUpdate(20);
                        }
                        else
                        {
                            if (this.ExtraData != "-1")
                            {
                                this.ExtraData = "-1";
                            }
                        }
                        this.UpdateState(false, true);
                        break;
                    case "alert":
                        if (this.ExtraData == "1")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        break;
                    case "vendingmachine":
                        if (this.ExtraData == "1")
                        {
                            RoomUser @class = this.GetRoom().GetRoomUserByHabbo(this.InteractingUser);
                            if (@class != null)
                            {
                                @class.method_6();
                                int int_ = this.GetBaseItem().VendingIds[Essential.smethod_5(0, this.GetBaseItem().VendingIds.Count - 1)];
                                @class.CarryItem(int_);
                            }

                            this.InteractingUser = 0u;
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        break;

                    case "wf_trg_onsay":
                    case "wf_trg_enterroom":
                    case "wf_trg_furnistate":
                    case "wf_trg_onfurni":
                    case "wf_trg_offfurni":
                    case "wf_trg_gameend":
                    case "wf_trg_gamestart":
                    case "wf_trg_atscore":
                    case "wf_act_saymsg":
                    case "wf_act_togglefurni":
                    case "wf_act_givepoints":
                    case "wf_act_moverotate":
                    case "wf_act_matchfurni":
                    case "wf_act_give_phx":
                    case "wf_cnd_trggrer_on_frn":
                    case "wf_cnd_furnis_hv_avtrs":
                    case "wf_cnd_has_furni_on":
                    case "wf_cnd_match_snapshot":
                    case "wf_cnd_not_hv_avtrs":
                    case "wf_cnd_not_match_snap":
                    case "wf_cnd_phx":
                    case "bb_teleport":
                    case "wf_act_bot_follow_avatar":
                    case "wf_act_bot_give_handitem":
                    case "wf_act_bot_talk":
                    case "wf_act_bot_talk_to_avatar":
                    case "wf_act_bot_clothes":
                    case "wf_act_yt":
                    case "wf_act_img":
                    case "wf_cnd_has_purse":
                    case "wf_cnd_hasnot_purse":
                        if (this.ExtraData == "1")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        break;
                    case "wf_trg_timer":
                        if (this.ExtraData == "1")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        if (this.string_2.Length > 0)
                        {
                            this.GetRoom().method_15(this);
                            this.ReqUpdate(Convert.ToInt32(double.Parse(this.string_2, CustomCultureInfo.GetCustomCultureInfo()) * 2.0));
                        }
                        break;
                    case "wf_act_moveuser":
                        if (this.dictionary_1.Count > 0 && this.GetRoom().RoomUsers != null)
                        {
                            int num4 = 0;
                            RoomUser[] RoomUser_ = this.GetRoom().RoomUsers;
                            for (int i = 0; i < RoomUser_.Length; i++)
                            {
                                RoomUser class4 = RoomUser_[i];
                                if (class4 != null)
                                {
                                    if (class4.IsBot)
                                    {
                                        this.dictionary_1.Remove(class4);
                                    }
                                    if (this.dictionary_1.ContainsKey(class4))
                                    {
                                        if (this.dictionary_1[class4] > 0)
                                        {
                                            int oldeffect = 0;
                                            if (this.dictionary_1[class4] == 10 && !class4.IsBot && class4 != null && class4.GetClient() != null && class4.GetClient().GetHabbo() != null)
                                            {
                                                oldeffect = class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0;
                                                class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(4, true);
                                            }
                                            Dictionary<RoomUser, int> dictionary;
                                            RoomUser key;
                                            (dictionary = this.dictionary_1)[key = class4] = dictionary[key] - 1;
                                            num4++;
                                        }
                                        else
                                        {
                                            this.dictionary_1.Remove(class4);
                                            class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(class4.GetClient().GetHabbo().GetEffectsInventoryComponent().oldeffect, true);

                                            if (class4.team != Team.None && class4.game == Rooms.Games.Game.Freeze)
                                            {
                                                int FreezeEffect = ((int)class4.team) + 39;
                                                if (class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 != FreezeEffect)
                                                {
                                                    class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(FreezeEffect, true);
                                                }
                                            }

                                            else if (class4.team != Team.None && class4.game == Rooms.Games.Game.BattleBanzai)
                                            {
                                                int FreezeEffect = ((int)class4.team) + 32;
                                                if (class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 != FreezeEffect)
                                                {
                                                    class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(FreezeEffect, true);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (num4 > 0)
                            {
                                this.ReqUpdate(0);
                            }
                            else
                            {
                                this.dictionary_1.Clear();
                            }
                        }
                        break;
                    case "counter":
                        if (this.bool_0 && this.string_2 != "1")
                        {
                            this.ExtraData = Convert.ToString(Convert.ToInt32(this.ExtraData) - 1);
                            if (Convert.ToInt32(this.ExtraData) <= 0)
                            {
                                this.ExtraData = "0";
                                this.bool_0 = false;
                                this.GetRoom().method_89(0, this, true, null);

                                foreach (RoomItem Item in this.GetRoom().Hashtable_0.Values)
                                {
                                    if (Item.GetBaseItem().Name == "bb_apparatus")
                                    {
                                        Item.ExtraData = "0";
                                        Item.UpdateState(false, true);
                                        Item.ReqUpdate(1);
                                    }
                                }
                            }
                            this.UpdateState(true, true);
                            this.string_2 = "1";
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            if (this.bool_0)
                            {
                                this.string_2 = "0";
                                this.ReqUpdate(1);
                            }
                        }
                        break;
                    case "freeze_counter":
                        if (this.GetRoom().frzTimer && this.string_2 != "1")
                        {
                            this.ExtraData = Convert.ToString(Convert.ToInt32(this.ExtraData) - 1);
                            if (Convert.ToInt32(this.ExtraData) <= 0)
                            {
                                this.ExtraData = "0";
                                this.GetRoom().frzTimer = false;
                                this.GetRoom().GetFreeze().StopGame();
                            }
                            this.UpdateState(true, true);
                            this.string_2 = "1";
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            if (this.GetRoom().frzTimer)
                            {
                                this.string_2 = "0";
                                this.ReqUpdate(1);
                            }
                        }
                        break;
                    case "wf_trg_attime":
                        if (!this.WiredAtTimeNeedReset)
                        {
                            if (this.WiredAtTimeTimer < 0)
                            {
                                this.WiredAtTimeTimer = 0;
                            }
                            this.WiredAtTimeTimer += 0.5;
                            this.GetRoom().method_16(this);
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            this.WiredAtTimeNeedReset = true;
                        }
                        break;
                    case "wf_cnd_time_more_than":
                        if (!this.WiredNeedReset)
                        {
                            if (this.WiredCounter >= double.Parse(this.string_2, CultureInfo.InvariantCulture))
                            {
                                this.WiredNeedReset = true;
                            }
                            this.WiredCounter += 0.5;
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            this.WiredNeedReset = true;
                        }
                        break;
                    case "wf_cnd_time_less_than":
                        if (!this.WiredNeedReset)
                        {
                            if (this.WiredCounter >= double.Parse(this.string_2, CultureInfo.InvariantCulture))
                            {
                                this.WiredNeedReset = true;
                            }
                            this.WiredCounter += 0.5;
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            this.WiredNeedReset = true;
                        }
                        break;
                }
            }
        }
        internal void ReqUpdate(int int_5)
        {
            this.int_4 = int_5;
            this.bool_1 = true;
        }
        internal void UpdateState()
        {
            this.UpdateState(true, true);
        }
        internal void UpdateState(bool bool_5, bool bool_6, bool adsBackground = false)
        {
            if (bool_5)
            {
                this.GetRoom().method_80(this);
            }
            /*if (adsBackground)
            {
                ServerMessage Message = new ServerMessage();
                Message.Init(Outgoing.ObjectDataUpdate); // Update
                Message.AppendStringWithBreak(this.uint_0.ToString());
                Message.AppendInt32(0);
                Message.AppendInt32(1);
                Message.AppendStringWithBreak(this.ExtraData);
            }
                */
            if (bool_6)
            {
                ServerMessage Message = new ServerMessage();
                if (this.Boolean_2)
                {
                    Message.Init(Outgoing.ObjectDataUpdate); // Update
                    Message.AppendStringWithBreak(this.uint_0.ToString());
                    if (this.GetBaseItem().Name == "ads_mpu_720" || this.GetBaseItem().Name == "ads_background" || this.GetBaseItem().Name == "ads_mpu_300" || this.GetBaseItem().Name == "ads_mpu_160")
                    {
                        Message.AppendInt32(0);
                        Message.AppendInt32(1);
                        if (ExtraData != "")
                        {
                            Message.AppendInt32(ExtraData.Split(Convert.ToChar(9)).Length / 2);

                            for (int i = 0; i <= ExtraData.Split(Convert.ToChar(9)).Length - 1; i++)
                            {
                                Message.AppendString(ExtraData.Split(Convert.ToChar(9))[i]);
                            }
                        }
                        else
                        {
                            Message.AppendInt32(0);
                        }

                    }
                    else if (this.GetBaseItem().InteractionType == "mannequin")
                    {
                        if (this.ExtraData.Contains((char)5))
                        {
                            Message.AppendInt32(1);
                            Message.AppendInt32(3);
                            Message.AppendString("GENDER");
                            Message.AppendString(this.ExtraData.Split((char)5)[1]);
                            Message.AppendString("FIGURE");
                            Message.AppendString(this.ExtraData.Split((char)5)[0]);
                            Message.AppendString("OUTFIT_NAME");
                            Message.AppendString(this.ExtraData.Split((char)5)[2]);
                        }
                        else
                        {
                            Message.AppendInt32(1);
                            Message.AppendInt32(3);
                            Message.AppendString("GENDER");
                            Message.AppendString("M");
                            Message.AppendString("FIGURE");
                            Message.AppendString("ch-3015-1430.lg-275-110.hd-209-1379.hr-3163-45");
                            Message.AppendString("OUTFIT_NAME");
                            Message.AppendString("Rootkit");
                        }
                    }
                    else if (this.GetBaseItem().InteractionType == "badgedisplay")
                    {
                        Message.AppendInt32(0);
                        Message.AppendInt32(2);
                        Message.AppendInt32(4);
                        Message.AppendString("0");
                        Message.AppendString(this.ExtraData);
                        Message.AppendString("");
                        Message.AppendString("");
                    }
                    else
                    {
                        Message.AppendInt32(0);

                        Message.AppendStringWithBreak(this.ExtraData);
                    }

                }
                else
                {
                    Message.Init(Outgoing.UpdateWallItemOnRoom); // Update
                    this.Serialize(Message);
                }
                this.GetRoom().SendMessage(Message, null);
            }
        }
        internal void Serialize(ServerMessage Message5_0)
        {
            if (this.Boolean_2)
            {
                Message5_0.AppendUInt(this.uint_0);
                Message5_0.AppendInt32(this.GetBaseItem().Sprite);
                Message5_0.AppendInt32(this.mX);
                Message5_0.AppendInt32(this.mY);
                Message5_0.AppendInt32(this.int_3);
                Message5_0.AppendStringWithBreak(this.double_0.ToString().Replace(',', '.'));
                if (this.GetBaseItem().Name == "song_disk" && this.ExtraData.Length > 0)
                {
                    Message5_0.AppendInt32(712);
                    Message5_0.AppendInt32(0);
                    Message5_0.AppendStringWithBreak("");
                }
                else
                {
                    if (this.GetBaseItem().Name == "ads_mpu_720" || this.GetBaseItem().Name == "ads_background" || this.GetBaseItem().Name == "ads_mpu_300" || this.GetBaseItem().Name == "ads_mpu_160")
                    {
                        Message5_0.AppendInt32(0);
                        Message5_0.AppendInt32(1);
                        if (ExtraData != "" && ExtraData.Contains(Convert.ToChar(9)))
                        {
                            ExtraData = ExtraData.Replace("https://", "http://");
                            Message5_0.AppendInt32(ExtraData.Split(Convert.ToChar(9)).Length / 2);
                            for (int i = 0; i <= ExtraData.Split(Convert.ToChar(9)).Length - 1; i++)
                            {
                                Message5_0.AppendString(ExtraData.Split(Convert.ToChar(9))[i]);
                            }
                        }
                        else
                        {
                            Message5_0.AppendInt32(0);
                        }
                    }
                    else if (this.GetBaseItem().Name.Contains("present_gen"))
                    {
                        if (this.ExtraData.Contains((char)5))
                        {
                            bool ShowGiver = (this.ExtraData.Split((char)5)[0] == "true");
                            string GiftMessage = this.ExtraData.Split((char)5)[1];
                            int PurchaserId = 0;
                            string Buyer = "Tuntematon";
                            string BuyerLook = "hd-180";
                            Message5_0.AppendInt32(0);
                            Message5_0.AppendInt32(1);
                            Message5_0.AppendInt32((ShowGiver) ? 6 : 4);
                            Message5_0.AppendString("EXTRA_PARAM");
                            Message5_0.AppendString("");
                            Message5_0.AppendString("MESSAGE");
                            Message5_0.AppendString(GiftMessage);
                            if (ShowGiver)
                            {
                                PurchaserId = int.Parse(this.ExtraData.Split((char)5)[2]);
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Buyer = dbClient.ReadString("SELECT username FROM users WHERE id = '" + PurchaserId + "' LIMIT 1");
                                    BuyerLook = dbClient.ReadString("SELECT look FROM users WHERE id = '" + PurchaserId + "' LIMIT 1");
                                }
                                Message5_0.AppendString("PURCHASER_NAME");
                                Message5_0.AppendString(Buyer);
                                Message5_0.AppendString("PURCHASER_FIGURE");
                                Message5_0.AppendString(BuyerLook);
                            }
                            Message5_0.AppendString("PRODUCT_CODE");
                            Message5_0.AppendString("normal_item");
                            Message5_0.AppendString("state");
                            Message5_0.AppendString("0");
                        }
                        else
                        {
                            Message5_0.AppendInt32(0);
                            Message5_0.AppendInt32(1);
                            Message5_0.AppendInt32(4);
                            Message5_0.AppendString("EXTRA_PARAM");
                            Message5_0.AppendString("");
                            Message5_0.AppendString("MESSAGE");
                            Message5_0.AppendString("");
                            Message5_0.AppendString("PRODUCT_CODE");
                            Message5_0.AppendString("no_item");
                            Message5_0.AppendString("state");
                            Message5_0.AppendString("0");
                        }
                    }
                    else if (this.GetBaseItem().InteractionType == "mannequin")
                    {
                        if (this.ExtraData.Contains((char)5))
                        {
                            Message5_0.AppendInt32(0);
                            Message5_0.AppendInt32(1);
                            Message5_0.AppendInt32(3);
                            Message5_0.AppendString("GENDER");
                            Message5_0.AppendString(this.ExtraData.Split((char)5)[1]);
                            Message5_0.AppendString("FIGURE");
                            Message5_0.AppendString(this.ExtraData.Split((char)5)[0]);
                            Message5_0.AppendString("OUTFIT_NAME");
                            Message5_0.AppendString(this.ExtraData.Split((char)5)[2]);
                        }
                        else
                        {
                            Message5_0.AppendInt32(0);
                            Message5_0.AppendInt32(1);
                            Message5_0.AppendInt32(3);
                            Message5_0.AppendString("GENDER");
                            Message5_0.AppendString("m");
                            Message5_0.AppendString("FIGURE");
                            Message5_0.AppendString("ch-3015-1430.lg-275-110.hd-209-1379.hr-3163-45");
                            Message5_0.AppendString("OUTFIT_NAME");
                            Message5_0.AppendString("Rootkit");
                        }

                    }
                    else if (this.GetBaseItem().InteractionType == "badge_display")
                    {
                        /*Message5_0.AppendInt32(0);
                        Message5_0.AppendInt32(1);
                        Message5_0.AppendInt32(1);
                        Message5_0.AppendString("imageUrl");
                        try
                        {
                            ExtraData = ExtraData == "" ? "null" : ExtraData;
                        }
                        catch { }
                        Message5_0.AppendString("https://swf.habbo.tl/c_images/album1584/" + ExtraData + ".gif");//ExtraData);
                        */
                        Message5_0.AppendInt32(0);
                        Message5_0.AppendInt32(2);
                        Message5_0.AppendInt32(3);
                        Message5_0.AppendString("0");
                        Message5_0.AppendString("1");
                        try
                        {
                            ExtraData = ExtraData == "" ? "BUG" : ExtraData;
                        }
                        catch { }
                        Message5_0.AppendString("userbadge/" + ExtraData);
                       // Message5_0.AppendString("ffffff");
                       // Message5_0.AppendString("ffffff");
                    }
                    else
                    {
                        Message5_0.AppendInt32(0);


                        if (this.LimitedId > 0 && !this.GetBaseItem().Name.Contains("present_gen"))
                        {
                            Message5_0.AppendString("");
                            Message5_0.AppendBoolean(true);
                            Message5_0.AppendBoolean(false);
                        }


                        else if (this.GuildData != "")
                        {
                            GroupsManager Guild = Groups.GetGroupById(int.Parse(this.GuildData));
                            if (Guild == null)
                            {
                                Message5_0.AppendInt32(2);
                                Message5_0.AppendInt32(5);
                                Message5_0.AppendString(this.ExtraData);
                                Message5_0.AppendString("1");
                                Message5_0.AppendString("");
                                Message5_0.AppendString("ffffff");
                                Message5_0.AppendString("ffffff");
                            }
                            else
                            {
                                Message5_0.AppendInt32(2);
                                Message5_0.AppendInt32(5);
                                if (this.GetBaseItem().Name == "gld_gate")
                                {
                                    Message5_0.AppendString("0");
                                }
                                else
                                {
                                    Message5_0.AppendString(this.ExtraData);
                                }

                                Message5_0.AppendString(Guild.Id.ToString());
                                Message5_0.AppendString(Guild.Badge);
                                Message5_0.AppendString(Guild.ColorOne);
                                Message5_0.AppendString(Guild.ColorTwo);
                            }
                        }
                        else
                        {
                            Message5_0.AppendInt32(0);
                        }


                        if (this.GuildData == "")
                        {
                            Message5_0.AppendStringWithBreak(this.ExtraData);
                        }
                    }

                }

                if (this.LimitedId > 0 && !this.GetBaseItem().Name.Contains("present_gen"))
                {
                    Message5_0.AppendInt32(this.LimitedId);
                    Message5_0.AppendInt32(this.LimitetCnt);
                }

                Message5_0.AppendInt32(-1);
                Message5_0.AppendInt32((this.GetBaseItem().InteractionType != "default" || this.GetBaseItem().Modes > 1 ? 1 : 0));
                Message5_0.AppendInt32((class14_0.RoomData.HideOwner ? 0 : class14_0.OwnerId));
                /* using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\filut\testit.txt", true))
                 {
                     file.WriteLine(Message5_0.ToString());
                 }
                 */
            }
            else
            {
                if (this.Boolean_1)
                {
                    Message5_0.AppendStringWithBreak(string.Concat(this.uint_0));
                    Message5_0.AppendInt32(this.GetBaseItem().Sprite);
                    Message5_0.AppendStringWithBreak(this.string_7);
                    /*if (this.GetBaseItem().Name.StartsWith("poster_"))
                    {
                        Message5_0.AppendString(this.GetBaseItem().Name.Split(new char[]
                        {
                            '_'
                        })[1]);
                    }*/
                    string text = this.GetBaseItem().InteractionType.ToLower();
                    if (text != null && text == "postit")
                    {
                        Message5_0.AppendStringWithBreak(this.ExtraData.Split(new char[]
						{
							' '
						})[0]);
                    }
                    else
                    {
                        Message5_0.AppendStringWithBreak(this.ExtraData);
                    }
                    Message5_0.AppendInt32(-1);
                    Message5_0.AppendInt32(-1);
                    Message5_0.AppendInt32(class14_0.OwnerId);
                }
            }
        }
        internal Item GetBaseItem()
        {
            if (this.Item == null)
            {
                this.Item = Essential.GetGame().GetItemManager().GetItemById(this.uint_2);
            }
            return this.Item;
        }
        internal Room GetRoom()
        {
            if (this.class14_0 == null)
            {
                this.class14_0 = Essential.GetGame().GetRoomManager().GetRoom(this.uint_1);
            }
            return this.class14_0;
        }
        internal void CheckExtraData4()
        {
            if (!(this.string_4 == ""))
            {
                string[] collection = this.string_4.Split(new char[]
				{
					','
				});
                IEnumerable<string> enumerable = new List<string>(collection);
                List<string> list = enumerable.ToList<string>();
                bool flag = false;
                if (list.Count > 100000)
                {
                    this.string_4 = "";
                    this.string_5 = "";
                }
                else
                {
                    foreach (string current in enumerable)
                    {
                        RoomItem @class = null;
                        if (current.Length > 0)
                        {
                            @class = this.GetRoom().method_28(Convert.ToUInt32(current));
                        }
                        if (@class == null)
                        {
                            list.Remove(current);
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        this.string_5 = OldEncoding.encodeVL64(list.Count);
                        for (int i = 0; i < list.Count; i++)
                        {
                            int value = Convert.ToInt32(list[i]);
                            this.string_5 += OldEncoding.encodeVL64(value);
                            this.string_4 = this.string_4 + "," + Convert.ToString(value);
                        }
                        this.string_4 = this.string_4.Substring(1);
                    }
                }
            }
        }
        internal void CheckExtraData3()
        {
            if (!(this.string_3 == ""))
            {
                string[] collection = this.string_3.Split(new char[]
				{
					','
				});
                IEnumerable<string> enumerable = new List<string>(collection);
                List<string> list = enumerable.ToList<string>();
                bool flag = false;
                foreach (string current in enumerable)
                {
                    RoomItem @class = this.GetRoom().method_28(Convert.ToUInt32(current));
                    if (@class == null)
                    {
                        list.Remove(current);
                        flag = true;
                    }
                }
                if (flag)
                {
                    this.string_2 = OldEncoding.encodeVL64(list.Count);
                    for (int i = 0; i < list.Count; i++)
                    {
                        int num = Convert.ToInt32(list[i]);
                        this.string_2 += OldEncoding.encodeVL64(num);
                    }
                    this.string_3 = string.Join(",", list.ToArray());
                }
            }
        }
    }
}
