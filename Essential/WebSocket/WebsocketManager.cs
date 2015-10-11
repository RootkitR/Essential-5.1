using System;
using System.Collections.Generic;
using System.Net;
using Fleck;
using Essential.Storage;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Misc;
using Essential.Core;
namespace Essential.Websockets
{
    class WebSocketServerManager
    {
        private List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        public static Dictionary<string, IWebSocketConnection> socketbyName = new Dictionary<string, IWebSocketConnection>();
        public static Dictionary<IWebSocketConnection, string> namebySocket = new Dictionary<IWebSocketConnection, string>();
        private WebSocketServer server;
        public WebSocketServerManager()
        {
            this.allSockets = new List<IWebSocketConnection>();
            socketbyName = new Dictionary<string, IWebSocketConnection>();
            namebySocket = new Dictionary<IWebSocketConnection, string>();
        }
        public List<IWebSocketConnection> getAllSockets()
        {
            return Essential.getWebSocketManager().allSockets;
        }
        public IWebSocketConnection getWebSocketByName(string name)
        {
            if (socketbyName.ContainsKey(name))
                return socketbyName[name];

            return null;
        }
        public string GetNameByWebSocket(IWebSocketConnection socket)
        {
            if(namebySocket.ContainsKey(socket))
                return namebySocket[socket];
            return "";
        }
        public void SendMessageToEveryConnection(string Message)
        {
            foreach (IWebSocketConnection iwsc in allSockets)
            {
                try
                {
                    iwsc.Send(Message);
                }
                catch { }
            }
        }
        public void LogWebsocketException(LogLevel ll, string s, Exception ex)
        {
            switch(ll)
            {
                case LogLevel.Error:
                    {
                        //Logging.LogException(s + ex.ToString());
                        break;
                    }
            }
        }
        public void Dispose()
        {
            foreach(IWebSocketConnection iwsc in allSockets)
            {
                try { iwsc.Close(); }
                catch { }
            }
            server.Dispose();
        }
        public WebSocketServerManager(string SocketURL)
        {
            FleckLog.Level = LogLevel.Error;
            FleckLog.LogAction = LogWebsocketException;
            allSockets = new List<IWebSocketConnection>();
            socketbyName = new Dictionary<string, IWebSocketConnection>();
            namebySocket = new Dictionary<IWebSocketConnection, string>();
            server = new WebSocketServer(SocketURL);
            if(SocketURL.StartsWith("wss://"))
                server.Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2("client.habbo.tl.cert.cer");
            server.Start(socket =>
            {
            socket.OnOpen = () =>
            {
                if (allSockets.Contains(socket))
                    allSockets.Remove(socket);
                allSockets.Add(socket);
            };
            socket.OnClose = () =>
            {
                string name = "";
                if (namebySocket.ContainsKey(socket))
                {
                    name = namebySocket[socket].ToString();
                    namebySocket.Remove(socket);
                }
                if (socketbyName.ContainsKey(name) && name != "")
                    socketbyName.Remove(name);
                if (allSockets.Contains(socket))
                    allSockets.Remove(socket);
                if (name != "")
                {
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.AddParamWithValue("name", name);
                        dbClient.ExecuteQuery("UPDATE users SET websocket='0' WHERE username=@name");
                    }
                }
            };
            socket.OnMessage = message =>
            {
            var msg = message;
            int pId = 0;
            if (!int.TryParse(msg.Split('|')[0], out pId))
                return;
            if (msg.Length > 1024)
                return;
            if (msg.StartsWith("1|"))
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    dbClient.AddParamWithValue("auth", msg.Substring(2));
                    DataRow drow = null;
                    drow = dbClient.ReadDataRow("SELECT username FROM users WHERE auth_ticket= @auth LIMIT 1");
                    if (drow == null)
                    {
                        socket.Close();

                    }
                    else
                    {
                            if (socketbyName.ContainsKey((string)drow["username"]))
                                socketbyName.Remove((string)drow["username"]);
                            socketbyName.Add(drow["username"].ToString(), socket);
                            if (namebySocket.ContainsKey(socket))
                                namebySocket.Remove(socket);
                            namebySocket.Add(socket, drow["username"].ToString());
                        dbClient.AddParamWithValue("name", drow["username"].ToString());
                        dbClient.ExecuteQuery("UPDATE users SET websocket='1' WHERE username=@name");
                    }
                }
            }
            else
            {
                GameClient Session = Essential.GetGame().GetClientManager().GetClientByHabbo(GetNameByWebSocket(socket));
                Room room = null;
                string[] args = msg.Split('|');
                switch (int.Parse(args[0]))
                {
                    case 6:
                        {
                            try
                            {
                                room = Essential.GetGame().GetRoomManager().GetRoom(uint.Parse(args[1]));
                                if (Session != null && room != null)
                                {
                                        ServerMessage Message = new ServerMessage(Outgoing.RoomForward);
                                        Message.AppendBoolean(false);
                                        Message.AppendUInt(room.Id);
                                        Session.SendMessage(Message);
                                }
                            }
                            catch { }
                            break;
                        }
                    case 7:
                        {

                            try
                            {
                                if (Session.GetHabbo().CurrentRoom.CheckRights(Session, false))
                                {
                                    int ItemId = int.Parse(args[1]);
                                    double newZ = double.Parse(args[2]);
                                    RoomItem ri = Session.GetHabbo().CurrentRoom.method_28((uint)ItemId);
                                    if (ri != null && ri.GetBaseItem().InteractionType == "stackfield")
                                    {
                                        ri.setHeight(newZ);
                                        ServerMessage smg = new ServerMessage(Outgoing.ObjectUpdate);
                                        ri.Serialize(smg);
                                        Session.GetHabbo().CurrentRoom.SendMessage(smg, null);
                                    }
                                }
                            }
                            catch { }
                            break;
                        }
                    case 10:
                        {
                            try
                            {
                                if (Session.GetHabbo().CurrentRoom.CheckRights(Session, false))
                                {
                                    uint itemid = uint.Parse(args[1]);
                                    int handitemId = int.Parse(args[2]);
                                    RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(itemid);
                                    if (ri != null && ri.GetBaseItem().InteractionType == "wf_cnd_has_handitem")
                                    {
                                        ri.string_2 = handitemId.ToString();
                                        ri.UpdateState(true, false);
                                    }
                                }
                            } catch { }
                            break;
                        }
                    case 12:
                        {
                            try
                            {
                                if (Session.GetHabbo().CurrentRoom.CheckRights(Session, false))
                                {
                                    uint itemid = uint.Parse(args[1]);
                                    string team = args[2];
                                    RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(itemid);
                                    if (ri != null && (ri.GetBaseItem().InteractionType == "wf_cnd_actor_in_team" || ri.GetBaseItem().InteractionType == "wf_cnd_not_in_team") && Session.GetHabbo().CurrentRoom.IsValidTeam(team))
                                    {
                                        ri.string_2 = team;
                                        ri.UpdateState(true, false);
                                    }
                                }
                            } catch { }
                            break;
                        }
                    case 14:
                        {
                            try
                            {
                                Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username).CarryItem(int.Parse(args[1]));
                            }
                            catch { }
                            break;
                        }
                    case 18:
                        {
                            if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                            {
                                try
                                {
                                    RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                    string BrandData = "state" + Convert.ToChar(9) + "0";
                                    if (ri == null || ri.GetBaseItem().InteractionType !="background")
                                        break;
                                    BrandData = BrandData + Convert.ToChar(9) + "imageUrl" + Convert.ToChar(9) + args[2];
                                    BrandData = BrandData + Convert.ToChar(9) + "offsetX" + Convert.ToChar(9) + int.Parse(args[3]);
                                    BrandData = BrandData + Convert.ToChar(9) + "offsetY" + Convert.ToChar(9) + int.Parse(args[4]);
                                    BrandData = BrandData + Convert.ToChar(9) + "offsetZ" + Convert.ToChar(9) + int.Parse(args[5]);
                                    using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
                                    {
                                        class2.AddParamWithValue("extradata", BrandData);
                                        class2.ExecuteQuery("UPDATE items_extra_data SET extra_data = @extradata WHERE item_id = '" + uint.Parse(args[1]) + "' LIMIT 1");
                                    }
                                    ri.ExtraData = BrandData;
                                    Session.GetHabbo().CurrentRoom.method_79(Session, ri, ri.GetX, ri.Int32_1, ri.int_3, false, false, true);
                                    ri.UpdateState(true, false, true);
                                }
                                catch(Exception ex){ Core.Logging.LogException(ex.ToString()); }
                            }
                            break;
                        }
                    case 19:
                    {
                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if (ri != null && ri.GetBaseItem().InteractionType == "badge_display")
                                {
                                    string Badge = Session.GetHabbo().GetBadgeComponent().HasBadge(args[2]) ? args[2] : "null";
                                    ri.ExtraData = Badge;
                                    using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
                                    {
                                        class2.AddParamWithValue("extradata", Badge);
                                        class2.ExecuteQuery("UPDATE items_extra_data SET extra_data = @extradata WHERE item_id = '" + uint.Parse(args[1]) + "' LIMIT 1");
                                    }
                                    Session.GetHabbo().CurrentRoom.method_79(Session, ri, ri.GetX, ri.Int32_1, ri.int_3, false, false, true);
                                    ri.UpdateState(true, false, true);
                                }
                            }
                            catch { }
                        }
                        break;
                    }
                    case 21:
                    {
                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_bot_follow_avatar")
                                {
                                    string username = args[2];
                                    int timeinseconds = 30;
                                    if (int.TryParse(args[3], out timeinseconds))
                                    {
                                        RoomUser bot = null;
                                        foreach (RoomUser ru in Session.GetHabbo().CurrentRoom.RoomUsers)
                                        {
                                            if (ru != null && ru.IsBot && !ru.IsPet && ru.RoomBot.Name == username)
                                                bot = ru;
                                        }
                                        if (bot != null)
                                            ri.string_2 = username + ";" + timeinseconds;
                                        ri.UpdateState(true, false);
                                    }
                                }
                            }
                            catch { }
                        }
                        break;
                    }
                    case 23:
                    {
                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_bot_give_handitem")
                                {
                                    int itemId = 0;
                                    if(int.TryParse(args[2], out itemId))
                                    {
                                        string username = args[3];
                                        RoomUser bot = null;
                                        foreach (RoomUser ru in Session.GetHabbo().CurrentRoom.RoomUsers)
                                        {
                                            if (ru != null && ru.IsBot && !ru.IsPet && ru.RoomBot.Name == username)
                                                bot = ru;
                                        }
                                        if(bot != null)
                                            ri.string_2 = itemId+ ";"+ username;
                                        ri.UpdateState(true, false);
                                    }
                                }
                            }
                            catch { }
                        }
                        break;
                    }
                    case 25:
                    {
                        if(Session.GetHabbo().CurrentRoom.CheckRights(Session,true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_bot_talk" && args[4].Length < 200)
                                {
                                    string username = args[2];
                                    string type = (args[3] == "shout" || args[3] =="say") ? args[3] : "say";
                                    string text = args[4];
                                    RoomUser bot = null;
                                    foreach(RoomUser ru in Session.GetHabbo().CurrentRoom.RoomUsers)
                                    {
                                        if (ru != null && ru.IsBot && !ru.IsPet && ru.RoomBot.Name == username)
                                            bot = ru;
                                    }
                                    if (bot != null)
                                        ri.string_2 = username + ";" + type + ";" + text;
                                    ri.UpdateState(true, false);
                                }
                            }
                            catch (Exception ex) { Logging.LogException(ex.ToString()); }
                        }
                        break;
                    }
                    case 26:
                    {
                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_bot_talk_to_avatar" && args[4].Length < 200)
                                {
                                    string username = args[2];
                                    string type = (args[3] == "whisper" || args[3] == "say") ? args[3] : "say";
                                    string text = args[4];
                                    RoomUser bot = null;
                                    foreach (RoomUser ru in Session.GetHabbo().CurrentRoom.RoomUsers)
                                    {
                                        if (ru != null && ru.IsBot && !ru.IsPet && ru.RoomBot.Name == username)
                                            bot = ru;
                                    }
                                    if (bot != null)
                                        ri.string_2 = username + ";" + type + ";" + text;
                                    ri.UpdateState(true, false);
                                }
                            }
                            catch (Exception ex) { Logging.LogException(ex.ToString()); }
                        }
                        break;
                    }
                    case 28:
                    {
                        if(Session.GetHabbo().CurrentRoom.CheckRights(Session,true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_bot_clothes")
                                {
                                    string username = args[2];
                                    string newlook = args[3];
                                    string gender = args[4];
                                    RoomUser bot = null;
                                    foreach (RoomUser ru in Session.GetHabbo().CurrentRoom.RoomUsers)
                                    {
                                        if (ru != null && ru.IsBot && !ru.IsPet && ru.RoomBot.Name == username)
                                            bot = ru;
                                    }
                                    if (bot != null && AntiMutant.ValidateLook(newlook, gender))      
                                        ri.string_2 = username + ";" + newlook + ";" + gender;
                                    ri.UpdateState(true, false);
                                }
                            }
                            catch { }
                        }
                        break;
                    }
                    case 32:
                    {
                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_yt")
                                {
                                    string ytlink = args[2].Split('=')[1];
                                    ri.string_2 = ytlink;
                                    ri.UpdateState(true, false);
                                }
                            }
                            catch { }
                        }
                        break;
                    }
                    case 35:
                    {
                        if(Session.GetHabbo().CurrentRoom.CheckRights(Session,true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if(ri != null &&(ri.GetBaseItem().InteractionType == "wf_cnd_has_purse" || ri.GetBaseItem().InteractionType == "wf_cnd_hasnot_purse"))
                                {
                                    string currency = Session.GetHabbo().CurrentRoom.IsValidCurrency(args[2]) ? args[2] : "credits";
                                    int number = 1337;
                                    int.TryParse(args[3], out number);
                                    ri.string_2 = currency + ";" + number;
                                    ri.UpdateState(true, false);
                                }
                           }
                            catch { }
                        }
                        break;
                    }
                    case 36:
                    {
                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                        {
                            try
                            {
                                RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_img" && IsValidFile(args[2]))
                                {
                                    ri.string_2 = args[2];
                                    ri.UpdateState(true, false);
                                }
                            }
                            catch { }
                        }
                        break;
                    }
                }
            }
            };
            });
        }
        public bool IsValidFile(string url)
        {
            return url.StartsWith("http://") && (url.EndsWith(".png") || url.EndsWith(".jpg") || url.EndsWith(".gif"));
        }
    }
}