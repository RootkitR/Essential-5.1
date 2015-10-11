using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
using System.Data;
using System.Threading;
using System.Collections.Generic;
using Essential.HabboHotel.Users.Inventory;
namespace Essential.HabboHotel.RoomBots
{
    internal sealed class UserBotInt : BotAI
    {
        private int int_2;
        private int int_3;
        private Thread speechthread;
        private List<BotResponse> botResponse;
        private List<RandomSpeech> randomSpeeches;
        private UserBot uBot;
        private int speechInterval;
        private bool canSpeak;
        private int ActionTimer;
        private int SpeechTimer;
        private int speechDelta;
        private uint virtualId;
        public UserBotInt(uint virtualId)
        {
            this.int_2 = 0;
            this.int_3 = 0;
            this.virtualId = virtualId;
        }
        public override void OnSelfEnterRoom()
        {
            try
            {
                this.botResponse = new List<BotResponse>();
                this.randomSpeeches = new List<RandomSpeech>();
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {

                    this.uBot = Essential.GetGame().GetCatalog().RetrBot(dbClient.ReadDataRow("SELECT * FROM user_bots WHERE id=" + (base.GetRoomUser().UId - 1000)));
                    if (this.uBot.BotType != "default")
                    {
                        foreach (DataRow drow in dbClient.ReadDataTable("SELECT * FROM bots_responses WHERE bot_type='" + this.uBot.BotType + "'").Rows)
                        {
                            botResponse.Add(new BotResponse((uint)drow["id"], 0, drow["keywords"].ToString(), drow["response_text"].ToString(), drow["mode"].ToString(), (int)drow["serve_id"]));
                        }
                    }
                    else
                    {
                        foreach (DataRow drow in dbClient.ReadDataTable("SELECT * FROM bots_responses WHERE bot_id='" + this.uBot.BotId + "'").Rows)
                        {
                            botResponse.Add(new BotResponse((uint)drow["id"], 0, drow["keywords"].ToString(), drow["response_text"].ToString(), drow["mode"].ToString(), (int)drow["serve_id"]));
                        }
                    }
                    DataTable dtable = dbClient.ReadDataTable("SELECT * FROM bots_speech WHERE bot_id='" + this.uBot.BotId + "'");
                    foreach (DataRow drow in dtable.Rows)
                    {
                        randomSpeeches.Add(new RandomSpeech((string)drow["text"], Essential.StringToBoolean(drow["shout"].ToString()), Convert.ToUInt32(drow["bot_id"])));
                    }
                    speechInterval = dbClient.ReadInt32("SELECT speaking_interval from user_bots WHERE id = " + this.uBot.BotId);
                    canSpeak = Convert.ToBoolean(dbClient.ReadString("SELECT automatic_chat from user_bots WHERE id = " + this.uBot.BotId));
                }
                this.SpeechTimer = speechInterval * 2;
                this.speechDelta = speechInterval * 2;
                this.ActionTimer = new Random(((int)this.virtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 30);
            }catch
            {
            }
        }
        public override void OnSelfLeaveRoom(bool bool_0)
        {
        }
        public override void OnUserEnterRoom(RoomUser RoomUser_0)
        {
            speechthread = new Thread(delegate()
            {
                try
                {
                    if (RoomUser_0 != null && RoomUser_0.GetClient() != null && this.uBot.BotType == "friendbot")
                    {
                        Thread.Sleep(1000);
                        base.GetRoomUser().HandleSpeech(null, "Hallo " + RoomUser_0.GetClient().GetHabbo().Username + " wie gehts dir?", false);
                    }
                }
                catch { }
            });
            speechthread.Start();
        }
        public override void OnUserLeaveRoom(GameClient Session)
        {
            /*if (base.method_1().Owner.ToLower() == Session.GetHabbo().Username.ToLower())
            {
                base.method_1().method_6(base.GetRoomUser().VirtualId, false);
            }*/
        }
        public override void OnUserSay(RoomUser RoomUser_0, string string_0)
        {
            try
            {
                foreach (BotResponse br in botResponse)
                {
                    if (br.ContainsWord(string_0))
                    {
                        if (RoomUser_0 != null && RoomUser_0.GetClient() != null && br.ContainsWord(string_0) && this.uBot.BotType != "spybot" && this.uBot.BotType != "drinkbot")
                        {
                            if (RoomUser_0 != null && RoomUser_0.GetClient() != null && RoomUser_0.GetClient().GetHabbo() != null)
                                base.GetRoomUser().HandleSpeech(null, br.Response.Replace("%username%", RoomUser_0.GetClient().GetHabbo().Username), br.Shout);
                        }
                        else if (RoomUser_0 != null && RoomUser_0.GetClient() != null && br.ContainsWord(string_0) && this.uBot.BotType == "spybot" && RoomUser_0.GetClient().GetHabbo().Username == this.GetRoomUser().GetRoom().Owner)
                        {
                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                            {
                                string text = "";
                                string username;
                                foreach (DataRow dr in dbClient.ReadDataTable("SELECT user_id FROM user_roomvisits WHERE room_id=" + base.GetRoomUser().GetRoom().Id + " ORDER BY exit_timestamp DESC LIMIT 10").Rows)
                                {
                                    username = dbClient.ReadString("SELECT username FROM users WHERE id=" + dr["user_id"].ToString());
                                    if (text == "")
                                        text = username;
                                    else
                                        text = text + ", " + username;
                                }
                                base.GetRoomUser().HandleSpeech(null, "Die letzen 10 Habbos in deinem Raum waren:", false);
                                base.GetRoomUser().HandleSpeech(null, text, false);

                            }
                        }
                        else if (RoomUser_0 != null && RoomUser_0.GetClient() != null && br.ContainsWord(string_0) && this.uBot.BotType == "drinkbot")
                        {
                            if (br.ServeId > 0)
                            {
                                base.GetRoomUser().CarryItem(br.ServeId);
                                base.GetRoomUser().HandleSpeech(null, br.Response.Replace("%username%", RoomUser_0.GetClient().GetHabbo().Username), br.Shout);
                                base.GetRoomUser().CarryItem(0);
                                RoomUser_0.CarryItem(br.ServeId);
                            }
                        }
                    }
                }
            }
            catch { }
        }
        public override void OnUserShout(RoomUser RoomUser_0, string string_0)
        {
            try
            {
                foreach (BotResponse br in botResponse)
                {
                    if (RoomUser_0 != null && RoomUser_0.GetClient() != null && br.ContainsWord(string_0) && this.uBot.BotType != "spybot" && this.uBot.BotType != "drinkbot")
                    {
                        if (RoomUser_0 != null && RoomUser_0.GetClient() != null && RoomUser_0.GetClient().GetHabbo() != null)
                            base.GetRoomUser().HandleSpeech(null, br.Response.Replace("%username%", RoomUser_0.GetClient().GetHabbo().Username), br.Shout);

                    }
                    else if (RoomUser_0 != null && RoomUser_0.GetClient() != null && br.ContainsWord(string_0) && this.uBot.BotType == "spybot" && RoomUser_0.GetClient().GetHabbo().Username == this.GetRoomUser().GetRoom().Owner)
                    {
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            string text = "";
                            string username;
                            foreach (DataRow dr in dbClient.ReadDataTable("SELECT user_id FROM user_roomvisits WHERE room_id=" + base.GetRoomUser().GetRoom().Id + " ORDER BY exit_timestamp DESC LIMIT 10").Rows)
                            {
                                username = dbClient.ReadString("SELECT username FROM users WHERE id=" + dr["user_id"].ToString());
                                if (text == "")
                                    text = username;
                                else
                                    text = text + ", " + username;
                            }
                            base.GetRoomUser().HandleSpeech(null, "Die letzten 10 Habbos in deinem Raum waren:", false);
                            base.GetRoomUser().HandleSpeech(null, text, false);

                        }
                    }
                    else if (RoomUser_0 != null && RoomUser_0.GetClient() != null && br.ContainsWord(string_0) && this.uBot.BotType == "drinkbot")
                    {
                        if (br.ServeId != 0)
                        {
                            base.GetRoomUser().CarryItem(br.ServeId);
                            base.GetRoomUser().HandleSpeech(null, br.Response.Replace("%username%", RoomUser_0.GetClient().GetHabbo().Username), br.Shout);
                            base.GetRoomUser().CarryItem(0);
                            RoomUser_0.CarryItem(br.ServeId);
                        }
                    }
                }
            }
            catch { }
        }
        public RandomSpeech GetRandomSpeech()
        {
            return this.randomSpeeches[new Random().Next(0,this.randomSpeeches.Count)];
        }
        public override void OnTimerTick()
        {
            try
            {
                if (this.SpeechTimer <= 0)
                {
                    if (this.randomSpeeches.Count > 0)
                    {
                        if (canSpeak)
                        {
                            RandomSpeech randomSpeech = this.GetRandomSpeech();
                            base.GetRoomUser().HandleSpeech(null, randomSpeech.Message, randomSpeech.Shout, 0);
                        }
                    }
                    this.SpeechTimer = speechDelta;
                }
                else
                {
                    this.SpeechTimer--;
                }
                if (this.int_3 <= 0 && (this.uBot.walkmode == "freeroam" || this.uBot.walkmode == "specified_range") && !base.FollowsUser)
                {
                    if (this.uBot.walkmode == "freeroam")
                    {
                        int int_ = Essential.smethod_5(0, base.GetRoom().RoomModel.int_4);
                        int int_2 = Essential.smethod_5(0, base.GetRoom().RoomModel.int_5);
                        base.GetRoomUser().MoveTo(int_, int_2);
                        this.int_3 = Essential.smethod_5(0, 30);
                    }
                    else
                    {
                        int int_ = Essential.smethod_5(this.uBot.minX, this.uBot.maxX);
                        int int_2 = Essential.smethod_5(this.uBot.minY, this.uBot.maxY);
                        base.GetRoomUser().MoveTo(int_, int_2);
                        this.int_3 = Essential.smethod_5(0, 30);
                    }
                }
                else
                {
                    this.int_3--;
                }
            }
            catch { }
        }
    }
}
