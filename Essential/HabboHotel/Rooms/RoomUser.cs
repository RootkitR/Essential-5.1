using System;
using System.Collections.Generic;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Users;
using Essential.HabboHotel.Pets;
using Essential.HabboHotel.Pathfinding;
using Essential.Util;
using Essential.Messages;
using Essential.HabboHotel.RoomBots;
using Essential.HabboHotel.Items;
using Essential.Storage;
using System.Text;
using Essential.HabboHotel.Rooms.Games;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
namespace Essential.HabboHotel.Rooms
{
    internal sealed class RoomUser
    {
        public uint UId;
        public int VirtualId;

        public uint RoomId;

        public int int_1;
        internal int int_2;
        public int X;
        public int Y;
        public double double_0;
        internal byte byte_0;

        public int CarryItemID;

        public int int_6;
        public int int_7;
        public int BodyRotation;
        public bool bool_0;
        public bool bool_1;

        public bool TeleportMode;

        public int int_9;
        public int int_10;
        public int int_11;
        public bool bool_3;
        public bool bool_4;
        public int int_12;
        public int int_13;
        public double double_1;

        public RoomBot RoomBot;
        public BotAI BotAI;
        public Pet PetData;

        internal byte byte_1;
        internal bool bool_5;

        public RoomUser RoomUser_0;
        public RoomItem RoomItem_0;
        public RoomBot class34_1;
        public bool bool_6;
        public bool UpdateNeeded;
        public bool bool_8;
        public int int_14;
        public Dictionary<string, string> Statusses;
        internal Team team;
        internal Rooms.Games.Game game;
        public int DanceId;
        public int int_16;
        public bool bool_10;
        public int int_17;
        public int int_18;
        public int int_19;
        internal bool bool_11;
        internal bool bool_12;
        internal string string_0;
        internal int int_20;
        internal FreezePowerUp freezePowerUp;
        internal bool Freezed;
        internal int FreezeLives;
        internal bool shieldActive;
        internal int shieldCounter;
        internal int FreezeRange;
        internal int FreezeCounter;
        internal int FreezeBalls;
        internal bool isFlying;
        internal int flyk;

        internal bool onPrivateTile;
        internal uint privateTileID = 0;
        internal uint TentID = 0;
        internal uint miauId;
        internal RoomUser followingUser;
        public ThreeDCoord Position
        {
            get
            {
                return new ThreeDCoord(this.X, this.Y);
            }
        }

        public bool IsPet
        {
            get
            {
                return this.IsBot && this.RoomBot.Boolean_0;
            }
        }

        internal bool IsDancing
        {
            get
            {
                return this.DanceId >= 1;
            }
        }

        internal bool Boolean_2
        {
            get
            {
                return !this.IsBot && this.int_1 >= ServerConfiguration.KickTimer;
            }
        }

        internal bool Boolean_3
        {
            get
            {
                return !this.IsBot && this.Statusses.ContainsKey("trd");
            }
        }

        internal bool IsBot
        {
            get
            {
                return this.RoomBot != null;
            }
        }
        public RoomUser(uint UserId, uint RoomId, int VirtualId, bool Invisible, uint botid = 0)
        {
            this.bool_5 = false;
            this.UId = UserId;
            this.RoomId = RoomId;
            this.VirtualId = VirtualId;
            this.int_1 = 0;
            this.X = 0;
            this.Y = 0;
            this.double_0 = 0.0;
            this.int_7 = 0;
            this.BodyRotation = 0;
            this.UpdateNeeded = true;
            this.Statusses = new Dictionary<string, string>();
            this.int_16 = 0;
            this.int_19 = -1;
            this.RoomUser_0 = null;
            this.bool_1 = false;
            this.bool_0 = true;
            this.bool_11 = false;
            this.byte_0 = 3;
            this.int_2 = 0;
            this.int_20 = 0;
            this.byte_1 = 0;
            this.bool_12 = Invisible;
            this.string_0 = "";
            this.isFlying = false;
            this.flyk = 1;
            this.miauId = botid;
        }

        public void Unidle()
        {
            this.int_1 = 0;
            if (this.bool_8)
            {
                this.bool_8 = false;
                ServerMessage Message = new ServerMessage(Outgoing.IdleStatus); // P
                Message.AppendInt32(this.VirtualId);
                Message.AppendBoolean(false);
                this.GetRoom().SendMessage(Message, null);
            }
        }
        internal void HandleSpeech(GameClient Session, string str, bool bool_13, int TextColor = 0)
        {
            if (TextColor == 23 && Session.GetHabbo().Rank < 4)
                TextColor = 0;
            if (TextColor > 23 || TextColor < 0 || TextColor == 1 || TextColor == 8 || TextColor == 2)
                TextColor = 0;
            if (!String.IsNullOrEmpty(str) || !String.IsNullOrWhiteSpace(str))
            {
                string object_ = str;

                // string linkRegex = @"((http|https):\/\/|www.)?[a-zA-Z0-9\-\.]+\b(com|co\.uk|org|net|eu|cf|info|ml|nl|ca|es|fi)\b";

                if (Session == null || (Session.GetHabbo().HasFuse("ignore_roommute") || !this.GetRoom().bool_4))
                {
                    this.Unidle();

                    if (!this.IsBot && this.GetClient().GetHabbo().IsMuted)
                    {
                        this.GetClient().SendNotification(EssentialEnvironment.GetExternalText("error_muted"));
                    }
                    else
                    {
                        if (!str.StartsWith(":") || Session == null || !ChatCommandHandler.HandleCommands(Session, str.Substring(1)))
                        {
                            uint num = Outgoing.Talk; // Updated
                            if (bool_13)
                            {
                                num = Outgoing.Shout; // Updated
                            }
                            if (!this.IsBot && Session.GetHabbo().method_4() > 0)
                            {
                                TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
                                if (timeSpan.Seconds > 4)
                                {
                                    Session.GetHabbo().int_23 = 0;
                                }
                                if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 5 && !this.IsBot)
                                {
                                    ServerMessage Message = new ServerMessage(Outgoing.FloodFilter);
                                    Message.AppendInt32(Session.GetHabbo().method_4());
                                    this.GetClient().SendMessage(Message);
                                    this.GetClient().GetHabbo().IsMuted = true;
                                    this.GetClient().GetHabbo().int_4 = Session.GetHabbo().method_4();
                                    return;
                                }
                                Session.GetHabbo().dateTime_0 = DateTime.Now;
                                Session.GetHabbo().int_23++;
                            }

                            if (!this.IsBot)
                                str = ChatCommandHandler.ApplyFilter(str);

                            if (!this.GetRoom().method_9(this, str))
                            {
                                ServerMessage Message2 = new ServerMessage(num);
                                Message2.AppendInt32(this.VirtualId);
                                string Site = "";
                                if (str.Contains("http://") || str.Contains("www."))
                                {
                                    string[] Split = str.Split(' ');

                                    foreach (string Msg in Split)
                                    {
                                        if (Msg.StartsWith("http://") || Msg.StartsWith("www."))
                                        {
                                            Site = Msg;
                                        }
                                    }

                                    //str = str.Replace(Site, "{0}");
                                }

                                Message2.AppendStringWithBreak(str);
                                Message2.AppendInt32(ParseEmoticon(str));
                                Message2.AppendInt32(this.IsBot && !this.IsPet ? 2 : TextColor);
                                Message2.AppendInt32(0);
                                Message2.AppendInt32(-1);
                                if (!Essential.GetAntiAd().ContainsIllegalWord(object_))
                                {
                                    if (!this.IsBot && this.GetClient() != null && this.GetClient().GetHabbo().PassedSafetyQuiz)
                                        this.GetRoom().method_58(Message2, Session.GetHabbo().list_2, Session.GetHabbo().Id);
                                    else
                                    {
                                        this.GetRoom().SendMessage(Message2, this.IsPet ? this.GetRoom().RoomUsers.Where(p => p != null && p.GetClient() != null && p.GetClient().GetHabbo() != null && p.GetClient().GetHabbo().MutePets).Select(o => o.GetClient().GetHabbo().Id).ToList() : this.IsBot ? this.GetRoom().RoomUsers.Where(p => p != null && p.GetClient() != null && p.GetClient().GetHabbo() != null && p.GetClient().GetHabbo().MuteBots).Select(o => o.GetClient().GetHabbo().Id).ToList() : null);
                                    }
                                }
                                else if (Essential.GetAntiAd().ContainsIllegalWord(object_) && this.GetClient() != null)
                                {
                                    ServerMessage Message3 = new ServerMessage(Outgoing.InstantChat);
                                    Message3.AppendUInt(0u);
                                    Message3.AppendString("[AWS] " + Session.GetHabbo().Username + ": " + object_);
                                    Message3.AppendString(Essential.GetUnixTimestamp() + string.Empty);
                                    Essential.GetGame().GetClientManager().SendToStaffs(Session, Message3);

                                    Session.SendNotification(Essential.GetGame().GetRoleManager().GetConfiguration().getData("antiad.alert"));
                                    return;
                                }
                            }
                            else
                            {
                                if (!this.IsBot)
                                    Session.GetHabbo().Whisper(str);
                            }

                            if (!this.IsBot)
                            {
                                this.GetRoom().method_7(this, str, bool_13);

                                if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "CHAT_WITH_SOMEONE")
                                    Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                            }

                            if (ServerConfiguration.EnableChatlog && !this.IsBot && !Essential.GetAntiAd().ContainsIllegalWord(object_) && this.GetRoom().Id != 74402)
                            {
                                using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                                {
                                    @class.AddParamWithValue("message", object_);
                                    @class.ExecuteQuery(string.Concat(new object[]
								{
									"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
									Session.GetHabbo().Id,
									"','",
									this.GetRoom().Id,
									"','",
									DateTime.Now.Hour,
									"','",
									DateTime.Now.Minute,
									"',UNIX_TIMESTAMP(),@message,'",
									Session.GetHabbo().Username,
									"','",
									DateTime.Now.ToLongDateString(),
									"')"
								}));
                                }
                            }
                        }
                    }
                }
            }
        }

        internal int ParseEmoticon(string string_1)
        {
            string_1 = string_1.ToLower();
            int result;
            if (string_1.Contains(":)") || string_1.Contains(":d") || string_1.Contains("=]") || string_1.Contains("=d") || string_1.Contains(":>"))
            {
                result = 1;
            }
            else
            {
                if (string_1.Contains(">:(") || string_1.Contains(":@"))
                {
                    result = 2;
                }
                else
                {
                    if (string_1.Contains(":o") || string_1.Contains(";o"))
                    {
                        result = 3;
                    }
                    else
                    {
                        if (string_1.Contains(":(") || string_1.Contains(";<") || string_1.Contains("=[") || string_1.Contains(":'(") || string_1.Contains("='["))
                        {
                            result = 4;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                }
            }
            return result;
        }

        internal void method_3(bool bool_13)
        {

            this.bool_6 = false;
            this.bool_10 = false;
            this.Statusses.Remove("mv");
            this.int_10 = 0;
            this.int_11 = 0;
            this.bool_4 = false;
            this.int_12 = 0;
            this.int_13 = 0;
            this.double_1 = 0.0;
            if (bool_13)
            {
                this.UpdateNeeded = true;
            }
        }

        internal void MoveTo(ThreeDCoord position)
        {
            this.MoveTo(position.x, position.y);
        }

        internal void MoveTo(int x, int y)
        {
            if (this.GetRoom().method_92(x, y) && !this.GetRoom().HasUserOnItem(x, y, this.double_0))
            {
                this.Unidle();
                this.bool_6 = true;
                this.bool_10 = true;
                this.int_17 = x;
                this.int_18 = y;
                if (this.followingUser != null)
                {
                    Action<object> action4 = delegate(object obj)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            this.followingUser.MoveTo(x, y);
                        }
                        catch { }
                    };
                    new Task(action4, "").Start();

                }
                if (x >= this.GetRoom().RoomModel.int_4 || y >= this.GetRoom().RoomModel.int_5)
                {
                    this.int_10 = x;
                    this.int_11 = y;
                }
                else
                {
                    this.int_10 = this.GetRoom().gstruct1_0[x, y].x;
                    this.int_11 = this.GetRoom().gstruct1_0[x, y].y;
                }
            }
        }

        internal void method_6()
        {
            this.bool_1 = false;
            this.bool_0 = true;
        }

        internal void method_7(int int_21, int int_22, double double_2)
        {
            this.X = int_21;
            this.Y = int_22;
            this.double_0 = double_2;
            if (this.isFlying)
            {
                this.double_0 += 1.0 + (0.5 * Math.Sin(0.7 * this.flyk));
            }
        }

        public void CarryItem(int int_21)
        {
            this.CarryItemID = int_21;
            if (int_21 > 1000)
            {
                this.int_6 = 5000;
            }
            else
            {
                if (int_21 > 0)
                {
                    this.int_6 = 240;
                }
                else
                {
                    this.int_6 = 0;
                }
            }
            ServerMessage Message = new ServerMessage(Outgoing.ApplyCarryItem); // P
            Message.AppendInt32(this.VirtualId);
            Message.AppendInt32(int_21);
            this.GetRoom().SendMessage(Message, null);
        }

        public void method_9(int int_21)
        {
            this.method_10(int_21, false);
        }
        public void method_10(int int_21, bool bool_13)
        {
            if (!this.Statusses.ContainsKey("lay") && !this.bool_6)
            {
                int num = this.BodyRotation - int_21;
                this.int_7 = this.BodyRotation;
                if (this.Statusses.ContainsKey("sit") || bool_13)
                {
                    if (this.BodyRotation == 2 || this.BodyRotation == 4)
                    {
                        if (num > 0)
                        {
                            this.int_7 = this.BodyRotation - 1;
                        }
                        else
                        {
                            if (num < 0)
                            {
                                this.int_7 = this.BodyRotation + 1;
                            }
                        }
                    }
                    else
                    {
                        if (this.BodyRotation == 0 || this.BodyRotation == 6)
                        {
                            if (num > 0)
                            {
                                this.int_7 = this.BodyRotation - 1;
                            }
                            else
                            {
                                if (num < 0)
                                {
                                    this.int_7 = this.BodyRotation + 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (num <= -2 || num >= 2)
                    {
                        this.int_7 = int_21;
                        this.BodyRotation = int_21;
                    }
                    else
                    {
                        this.int_7 = int_21;
                    }
                }
                this.UpdateNeeded = true;
            }
        }
        public void AddStatus(string string_1, string string_2)
        {
            this.Statusses[string_1] = string_2;
        }

        public void RemoveStatus(string string_1)
        {
            if (this.Statusses.ContainsKey(string_1))
            {
                this.Statusses.Remove(string_1);
            }
        }

        public void ClearStatuses()
        {
            this.Statusses = new Dictionary<string, string>();
        }

        public void method_14(ServerMessage Message)
        {
            if ((Message != null))
            {
                if (this.IsBot)
                {
                    Message.AppendInt32(this.BotAI.int_0);
                    Message.AppendString(this.RoomBot.Name);
                    Message.AppendString(this.RoomBot.Motto);
                    if (this.IsPet)
                    {
                        Message.AppendString(string.Concat(new object[] { this.PetData.Look.ToLower(), " 2 2 0 -1 3 0 -1" }));
                    }
                    else
                    {
                        Message.AppendString(this.RoomBot.Look);
                    }
                    Message.AppendInt32(this.VirtualId);
                    Message.AppendInt32(this.X);
                    Message.AppendInt32(this.Y);
                    Message.AppendString(this.double_0.ToString().Replace(',', '.'));
                    Message.AppendInt32(0);
                    if (this.IsPet)
                    {
                        Message.AppendInt32((this.RoomBot.AiType == AIType.Pet) ? 2 : 3);
                    }
                    else
                    {
                        Message.AppendInt32(4);
                    }
                    if (this.RoomBot.AiType == AIType.Pet)
                    {
                        Message.AppendInt32(this.PetData.Type);
                        Message.AppendInt32(this.PetData.OwnerId);
                        Message.AppendString(this.PetData.OwnerName);
                        Message.AppendInt32(1);
                        Message.AppendBoolean(false);
                        Message.AppendBoolean(false);
                        Message.AppendInt32(0);
                        Message.AppendInt32(0);
                        Message.AppendString("");
                    }
                    else
                    {
                        Message.AppendString("m");
                        Message.AppendInt32(this.GetRoom().OwnerId);
                        string Owner;
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            Owner = dbClient.ReadString("SELECT username FROM users WHERE id = '" + this.GetRoom().OwnerId + "'");
                        }
                        Message.AppendString(Owner);
                        Message.AppendInt32(4);
                        Message.AppendShort(1);
                        Message.AppendShort(2);
                        Message.AppendShort(5);
                        Message.AppendShort(4);
                    }
                }
                else if ((!this.IsBot && (this.GetClient() != null)) && (this.GetClient().GetHabbo() != null))
                {
                    Habbo habbo = this.GetClient().GetHabbo();
                    Message.AppendInt32(habbo.Id);
                    Message.AppendString(habbo.Username);
                    Message.AppendString(habbo.Motto);
                    Message.AppendString(habbo.Figure);
                    Message.AppendInt32(this.VirtualId);
                    Message.AppendInt32(this.X);
                    Message.AppendInt32(this.Y);
                    Message.AppendString(TextHandling.GetString(this.double_0));
                    Message.AppendInt32(this.BodyRotation);
                    Message.AppendInt32(1);
                    Message.AppendString(habbo.Gender.ToLower());
                    if (habbo.FavouriteGroup == 0)
                    {
                        Message.AppendInt32(-1);
                        Message.AppendInt32(-1);
                        Message.AppendInt32(0);
                    }
                    else
                    {
                        GroupsManager guild = Groups.GetGroupById(habbo.FavouriteGroup);
                        Message.AppendInt32(habbo.FavouriteGroup);
                        Message.AppendInt32(2);
                        Message.AppendString(guild.Name);
                        Message.AppendString("");
                    }
                    Message.AppendInt32(habbo.AchievementScore);
                }
            }
        }
        public void method_15(ServerMessage Message5_0)
        {
            if (!this.bool_11)
            {
                Message5_0.AppendInt32(this.VirtualId);
                Message5_0.AppendInt32(this.X);
                Message5_0.AppendInt32(this.Y);
                Message5_0.AppendString(TextHandling.GetString(this.double_0));
                Message5_0.AppendInt32(this.int_7);
                Message5_0.AppendInt32(this.BodyRotation);
                StringBuilder builder = new StringBuilder();
                builder.Append("/");
                foreach (KeyValuePair<string, string> pair in this.Statusses)
                {
                    builder.Append(pair.Key);
                    if (pair.Value != string.Empty)
                    {
                        builder.Append(" ");
                        builder.Append(pair.Value);
                    }
                    builder.Append("/");
                }
                builder.Append("/");
                Message5_0.AppendString(builder.ToString());
                if (this.Statusses.ContainsKey("sign"))
                    this.RemoveStatus("sign");
            }
        }

        public GameClient GetClient()
        {
            GameClient result;
            if (this.IsBot)
            {
                result = null;
            }
            else
            {
                result = null;
                if (this.UId > 0)
                {
                    result = Essential.GetGame().GetClientManager().GetClient(this.UId);
                }
            }
            return result;
        }

        internal Room GetRoom()
        {
            return Essential.GetGame().GetRoomManager().GetRoom(this.RoomId);
        }
        public void Kiss()
        {
            ServerMessage Message = new ServerMessage(Outgoing.Action); // Updated
            Message.AppendInt32(this.VirtualId);
            Message.AppendInt32(2);
            this.GetRoom().SendMessage(Message, null);
        }
        public void Wave()
        {
            ServerMessage Message = new ServerMessage(Outgoing.Action); // Updated
            Message.AppendInt32(this.VirtualId);
            Message.AppendInt32(1);
            this.GetRoom().SendMessage(Message, null);
        }
    }
}
