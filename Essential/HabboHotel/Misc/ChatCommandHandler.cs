using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Achievements;
using Essential.HabboHotel.Users;
using Essential.Util;
using Essential.Messages;
using Essential.HabboHotel.Users.Authenticator;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Pets;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
using System.Threading;
using Essential.HabboHotel.Pathfinding;
using Essential.HabboHotel.Roles;
using System.Net;
using Essential.Net;
using System.IO;
namespace Essential.HabboHotel.Misc
{
    internal sealed class ChatCommandHandler
    {
        private static List<string> list_0;
        private static List<string> list_1;
        private static List<int> list_2;
        private static List<string> list_3;
        private static Configuration config;
        public static void Initialize(DatabaseClient class6_0)
        {
            Logging.Write("Loading Chat Filter..");
            ChatCommandHandler.list_0 = new List<string>();
            ChatCommandHandler.list_1 = new List<string>();
            ChatCommandHandler.list_2 = new List<int>();
            ChatCommandHandler.list_3 = new List<string>();
            ChatCommandHandler.config = Essential.GetGame().GetRoleManager().config;
            ChatCommandHandler.InitWords(class6_0);
            Logging.WriteLine("completed!", ConsoleColor.Green);
        }
        public static void InitWords(DatabaseClient dbClient)
        {
            ChatCommandHandler.list_0.Clear();
            ChatCommandHandler.list_1.Clear();
            ChatCommandHandler.list_2.Clear();
            ChatCommandHandler.list_3.Clear();
            DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM wordfilter ORDER BY word ASC;");
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ChatCommandHandler.list_0.Add(dataRow["word"].ToString());
                    ChatCommandHandler.list_1.Add(dataRow["replacement"].ToString());
                    ChatCommandHandler.list_2.Add(Essential.StringToInt(dataRow["strict"].ToString()));
                }
            }
            DataTable dataTable2 = dbClient.ReadDataTable("SELECT * FROM linkfilter;");
            if (dataTable2 != null)
            {
                foreach (DataRow dataRow in dataTable2.Rows)
                {
                    ChatCommandHandler.list_3.Add(dataRow["externalsite"].ToString());
                }
            }
        }
        public static bool InitLinks(string URLs)
        {
            if (ServerConfiguration.EnableExternalLinks == "disabled")
            {
                return false;
            }
            else
            {
                if ((URLs.StartsWith("http://") || URLs.StartsWith("www.") || URLs.StartsWith("https://")) && ChatCommandHandler.list_3 != null && ChatCommandHandler.list_3.Count > 0)
                {
                    foreach (string current in ChatCommandHandler.list_3)
                    {
                        if (URLs.Contains(current))
                        {
                            if (ServerConfiguration.EnableExternalLinks == "whitelist")
                            {
                                return true;
                            }
                            if (!(ServerConfiguration.EnableExternalLinks == "blacklist"))
                            {
                            }
                        }
                    }
                }
                return (URLs.StartsWith("http://") || URLs.StartsWith("www.") || (URLs.StartsWith("https://") && ServerConfiguration.EnableExternalLinks == "blacklist") || (ServerConfiguration.EnableExternalLinks == "whitelist" && false));
            }
        }
        public static string ApplyFilter(string string_0)
        {
            if (ChatCommandHandler.list_0 != null && ChatCommandHandler.list_0.Count > 0)
            {
                int num = -1;
                foreach (string current in ChatCommandHandler.list_0)
                {
                    num++;
                    if (string_0.ToLower().Contains(current.ToLower()) && ChatCommandHandler.list_2[num] == 1)
                    {
                        string_0 = Regex.Replace(string_0, current, ChatCommandHandler.list_1[num], RegexOptions.IgnoreCase);
                    }
                    else if (ChatCommandHandler.list_2[num] == 2)
                    {
                        string cheaters = @"\s*";
                        var re = new Regex(
                        @"\b("
                        + string.Join("|", list_0.Select(word =>
                            string.Join(cheaters, word.ToCharArray())))
                        + @")\b", RegexOptions.IgnoreCase);
                        return re.Replace(string_0, match =>
                        {
                            //return new string('*', match.Length);
                            return ChatCommandHandler.list_1[num];
                        });
                    }
                    else
                    {
                        if (string_0.ToLower().Contains(" " + current.ToLower() + " "))
                        {
                            string_0 = Regex.Replace(string_0, current, ChatCommandHandler.list_1[num], RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            return string_0;
        }
        public static bool HandleCommands(GameClient Session, string Input)
        {
            string[] Params = Input.Split(new char[]
			{
				' '
			});
            GameClient TargetClient = null;
            Room class2 = Session.GetHabbo().CurrentRoom;
            if (!Essential.GetGame().GetRoleManager().dictionary_4.ContainsKey(Params[0]))
            {
                return false;
            }
            else
            {
                try
                {
                    int num;
                    if (class2 != null && class2.CheckRights(Session, true))
                    {
                        num = Essential.GetGame().GetRoleManager().dictionary_4[Params[0]];
                        if (num <= 33)
                        {
                            if (num == 8)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                if (class2.bool_5)
                                {
                                    class2.bool_5 = false;
                                }
                                else
                                {
                                    class2.bool_5 = true;
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (num == 33)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                if (class2 != null && class2.CheckRights(Session, true))
                                {
                                    List<RoomItem> list = class2.method_24(Session);
                                    Session.GetHabbo().GetInventoryComponent().method_17(list);
                                    Session.GetHabbo().GetInventoryComponent().method_9(true);
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input + " " + Session.GetHabbo().CurrentRoomId);
                                    return true;
                                }
                                return false;
                            }
                        }
                        else
                        {
                            if (num == 46)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                try
                                {
                                    int num2 = int.Parse(Params[1]);
                                    if (Session.GetHabbo().Rank >= 6u)
                                    {
                                        class2.UsersMax = num2;
                                    }
                                    else
                                    {
                                        if (num2 > 100 || num2 < 5)
                                        {
                                            Session.SendNotification("Fehler: Wähle eine Zahl zwischen 5 und 100.");
                                        }
                                        else
                                        {
                                            class2.UsersMax = num2;
                                        }
                                    }
                                }
                                catch
                                {
                                    return false;
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (num == 53)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                Essential.GetGame().GetRoomManager().method_16(class2);
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        }
                    }
                    switch (Essential.GetGame().GetRoleManager().dictionary_4[Params[0]])
                    {
                        case 2:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_alert"))
                                {
                                    return false;
                                }
                                string TargetUser = Params[1];
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Konnte " + TargetUser + " nicht finden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.SendNotification(ChatCommandHandler.MergeParams(Params, 2), 0);
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 3:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_award"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Konnte " + text + " nicht finden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                Essential.GetGame().GetAchievementManager().addAchievement(TargetClient, Convert.ToUInt32(ChatCommandHandler.MergeParams(Params, 2)));
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 4:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_ban"))
                                {
                                    return false;
                                }
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Konnte " + Params[1] + " nicht finden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                {
                                    Session.SendNotification("Du kannst diesen Habbo nicht bannen.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }

                                string banLengthWithoutParams = Params[2];

                                banLengthWithoutParams = banLengthWithoutParams.Replace("m", "");
                                banLengthWithoutParams = banLengthWithoutParams.Replace("h", "");
                                banLengthWithoutParams = banLengthWithoutParams.Replace("d", "");

                                Console.WriteLine(banLengthWithoutParams);

                                int banLength = 0;

                                try
                                {
                                    banLength = int.Parse(banLengthWithoutParams);
                                }
                                catch (FormatException ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("OOPS! Something went wrong when trying format ban length! Report this and your date format!" + ex.ToString());
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }

                                if (Params[2].Contains("m"))
                                {
                                    banLength *= 60;
                                }

                                if (Params[2].Contains("h"))
                                {
                                    banLength *= 3600;
                                }

                                if (Params[2].Contains("d"))
                                {
                                    banLength *= 86400;
                                }

                                int laskettupaivia = 0;
                                int laskettutunteja = 0;
                                int laskettuminuutteja = 0;
                                int laskettavaa = banLength;

                                for (int i = 0; laskettavaa >= 86400; i++)
                                {
                                    laskettupaivia += 1;
                                    laskettavaa -= 86400;
                                }

                                for (int i = 0; laskettavaa >= 3600; i++)
                                {
                                    laskettutunteja += 1;
                                    laskettavaa -= 3600;
                                }

                                for (int i = 0; laskettavaa >= 60; i++)
                                {
                                    laskettuminuutteja += 1;
                                    laskettavaa -= 60;
                                }

                                if (banLength < 600)
                                {
                                    Session.SendNotification("Der Bann muss mindesten 10 Minuten (600 Sekunden) lang dauern.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                Session.SendNotification("Du hast " + TargetClient.GetHabbo().Username + " für " + laskettupaivia + " Tage " + laskettutunteja + "  Stunden " + laskettuminuutteja + " Minuten " + laskettavaa + " Sekunden gebannt.");
                                Essential.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, (double)banLength, ChatCommandHandler.MergeParams(Params, 3), false);
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 6:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_coins"))
                                {
                                    return false;
                                }
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Habbo wurde nicht gefunden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                int num4;
                                if (int.TryParse(Params[2], out num4))
                                {
                                    long NoBug = 0;
                                    //NoBug += TargetClient.GetHabbo().Credits;
                                    NoBug += num4;
                                    if (NoBug <= 30000 || NoBug >= -30000)
                                    {
                                        TargetClient.GetHabbo().GiveCredits(num4, "Command (coins)", Session.GetHabbo().Username);
                                        TargetClient.GetHabbo().UpdateCredits(true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " hat dir " + num4.ToString() + " Taler gegeben!");
                                        Session.SendNotification("Erfolgreich gesendet.");
                                        Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    }
                                    return true;
                                }

                                Session.SendNotification("Bitte send eine gültige Anzahl Taler");
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 7:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_coords"))
                                {
                                    return false;
                                }
                                class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    return false;
                                }
                                Session.SendNotification(string.Concat(new object[]
						{
							"X: ",
							class3.X,
							" - Y: ",
							class3.Y,
							" - Z: ",
							class3.double_0,
							" - Rot: ",
							class3.BodyRotation,
							", sqState: ",
							class2.Byte_0[class3.X, class3.Y].ToString()
						}));
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 11:
                            if (Session.GetHabbo().HasFuse("cmd_enable") && Session.GetHabbo().CurrentRoom.CanEnables)
                            {
                                int int_ = int.Parse(Params[1]);
                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(int_, true);
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 14:
                            if (Session.GetHabbo().HasFuse("cmd_freeze"))
                            {
                                RoomUser class4 = Session.GetHabbo().CurrentRoom.method_56(Params[1]);
                                if (class4 != null)
                                {
                                    class4.bool_5 = !class4.bool_5;
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 15:
                            if (Session.GetHabbo().HasFuse("cmd_givebadge"))
                            {
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null)
                                {
                                    TargetClient.GetHabbo().GetBadgeComponent().SendBadge(TargetClient, Essential.FilterString(Params[2]), true);
                                }
                                else
                                {
                                    Session.SendNotification("User: " + Params[1] + " could not be found in the database.\rPlease try your request again.");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 16:
                            if (Session.GetHabbo().HasFuse("cmd_globalcredits"))
                            {

                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    if (num5 <= 30000 && num5 >= -30000)
                                    {
                                        Essential.GetGame().GetClientManager().GiveCredits(num5);
                                        using (DatabaseClient class5 = Essential.GetDatabase().GetClient())
                                        {
                                            class5.ExecuteQuery("UPDATE users SET credits = credits + " + num5);
                                        }
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Eingabe muss eine Zahl sein.");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 17:
                            if (Session.GetHabbo().HasFuse("cmd_globalpixels"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    if (num5 <= 15000 && num5 >= -15000)
                                    {
                                        Essential.GetGame().GetClientManager().GivePixels(num5, false);
                                        using (DatabaseClient class5 = Essential.GetDatabase().GetClient())
                                        {
                                            class5.ExecuteQuery("UPDATE users SET activity_points = activity_points + " + num5);
                                        }
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Eingabe muss eine Zahl sein.");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 18:
                            if (Session.GetHabbo().HasFuse("cmd_globalpoints"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    if (num5 <= 100 && num5 >= -100)
                                    {
                                        Essential.GetGame().GetClientManager().GivePoints(num5, false);
                                        using (DatabaseClient class5 = Essential.GetDatabase().GetClient())
                                        {
                                            class5.ExecuteQuery("UPDATE users SET vip_points = vip_points + " + num5);
                                        }
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Eingabe muss eine Zahl sein.");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 19:
                            if (Session.GetHabbo().HasFuse("cmd_hal"))
                            {
                                string text2 = Params[1];
                                Input = Input.Substring(4).Replace(text2, "");
                                string text3 = Input.Substring(1);
                                ServerMessage Message = new ServerMessage(Outgoing.SendNotif); // Updated
                                Message.AppendStringWithBreak(string.Concat(new string[]
							{
								EssentialEnvironment.GetExternalText("cmd_hal_title"),
								"\r\n",
								text3,
								"\r\n-",
								Session.GetHabbo().Username
							}));
                                Message.AppendStringWithBreak(text2);
                                Essential.GetGame().GetClientManager().BroadcastMessage(Message);
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 20:
                            if (Session.GetHabbo().HasFuse("cmd_ha"))
                            {
                                string str = Input.Substring(3);
                                ServerMessage Message2 = new ServerMessage(Outgoing.BroadcastMessage);
                                Message2.AppendStringWithBreak(EssentialEnvironment.GetExternalText("mus_ha_title") + "\n\n" + str + "\n\n- " + Session.GetHabbo().Username);
                                Message2.AppendStringWithBreak("");
                                Essential.GetGame().GetClientManager().SendToHotel(Message2, Message2);
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 21:
                            if (Session.GetHabbo().HasFuse("cmd_invisible"))
                            {
                                Session.GetHabbo().IsVisible = !Session.GetHabbo().IsVisible;
                                Session.SendNotification("Du bist jetzt " + (Session.GetHabbo().IsVisible ? "sichtbar" : "unsichtbar") + "\nUm die Änderung zu speichern, lade den Raum neu.");
                                return true;
                            }
                            return false;
                        case 22:
                            if (!Session.GetHabbo().HasFuse("cmd_ipban"))
                            {
                                return false;
                            }
                            TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotification("Habbo wurde nicht gefunden.");
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                            {
                                Session.SendNotification("Du darfst diesen Habbo nicht bannen.");
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            Essential.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), true);
                            Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                            return true;
                        case 23:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_kick"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Konnte " + text + " nicht finden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank)
                                {
                                    Session.SendNotification("Du kannst diesen Habbo nicht kicken.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (TargetClient.GetHabbo().CurrentRoomId < 1u)
                                {
                                    Session.SendNotification("Dieser Habbo ist in keinem Raum, du kannst ihn nicht kicken.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                class2 = Essential.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                class2.RemoveUserFromRoom(TargetClient, true, false);
                                if (Params.Length > 2)
                                {
                                    TargetClient.SendNotification("Ein Moderator hat aus folgendem Grund gekickt: " + ChatCommandHandler.MergeParams(Params, 2));
                                }
                                else
                                {
                                    TargetClient.SendNotification("Ein Moderator hat dich aus dem Raum gekickt.");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 24:
                            if (Session.GetHabbo().HasFuse("cmd_massbadge"))
                            {
                                Essential.GetGame().GetClientManager().GiveBadge(Params[1]);
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 25:
                            if (Session.GetHabbo().HasFuse("cmd_masscredits"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    if (num5 <= 30000 && num5 >= -30000)
                                    {
                                        Essential.GetGame().GetClientManager().GiveCredits(num5);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Eingabe muss eine Zahl sein");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 26:
                            if (Session.GetHabbo().HasFuse("cmd_masspixels"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    if (num5 <= 15000 && num5 >= -15000)
                                    {
                                        Essential.GetGame().GetClientManager().GivePixels(num5, true);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Eingabe muss eine Zahl sein");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 27:
                            if (Session.GetHabbo().HasFuse("cmd_masspoints"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    if (num5 <= 100 && num5 >= -100)
                                    {
                                        Essential.GetGame().GetClientManager().GivePoints(num5, true);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Eingabe muss eine Zahl sein");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 30:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_motd"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Konnte " + text + " nicht finden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.SendNotification(ChatCommandHandler.MergeParams(Params, 2), 2);
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 31:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_mute"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null || TargetClient.GetHabbo() == null)
                                {
                                    Session.SendNotification("Konnte " + text + " nicht finden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                {
                                    Session.SendNotification("Du kannst diesen Habbo nicht (ent)muten.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.GetHabbo().Mute();
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 32:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_override"))
                                {
                                    return false;
                                }
                                class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    return false;
                                }
                                if (class3.bool_1)
                                {
                                    class3.bool_1 = false;
                                    Session.SendNotification("Durchlaufen deaktiviert.");
                                }
                                else
                                {
                                    class3.bool_1 = true;
                                    Session.SendNotification("Durchlaufen aktiviert.");
                                }
                                class2.method_22();
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 34:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_pixels"))
                                {
                                    return false;
                                }
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Habbo wurde nicht gefunden");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                int num4;
                                if (int.TryParse(Params[2], out num4))
                                {
                                    long NoBug = 0;
                                    NoBug += num4;
                                    if (NoBug <= 15000 || NoBug >= -15000)
                                    {
                                        TargetClient.GetHabbo().ActivityPoints = TargetClient.GetHabbo().ActivityPoints + num4;
                                        TargetClient.GetHabbo().UpdateActivityPoints(true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " hat dir " + num4.ToString() + " Pixel gegeben!");
                                        Session.SendNotification("Pixel erfolgreich versendet.");
                                        Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    }
                                    return true;
                                }
                                Session.SendNotification("Bitte send eine gültige Anzahl Pixel.");
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 35:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_points"))
                                {
                                    return false;
                                }
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Habbo wurde nicht gefunden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                int num4;
                                if (int.TryParse(Params[2], out num4))
                                {
                                    long NoBug = 0;
                                    NoBug += num4;
                                    if (NoBug <= 100 || NoBug >= -100)
                                    {
                                        TargetClient.GetHabbo().VipPoints = TargetClient.GetHabbo().VipPoints + num4;
                                        TargetClient.GetHabbo().UpdateVipPoints(false, true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " hat dir " + num4.ToString() + " Punkte gegeben!");
                                        Session.SendNotification("Punkte erfolgreich versendet.");
                                        Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    }

                                    return true;
                                }
                                Session.SendNotification("Bitte send eine gültige Anzahl Taler.");
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 39:
                            if (Session.GetHabbo().HasFuse("cmd_removebadge"))
                            {
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null)
                                {
                                    TargetClient.GetHabbo().GetBadgeComponent().RemoveBadge(Essential.FilterString(Params[2]));
                                }
                                else
                                {
                                    Session.SendNotification("Habbo " + Params[1] + " konnte nicht gefunden werden.");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 41:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_roomalert"))
                                {
                                    return false;
                                }
                                class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                string string_ = ChatCommandHandler.MergeParams(Params, 1);
                                for (int i = 0; i < class2.RoomUsers.Length; i++)
                                {
                                    RoomUser class6 = class2.RoomUsers[i];
                                    if (class6 != null && !class6.IsBot && !class6.IsPet)
                                    {
                                        class6.GetClient().SendNotification(string_);
                                    }
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 42:
                            if (!Session.GetHabbo().HasFuse("cmd_roombadge"))
                            {
                                return false;
                            }
                            if (Session.GetHabbo().CurrentRoom == null)
                            {
                                return false;
                            }
                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                            {
                                try
                                {
                                    RoomUser class6 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                    if (class6 != null)
                                    {
                                        if (!class6.IsBot)
                                        {
                                            if (class6.GetClient() != null)
                                            {
                                                if (class6.GetClient().GetHabbo() != null)
                                                {
                                                    class6.GetClient().GetHabbo().GetBadgeComponent().SendBadge(class6.GetClient(), Params[1], true);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Session.SendNotification("Error: " + ex.ToString());
                                }
                            }
                            Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                            return true;
                        case 43:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_roomkick"))
                                {
                                    return false;
                                }
                                class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                bool flag = true;
                                string text4 = ChatCommandHandler.MergeParams(Params, 1);
                                if (text4.Length > 0)
                                {
                                    flag = false;
                                }
                                for (int i = 0; i < class2.RoomUsers.Length; i++)
                                {
                                    RoomUser class7 = class2.RoomUsers[i];
                                    if (class7 != null && class7.GetClient().GetHabbo().Rank < Session.GetHabbo().Rank)
                                    {
                                        if (!flag)
                                        {
                                            class7.GetClient().SendNotification("Du wurdest gekickt: " + text4);
                                        }
                                        class2.RemoveUserFromRoom(class7.GetClient(), true, flag);
                                    }
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 44:
                            if (Session.GetHabbo().HasFuse("cmd_roommute"))
                            {
                                if (Session.GetHabbo().CurrentRoom.bool_4)
                                {
                                    Session.GetHabbo().CurrentRoom.bool_4 = false;
                                }
                                else
                                {
                                    Session.GetHabbo().CurrentRoom.bool_4 = true;
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 45:
                            if (Session.GetHabbo().HasFuse("cmd_sa"))
                            {
                                try
                                {
                                    foreach (GameClient staff in Essential.GetGame().GetClientManager().GetStaffs())
                                    {
                                        try
                                        {
                                            if (staff.GetHabbo().CurrentRoomId != 0)
                                            {
                                                ServerMessage msg = staff.GetHabbo().CurrentRoom.ComposeInfoMessage("[" + Session.GetHabbo().Username + "] " + Input.Substring(3), staff.GetHabbo().CurrentRoom.GetRoomUserByHabbo(staff.GetHabbo().Id).VirtualId);
                                                staff.SendMessage(msg);
                                            }
                                        }
                                        catch { }
                                    }
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                            return false;
                        case 47:
                            if (Session.GetHabbo().HasFuse("cmd_setspeed"))
                            {
                                int.Parse(Params[1]);
                                Session.GetHabbo().CurrentRoom.method_102(int.Parse(Params[1]));
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 48:
                            // no shutdown command :)
                            return false;
                        case 49:
                            if (Session.GetHabbo().HasFuse("cmd_spull"))
                            {
                                try
                                {
                                    if (!Session.GetHabbo().CurrentRoom.CanPull)
                                        return false;
                                    string a = "down";
                                    string text = Params[1];
                                    TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                    class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (Session == null || TargetClient == null)
                                    {
                                        return false;
                                    }
                                    RoomUser class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                    if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        Session.GetHabbo().Whisper("Du kannst dich nicht selber ziehen.");
                                        return true;
                                    }
                                    class6.HandleSpeech(Session, "*zieht " + TargetClient.GetHabbo().Username + " zu sich*", false);
                                    if (class6.BodyRotation == 0)
                                    {
                                        a = "up";
                                    }
                                    if (class6.BodyRotation == 2)
                                    {
                                        a = "right";
                                    }
                                    if (class6.BodyRotation == 4)
                                    {
                                        a = "down";
                                    }
                                    if (class6.BodyRotation == 6)
                                    {
                                        a = "left";
                                    }
                                    if (a == "up")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (!(class6.X == class2.RoomModel.DoorX && class6.Y - 1 == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                class4.MoveTo(class6.X, class6.Y - 1);
                                            else
                                                class4.MoveTo(class6.X, class6.Y + 1);
                                        }
                                        else
                                        {
                                            class4.MoveTo(class6.X, class6.Y - 1);
                                        }
                                    }
                                    if (a == "right")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (!(class6.X + 1 == class2.RoomModel.DoorX && class6.Y == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                class4.MoveTo(class6.X + 1, class6.Y);
                                            else
                                                class4.MoveTo(class6.X - 1, class6.Y);
                                        }
                                        else
                                        {
                                            class4.MoveTo(class6.X + 1, class6.Y);
                                        }
                                    }
                                    if (a == "down")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (!(class6.X == class2.RoomModel.DoorX && class6.Y + 1 == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                class4.MoveTo(class6.X, class6.Y + 1);
                                            else
                                                class4.MoveTo(class6.X, class6.Y - 1);
                                        }
                                        else
                                        {
                                            class4.MoveTo(class6.X, class6.Y + 1);
                                        }
                                    }
                                    if (a == "left")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (!(class6.X - 1 == class2.RoomModel.DoorX && class6.Y == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                class4.MoveTo(class6.X - 1, class6.Y);
                                            else
                                                class4.MoveTo(class6.X + 1, class6.Y);
                                        }
                                        else
                                        {
                                            class4.MoveTo(class6.X - 1, class6.Y);
                                        }
                                    }
                                    return true;
                                }
                                catch
                                {
                                    return false;
                                }
                            }
                            return false;
                        case 50:
                            if (Session.GetHabbo().HasFuse("cmd_summon"))
                            {
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null && TargetClient.GetHabbo().CurrentRoom != Session.GetHabbo().CurrentRoom)
                                {
                                    Room room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (room != null)
                                    {
                                        ServerMessage Message = new ServerMessage(Outgoing.RoomForward);
                                        Message.AppendBoolean(room.IsPublic);
                                        Message.AppendUInt(room.Id);
                                        TargetClient.SendMessage(Message);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " hat dich hergerufen!");
                                    }
                                }
                                else
                                {
                                    Session.GetHabbo().Whisper(Params[1] + " wurde nicht gefunden. Vielleicht ist er nicht mehr online.. :(");
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 51:
                            if (!Session.GetHabbo().HasFuse("cmd_superban"))
                            {
                                return false;
                            }
                            TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotification("Habbo wurde nicht gefunden.");
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                            {
                                Session.SendNotification("Du kannst diesen Habbo nicht bannen.");
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            Essential.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), false);
                            Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                            return true;
                        case 52:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_teleport"))
                                {
                                    return false;
                                }
                                class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    return false;
                                }
                                if (class3.TeleportMode)
                                {
                                    class3.TeleportMode = false;
                                    Session.GetHabbo().Whisper("Teleportieren ausgeschaltet.");
                                }
                                else
                                {
                                    class3.TeleportMode = true;
                                    Session.GetHabbo().Whisper("Teleportieren angeschaltet.");
                                }
                                class2.method_22();
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 54:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_unmute"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null || TargetClient.GetHabbo() == null)
                                {
                                    Session.SendNotification(text + " wurde nicht gefunden.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.GetHabbo().UnMute();
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 55:
                            if (Session.GetHabbo().HasFuse("cmd_update_achievements"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    AchievementManager.Load(dbClient);
                                }
                                return true;
                            }
                            return false;
                        case 56:
                            if (Session.GetHabbo().HasFuse("cmd_update_bans"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Essential.GetGame().GetBanManager().Initialise(dbClient);
                                }
                                Essential.GetGame().GetClientManager().UpdateBans();
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 57:
                            if (Session.GetHabbo().HasFuse("cmd_update_bots"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Essential.GetGame().GetBotManager().Initialize(dbClient);
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 58:
                            if (Session.GetHabbo().HasFuse("cmd_update_catalogue"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Essential.GetGame().GetCatalog().Initialize(dbClient);
                                    PetRace.Init(dbClient);
                                }
                                Essential.GetGame().GetCatalog().InitializeCache();
                                Essential.GetGame().GetClientManager().BroadcastMessage(new ServerMessage(Outgoing.UpdateShop)); // Updated
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 59:
                            if (Session.GetHabbo().HasFuse("cmd_update_filter"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    ChatCommandHandler.InitWords(dbClient);
                                    Essential.GetAntiAd().Refresh();
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 60:
                            if (Session.GetHabbo().HasFuse("cmd_update_items"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Essential.GetGame().GetItemManager().Initialize(dbClient);
                                }
                                Session.SendNotification("Item-Definition aktualisiert.");
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 61:
                            if (Session.GetHabbo().HasFuse("cmd_update_navigator"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Essential.GetGame().GetNavigator().Initialize(dbClient);
                                    Essential.GetGame().GetRoomManager().method_8(dbClient);
                                    Essential.GetGame().GetRoomManager().LoadMagicTiles(dbClient);
                                    Essential.GetGame().GetRoomManager().LoadBillboards(dbClient);
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 62:
                            if (Session.GetHabbo().HasFuse("cmd_update_permissions"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Essential.GetGame().GetRoleManager().Initialize(dbClient);
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 63:
                            if (Session.GetHabbo().HasFuse("cmd_update_settings"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    Essential.GetGame().LoadServerSettings(dbClient);
                                }
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 64:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_userinfo"))
                                {
                                    return false;
                                }
                                string text5 = Params[1];
                                bool flag2 = true;
                                if (string.IsNullOrEmpty(text5))
                                {
                                    Session.SendNotification("Bitte gib einen gültigen Benutzername ein");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                GameClient class8 = Essential.GetGame().GetClientManager().GetClientByHabbo(text5);
                                Habbo class9;
                                if (class8 == null)
                                {
                                    flag2 = false;
                                    class9 = Authenticator.CreateHabbo(text5);
                                }
                                else
                                {
                                    class9 = class8.GetHabbo();
                                }
                                if (class9 == null)
                                {
                                    Session.SendNotification("Konnte den Habbo " + Params[1] + " nicht finden");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                StringBuilder stringBuilder = new StringBuilder();
                                if (class9.CurrentRoom != null)
                                {
                                    stringBuilder.Append(" - RAUMINFORMATION FÜR RAUMID: " + class9.CurrentRoom.Id + " - \r");
                                    stringBuilder.Append("Besitzer: " + class9.CurrentRoom.Owner + "\r");
                                    stringBuilder.Append("Raumname: " + class9.CurrentRoom.Name + "\r");
                                    stringBuilder.Append(string.Concat(new object[]
							{
								"Habbos in Raum: ",
								class9.CurrentRoom.UserCount,
								"/",
								class9.CurrentRoom.UsersMax
							}));
                                }
                                uint num6 = class9.Rank;
                                string text6 = "";
                                if (Session.GetHabbo().HasFuse("cmd_userinfo_viewip"))
                                {
                                    text6 = "IP: " + class9.LastIp + " \r";
                                }
                                Session.SendNotification(string.Concat(new object[]
						{
							"Information über ",
							text5,
							":\rRank: ",
							num6,
							" \rOnline? ",
							flag2 ? "Ja" : "Nein",
							" \rUserID: ",
							class9.Id,
							" \r",
							text6,
							"Aktueller Raum: ",
							class9.CurrentRoomId,
							" \rMotto: ",
							class9.Motto,
							" \rTaler: ",
							class9.GetCredits(),
							" \rPixel: ",
							class9.ActivityPoints,
							" \rPunkte: ",
							class9.VipPoints,
							" \rStumm? ",
							class9.IsMuted ? "Ja" : "Nein",
							"\r\r\r",
							stringBuilder.ToString()
						}));
                                Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 65:
                            if (Session.GetHabbo().HasFuse("cmd_update_texts"))
                            {
                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                {
                                    EssentialEnvironment.LoadExternalTexts(dbClient);
                                }
                                return true;
                            }
                            return false;

                        case 66:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_disconnect"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Konnte den Habbo " + text + " nicht finden");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank)
                                {
                                    Session.SendNotification("Du kannst diesen Habbo nicht kicken.");
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.Disconnect("Command");
                                return true;
                            }
                        case 91:
                            if (!Session.GetHabbo().HasFuse("cmd_roomeffect"))
                            {
                                return false;
                            }
                            if (Session.GetHabbo().CurrentRoom == null)
                            {
                                return false;
                            }
                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                            {
                                try
                                {
                                    RoomUser class6 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                    if (class6 != null)
                                    {
                                        if (!class6.IsBot)
                                        {
                                            if (class6.GetClient() != null)
                                            {
                                                if (class6.GetClient().GetHabbo() != null)
                                                {
                                                    int int_ = int.Parse(Params[1]);
                                                    class6.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(int_, true);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Session.SendNotification("Fehler: " + ex.ToString());
                                }
                            }
                            Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                            return true;
                    }
                    num = Essential.GetGame().GetRoleManager().dictionary_4[Params[0]];
                    if (num <= 13)
                    {
                        if (num != 1)
                        {
                            switch (num)
                            {
                                case 5:
                                    {


                                        Session.GetHabbo().Whisper("Dieser Befehl existiert nicht mehr. Benutz doch die Funktion im Katalog :)");

                                        return true;
                                    }
                                case 6:
                                case 7:
                                case 8:
                                case 11:
                                    goto IL_3F91;
                                case 9:
                                    Session.GetHabbo().GetInventoryComponent().ClearInventory();
                                    Session.SendNotification(EssentialEnvironment.GetExternalText("cmd_emptyitems_success"));
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                case 10:
                                    if (Session.GetHabbo().HasFuse("cmd_empty") && Params[1] != null)
                                    {
                                        GameClient class10 = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                        if (class10 != null && class10.GetHabbo() != null)
                                        {
                                            class10.GetHabbo().GetInventoryComponent().ClearInventory();
                                            Session.SendNotification("Inventar geleert (Datenbank und Cache)");
                                        }
                                        else
                                        {
                                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                            {
                                                dbClient.AddParamWithValue("usrname", Params[1]);
                                                int num8 = int.Parse(dbClient.ReadString("SELECT Id FROM users WHERE username = @usrname LIMIT 1;"));
                                                dbClient.ExecuteQuery("DELETE FROM items WHERE user_id = '" + num8 + "' AND room_id = 0;");
                                                Session.SendNotification("Inventar geleert (Datenbank)");
                                            }
                                        }
                                        Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                        return true;
                                    }
                                    return false;
                                case 13:
                                    if (!(ServerConfiguration.UnknownBoolean9 || Session.GetHabbo().HasFuse("cmd_follow")))
                                    {
                                        Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_disabled"));
                                        return true;
                                    }
                                    if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_follow")))
                                    {
                                        Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        return true;
                                    }
                                    TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                    if (TargetClient != null && TargetClient.GetHabbo().InRoom && Session.GetHabbo().CurrentRoom != TargetClient.GetHabbo().CurrentRoom && !TargetClient.GetHabbo().HideInRom)
                                    {
                                        Room room = Essential.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);
                                        if (room != null)
                                        {
                                            ServerMessage Message = new ServerMessage(Outgoing.RoomForward);
                                            Message.AppendBoolean(room.IsPublic);
                                            Message.AppendUInt(room.Id);
                                            Session.SendMessage(Message);
                                            //TargetClient.SendNotification(Session.GetHabbo().Username + " hat dich hergerufen!");
                                        }
                                    }
                                    else
                                    {
                                        Session.GetHabbo().Whisper("Habbo " + Params[1] + " konnte nicht gefunden werden");
                                    }
                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                    return true;
                                default:
                                    goto IL_3F91;
                            }
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 28:
                                {
                                    if (!(ServerConfiguration.UnknownBoolean7 || Session.GetHabbo().HasFuse("cmd_mimic")))
                                    {
                                        Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_disabled"));
                                        return true;
                                    }
                                    if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_mimic")))
                                    {
                                        Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        return true;
                                    }
                                    string text = Params[1];
                                    TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                    if (TargetClient == null)
                                    {
                                        Session.GetHabbo().Whisper("Konnte " + text + " nicht finden.");
                                        return true;
                                    }
                                    if (TargetClient.GetHabbo().MimicDisabled)
                                        return false;
                                    Session.GetHabbo().Figure = TargetClient.GetHabbo().Figure;
                                    Session.GetHabbo().UpdateLook(false, Session);
                                    return true;
                                }
                            case 29:
                                {
                                    if (!(ServerConfiguration.UnknownBoolean8 || Session.GetHabbo().HasFuse("cmd_moonwalk")))
                                    {
                                        Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_disabled"));
                                        return true;
                                    }
                                    if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_moonwalk")))
                                    {
                                        Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        return true;
                                    }
                                    class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (class2 == null)
                                    {
                                        return false;
                                    }
                                    RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    if (class3 == null)
                                    {
                                        return false;
                                    }
                                    if (class3.bool_3)
                                    {
                                        class3.bool_3 = false;
                                        Session.GetHabbo().Whisper("Moonwalk deaktiviert.");
                                        return true;
                                    }
                                    class3.bool_3 = true;
                                    Session.GetHabbo().Whisper("Moonwalk aktiviert.");
                                    return true;
                                }
                            default:
                                {
                                    RoomUser class6;
                                    switch (num)
                                    {
                                        case 36:
                                            try
                                            {
                                                if (!(ServerConfiguration.UnknownBoolean2 || Session.GetHabbo().HasFuse("cmd_pull")))
                                                {
                                                    Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_disabled"));
                                                    return true;
                                                }
                                                if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_pull")))
                                                {
                                                    Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_permission_vip"));
                                                    return true;
                                                }
                                                if (!Session.GetHabbo().CurrentRoom.CanPull)
                                                    return false;
                                                string a = "down";
                                                string text = Params[1];
                                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                                class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                if (Session == null || TargetClient == null)
                                                {
                                                    return false;
                                                }
                                                class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                {
                                                    Session.GetHabbo().Whisper("Du kannst dich nicht ziehen.");
                                                    return true;
                                                }
                                                if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(class6.X - class4.X) < 3 && Math.Abs(class6.Y - class4.Y) < 3)
                                                {
                                                    class6.HandleSpeech(Session, "*zieht " + TargetClient.GetHabbo().Username + " zu sich*", false);
                                                    if (class6.BodyRotation == 0)
                                                    {
                                                        a = "up";
                                                    }
                                                    if (class6.BodyRotation == 2)
                                                    {
                                                        a = "right";
                                                    }
                                                    if (class6.BodyRotation == 4)
                                                    {
                                                        a = "down";
                                                    }
                                                    if (class6.BodyRotation == 6)
                                                    {
                                                        a = "left";
                                                    }
                                                    if (a == "up")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (!(class6.X == class2.RoomModel.DoorX && class6.Y - 1 == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                class4.MoveTo(class6.X, class6.Y - 1);
                                                            else
                                                                class4.MoveTo(class6.X, class6.Y + 1);
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class6.X, class6.Y - 1);
                                                        }
                                                    }
                                                    if (a == "right")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (!(class6.X + 1 == class2.RoomModel.DoorX && class6.Y == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                class4.MoveTo(class6.X + 1, class6.Y);
                                                            else
                                                                class4.MoveTo(class6.X - 1, class6.Y);
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class6.X + 1, class6.Y);
                                                        }
                                                    }
                                                    if (a == "down")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (!(class6.X == class2.RoomModel.DoorX && class6.Y + 1 == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                class4.MoveTo(class6.X, class6.Y + 1);
                                                            else
                                                                class4.MoveTo(class6.X, class6.Y - 1);
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class6.X, class6.Y + 1);
                                                        }
                                                    }
                                                    if (a == "left")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (!(class6.X - 1 == class2.RoomModel.DoorX && class6.Y == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                class4.MoveTo(class6.X - 1, class6.Y);
                                                            else
                                                                class4.MoveTo(class6.X + 1, class6.Y);
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class6.X - 1, class6.Y);
                                                        }
                                                    }
                                                    return true;
                                                }
                                                Session.GetHabbo().Whisper("Du kannst das nicht.");
                                                return true;
                                            }
                                            catch
                                            {
                                                return false;
                                            }
                                        case 37:
                                            break;
                                        case 38:
                                            goto IL_3F03;
                                        case 39:
                                            goto IL_3F91;
                                        case 40:
                                            {
                                                string text = Params[1];
                                                class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                RoomUser class4 = class2.method_57(text);
                                                if (class6.class34_1 != null)
                                                {
                                                    Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_ride_err_riding"));
                                                    return true;
                                                }
                                                if (!class4.IsBot || class4.PetData.Type != 15u)
                                                {
                                                    Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_ride_err_nothorse"));
                                                    return true;
                                                }
                                                bool arg_40EB_0;
                                                if ((class6.X + 1 != class4.X || class6.Y != class4.Y) && (class6.X - 1 != class4.X || class6.Y != class4.Y) && (class6.Y + 1 != class4.Y || class6.X != class4.X))
                                                {
                                                    if (class6.Y - 1 == class4.Y)
                                                    {
                                                        if (class6.X == class4.X)
                                                        {
                                                            goto IL_40C2;
                                                        }
                                                    }
                                                    arg_40EB_0 = (class6.X != class4.X || class6.Y != class4.Y);
                                                    goto IL_40EB;
                                                }
                                            IL_40C2:
                                                arg_40EB_0 = false;
                                            IL_40EB:
                                                if (arg_40EB_0)
                                                {
                                                    Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_ride_err_toofar"));
                                                    return true;
                                                }
                                                if (class4.RoomBot.RoomUser_0 == null)
                                                {
                                                    class4.RoomBot.RoomUser_0 = class6;
                                                    class6.class34_1 = class4.RoomBot;
                                                    class6.X = class4.X;
                                                    class6.Y = class4.Y;
                                                    class6.double_0 = class4.double_0 + 1.0;
                                                    class6.BodyRotation = class4.BodyRotation;
                                                    class6.int_7 = class4.int_7;
                                                    class6.UpdateNeeded = true;
                                                    class2.method_87(class6, false, false);
                                                    class6.RoomUser_0 = class4;
                                                    class6.Statusses.Clear();
                                                    class4.Statusses.Clear();
                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(77, true);
                                                    Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_ride_instr_getoff"));
                                                    class2.method_22();
                                                    return true;
                                                }
                                                Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_ride_err_tooslow"));
                                                return true;
                                            }
                                        case 88:
                                            try
                                            {
                                                if (!Session.GetHabbo().HasFuse("cmd_spush"))
                                                    return false;
                                                if (!Session.GetHabbo().CurrentRoom.CanPush)
                                                    return false;
                                                string a = "down";
                                                string text = Params[1];
                                                TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                                class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                if (Session == null || TargetClient == null)
                                                {
                                                    return false;
                                                }
                                                class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                {
                                                    Session.GetHabbo().Whisper("Du kannst das nicht!");
                                                    return true;
                                                }
                                                bool arg_3DD2_0;
                                                if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                {
                                                    if ((class6.X + 1 != class4.X || class6.Y != class4.Y) && (class6.X - 1 != class4.X || class6.Y != class4.Y) && (class6.Y + 1 != class4.Y || class6.X != class4.X))
                                                    {
                                                        if (class6.Y - 1 == class4.Y)
                                                        {
                                                            if (class6.X == class4.X)
                                                            {
                                                                goto IL_3DA6;
                                                            }
                                                        }
                                                        arg_3DD2_0 = (class6.X != class4.X || class6.Y != class4.Y);
                                                        goto IL_3DD2;
                                                    }
                                                IL_3DA6:
                                                    arg_3DD2_0 = false;
                                                }
                                                else
                                                {
                                                    arg_3DD2_0 = true;
                                                }
                                            IL_3DD2:
                                                if (!arg_3DD2_0)
                                                {
                                                    class6.HandleSpeech(Session, "*schubst " + TargetClient.GetHabbo().Username + "*", false);
                                                    if (class6.BodyRotation == 0)
                                                    {
                                                        a = "up";
                                                    }
                                                    if (class6.BodyRotation == 2)
                                                    {
                                                        a = "right";
                                                    }
                                                    if (class6.BodyRotation == 4)
                                                    {
                                                        a = "down";
                                                    }
                                                    if (class6.BodyRotation == 6)
                                                    {
                                                        a = "left";
                                                    }
                                                    if (a == "up")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class4.X, class4.Y - 1);
                                                                class4.MoveTo(class4.X, class4.Y - 2);
                                                                class4.MoveTo(class4.X, class4.Y - 3);
                                                                class4.MoveTo(class4.X, class4.Y - 4);
                                                                class4.MoveTo(class4.X, class4.Y - 5);
                                                            }
                                                            else
                                                            {
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y - 1 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y - 1);
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y - 2 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y - 2);
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y - 3 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y - 3);
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y - 4 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y - 4);
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y - 5 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y - 5);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class4.X, class4.Y - 1);
                                                            class4.MoveTo(class4.X, class4.Y - 2);
                                                            class4.MoveTo(class4.X, class4.Y - 3);
                                                            class4.MoveTo(class4.X, class4.Y - 4);
                                                            class4.MoveTo(class4.X, class4.Y - 5);
                                                        }
                                                    }
                                                    if (a == "right")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class4.X + 1, class4.Y);
                                                                class4.MoveTo(class4.X + 2, class4.Y);
                                                                class4.MoveTo(class4.X + 3, class4.Y);
                                                                class4.MoveTo(class4.X + 4, class4.Y);
                                                                class4.MoveTo(class4.X + 5, class4.Y);
                                                            }
                                                            else
                                                            {
                                                                if (!(class4.X + 1 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X + 1, class4.Y);
                                                                if (!(class4.X + 2 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X + 2, class4.Y);
                                                                if (!(class4.X + 3 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X + 3, class4.Y);
                                                                if (!(class4.X + 4 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X + 4, class4.Y);
                                                                if (!(class4.X + 5 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X + 5, class4.Y);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class4.X + 1, class4.Y);
                                                            class4.MoveTo(class4.X + 2, class4.Y);
                                                            class4.MoveTo(class4.X + 3, class4.Y);
                                                            class4.MoveTo(class4.X + 4, class4.Y);
                                                            class4.MoveTo(class4.X + 5, class4.Y);
                                                        }
                                                    }
                                                    if (a == "down")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class4.X, class4.Y + 1);
                                                                class4.MoveTo(class4.X, class4.Y + 2);
                                                                class4.MoveTo(class4.X, class4.Y + 3);
                                                                class4.MoveTo(class4.X, class4.Y + 4);
                                                                class4.MoveTo(class4.X, class4.Y + 5);
                                                            }
                                                            else
                                                            {
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y + 1 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y + 1);
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y + 2 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y + 2);
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y + 3 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y + 3);
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y + 4 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y + 4);
                                                                if (!(class4.X == class2.RoomModel.DoorX && class4.Y + 5 == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X, class4.Y + 5);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class4.X, class4.Y + 1);
                                                            class4.MoveTo(class4.X, class4.Y + 2);
                                                            class4.MoveTo(class4.X, class4.Y + 3);
                                                            class4.MoveTo(class4.X, class4.Y + 4);
                                                            class4.MoveTo(class4.X, class4.Y + 5);
                                                        }
                                                    }
                                                    if (a == "left")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class4.X - 1, class4.Y);
                                                                class4.MoveTo(class4.X - 2, class4.Y);
                                                                class4.MoveTo(class4.X - 3, class4.Y);
                                                                class4.MoveTo(class4.X - 4, class4.Y);
                                                                class4.MoveTo(class4.X - 5, class4.Y);
                                                            }
                                                            else
                                                            {
                                                                if (!(class4.X - 1 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X - 1, class4.Y);
                                                                if (!(class4.X - 2 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X - 2, class4.Y);
                                                                if (!(class4.X - 3 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X - 3, class4.Y);
                                                                if (!(class4.X - 4 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X - 4, class4.Y);
                                                                if (!(class4.X - 5 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY))
                                                                    class4.MoveTo(class4.X - 5, class4.Y);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class4.X - 1, class4.Y);
                                                            class4.MoveTo(class4.X - 2, class4.Y);
                                                            class4.MoveTo(class4.X - 3, class4.Y);
                                                            class4.MoveTo(class4.X - 4, class4.Y);
                                                            class4.MoveTo(class4.X - 5, class4.Y);
                                                        }
                                                    }
                                                }
                                                return true;
                                            }
                                            catch
                                            {
                                                return false;
                                            }
                                        default:
                                            switch (num)
                                            {
                                                case 67:
                                                    {
                                                        string text7 = "Deine Befehle:\r\r";
                                                        if (Session.GetHabbo().HasFuse("cmd_update_settings"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_settings_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_bans"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_bans_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_permissions"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_permissions_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_filter"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_filter_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_bots"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_bots_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_catalogue"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_catalogue_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_items"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_items_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_navigator"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_navigator_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_achievements"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_achievements_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_award"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_award_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_coords"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_coords_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_override"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_override_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_teleport"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_teleport_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_coins"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_coins_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_pixels"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_pixels_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_points"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_points_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_alert"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_alert_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_motd"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_motd_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roomalert"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_roomalert_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_ha"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_ha_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_hal"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_hal_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_freeze"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_freeze_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_enable"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_enable_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roommute"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_roommute_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_setspeed"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_setspeed_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_globalcredits"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_globalcredits_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_globalpixels"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_globalpixels_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_globalpoints"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_globalpoints_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_masscredits"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_masscredits_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_masspixels"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_masspixels_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_masspoints"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_masspoints_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_givebadge"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_givebadge_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_removebadge"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_removebadge_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_summon"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_summon_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roombadge"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_roombadge_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_massbadge"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_massbadge_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_userinfo"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_userinfo_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_shutdown"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_shutdown_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_invisible"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_invisible_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_ban"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_ban_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_superban"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_superban_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_ipban"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_ipban_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_kick"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_kick_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roomkick"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_roomkick_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_mute"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_mute_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_unmute"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_unmute_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_sa"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_sa_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_spull"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_spull_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_empty"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_empty_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_texts"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_update_texts_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_dance"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_dance_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_rave"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_rave_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roll"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_roll_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_control"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_control_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_makesay"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_makesay_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_sitdown"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_sitdown_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_lay"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_lay_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_startquestion"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_startquestion_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_handitem"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_handitem_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_vipha"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_vipha_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_spush"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_spush_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roomeffect"))
                                                        {
                                                            text7 = text7 + EssentialEnvironment.GetExternalText("cmd_roomeffect_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().IsVIP)
                                                        {
                                                            if (ServerConfiguration.UnknownBoolean8 || Session.GetHabbo().HasFuse("cmd_moonwalk"))
                                                            {
                                                                text7 = text7 + EssentialEnvironment.GetExternalText("cmd_moonwalk_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean7 || Session.GetHabbo().HasFuse("cmd_mimi"))
                                                            {
                                                                text7 = text7 + EssentialEnvironment.GetExternalText("cmd_mimic_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean9 || Session.GetHabbo().HasFuse("cmd_follow"))
                                                            {
                                                                text7 = text7 + EssentialEnvironment.GetExternalText("cmd_follow_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean1 || Session.GetHabbo().HasFuse("cmd_push"))
                                                            {
                                                                text7 = text7 + EssentialEnvironment.GetExternalText("cmd_push_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean2 || Session.GetHabbo().HasFuse("cmd_pull"))
                                                            {
                                                                text7 = text7 + EssentialEnvironment.GetExternalText("cmd_pull_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean3 || Session.GetHabbo().HasFuse("cmd_flagme"))
                                                            {
                                                                text7 = text7 + EssentialEnvironment.GetExternalText("cmd_flagme_desc") + "\r\r";
                                                            }
                                                        }
                                                        string text8 = "";
                                                        if (ServerConfiguration.EnableRedeemCredits)
                                                        {
                                                            text8 = text8 + EssentialEnvironment.GetExternalText("cmd_redeemcreds_desc") + "\r\r";
                                                        }
                                                        string text9 = "";
                                                        if (ServerConfiguration.EnableRedeemPixels)
                                                        {
                                                            text9 = text9 + EssentialEnvironment.GetExternalText("cmd_redeempixel_desc") + "\r\r";
                                                        }
                                                        string redeemshell = "";
                                                        if (ServerConfiguration.EnableRedeemShells)
                                                        {
                                                            redeemshell = redeemshell + EssentialEnvironment.GetExternalText("cmd_redeemshell_desc") + "\r\r";
                                                        }
                                                        string SpecialText = "";
                                                        //asdf
                                                        SpecialText = "Custom Commands:\r---------------------\r\r";
                                                        if (config.getData("cmd.drive.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.drive.name") + " " + config.getData("cmd.drive.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.roomalert.minrank")) && config.getData("cmd.roomalert.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.roomalert.name") + " " + config.getData("cmd.roomalert.desc") + "\r\r";
                                                        if (config.getData("cmd.makemedance.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.makemedance.name") + " " + config.getData("cmd.makemedance.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.rotate.minrank")) && config.getData("cmd.rotate.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.rotate.name") + " " + config.getData("cmd.rotate.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.faceless.minrank")) && config.getData("cmd.faceless.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.faceless.name") + " " + config.getData("cmd.faceless.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.roomfreeze.minrank")) && config.getData("cmd.roomfreeze.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.roomfreeze.name") + " " + config.getData("cmd.roomfreeze.desc") + "\r\r";
                                                        if (config.getData("cmd.habnam.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.habnam.name") + " " + config.getData("cmd.habnam.desc") + "\r\r";
                                                        if (config.getData("cmd.super.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.super.name") + " " + config.getData("cmd.super.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.laydown.minrank")) && config.getData("cmd.laydown.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.laydown.name") + " " + config.getData("cmd.laydown.desc") + "\r\r";
                                                        if (config.getData("cmd.afk.enabled") == "1")
                                                            SpecialText = SpecialText + config.getData("cmd.afk.desc") + "\r\r";
                                                        if (config.getData("cmd.enablefollow.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.enablefollow.name") + " " + config.getData("cmd.enablefollow.desc") + "\r\r";
                                                        if (config.getData("cmd.disablefollow.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.disablefollow.name") + " " + config.getData("cmd.disablefollow.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.slap.minrank")) && config.getData("cmd.slap.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.slap.name") + " " + config.getData("cmd.slap.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.miau.minrank")) && config.getData("cmd.miau.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.miau.name") + " " + config.getData("cmd.miau.desc") + "\r\r";
                                                        if (config.getData("cmd.staff.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.staff.name") + " " + config.getData("cmd.staff.desc") + "\r\r";
                                                        if (config.getData("cmd.howmanyrooms.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.howmanyrooms.name") + " " + config.getData("cmd.howmanyrooms.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.protect.minrank")) && config.getData("cmd.protect.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.protect.name") + " " + config.getData("cmd.protect.desc") + "\r\r";
                                                        if (config.getData("cmd.trade.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.trade.name") + " " + config.getData("cmd.trade.desc") + "\r\r";
                                                        if (config.getData("cmd.werber.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.werber.name") + " " + config.getData("cmd.werber.desc") + "\r\r";
                                                        if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.customhotelalert.minrank")) && config.getData("cmd.customhotelalert.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.customhotelalert.name") + " " + config.getData("cmd.customhotelalert.desc") + "\r\r";
                                                        if (config.getData("cmd.toggletrade.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.toggletrade.name") + " " + config.getData("cmd.toggletrade.desc") + "\r\r";
                                                        if (config.getData("cmd.eingang.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.eingang.name") + " " + config.getData("cmd.eingang.desc") + "\r\r";
                                                        if (config.getData("cmd.homeroom.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.homeroom.name") + " " + config.getData("cmd.homeroom.desc") + "\r\r";
                                                        if (config.getData("cmd.infocenter.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.infocenter.name") + " " + config.getData("cmd.infocenter.desc") + "\r\r";
                                                        if (config.getData("cmd.petcmds.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.petcmds.name") + " " + config.getData("cmd.petcmds.desc") + "\r\r";
                                                        if (config.getData("cmd.eventha.enabled") == "1" && Session.GetHabbo().Rank >= uint.Parse(config.getData("cmd.eventha.minrank")))
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.eventha.name") + " " + config.getData("cmd.eventha.desc") + "\r\r";
                                                        if (config.getData("cmd.emptybots.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.emptybots.name") + " " + config.getData("cmd.emptybots.desc") + "\r\r";
                                                        if (config.getData("cmd.looktome.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.looktome.name") + " " + config.getData("cmd.looktome.desc") + "\r\r";
                                                        if (config.getData("cmd.stand.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.stand.name") + " " + config.getData("cmd.stand.desc") + "\r\r";
                                                        if (config.getData("cmd.mutepets.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.mutepets.name") + " " + config.getData("cmd.mutepets.desc") + "\r\r";
                                                        if (config.getData("cmd.mutebots.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.mutebots.name") + " " + config.getData("cmd.mutebots.desc") + "\r\r";
                                                        if (config.getData("cmd.kickpets.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.kickpets.name") + " " + config.getData("cmd.kickpets.desc") + "\r\r";
                                                        if (config.getData("cmd.kickbots.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.kickbots.name") + " " + config.getData("cmd.kickbots.desc") + "\r\r";
                                                        if (config.getData("cmd.roompush.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.roompush.name") + " " + config.getData("cmd.roompush.desc") + "\r\r";
                                                        if (config.getData("cmd.roompull.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.roompull.name") + " " + config.getData("cmd.roompull.desc") + "\r\r";
                                                        if (config.getData("cmd.roomenable.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.roomenable.name") + " " + config.getData("cmd.roomenable.desc") + "\r\r";
                                                        if (config.getData("cmd.roomrespect.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.roomrespect.name") + " " + config.getData("cmd.roomrespect.desc") + "\r\r";
                                                        if (config.getData("cmd.disablegiftalerts.enabled") == "1")
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.disablegiftalerts.name") + " " + config.getData("cmd.disablegiftalerts.desc") + "\r\r";
                                                        if (config.getData("cmd.disablemimic.enabled") == "1" && Session.GetHabbo().IsVIP)
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.disablemimic.name") + " " + config.getData("cmd.disablemimic.desc") + "\r\r";
                                                        if (config.getData("cmd.enablewalkunder.enabled") == "1" && Session.GetHabbo().IsVIP)
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.enablewalkunder.name") + " " + config.getData("cmd.enablewalkunder.desc") + "\r\r";
                                                        if (config.getData("cmd.disablewalkunder.enabled") == "1" && Session.GetHabbo().IsVIP)
                                                            SpecialText = SpecialText + ":" + config.getData("cmd.disablewalkunder.name") + " " + config.getData("cmd.disablewalkunder.desc") + "\r\r";
                                                        string text11 = text7;
                                                        text7 = string.Concat(new string[]
									                    {
										                    text11,     
										                    "- - - - - - - - - - - \r\r",
										                    EssentialEnvironment.GetExternalText("cmd_about_desc"),
										                    "\r\r",
										                    EssentialEnvironment.GetExternalText("cmd_pickall_desc"),
										                    "\r\r",
										                    EssentialEnvironment.GetExternalText("cmd_unload_desc"),
										                    "\r\r",
										                    EssentialEnvironment.GetExternalText("cmd_disablediagonal_desc"),
										                    "\r\r",
										                    EssentialEnvironment.GetExternalText("cmd_setmax_desc"),
										                    "\r\r",
										                    text8,
                                                            text9,
                                                            redeemshell,
										                    EssentialEnvironment.GetExternalText("cmd_ride_desc"),
										                    "\r\r",
										                    EssentialEnvironment.GetExternalText("cmd_buy_desc"),
										                    "\r\r",
										                    EssentialEnvironment.GetExternalText("cmd_emptypets_desc"),
										                    "\r\r",
										                    EssentialEnvironment.GetExternalText("cmd_emptyitems_desc"),
                                                            SpecialText
									                    });
                                                        Session.SendNotification(text7, 2);
                                                        return true;
                                                    }
                                                case 68:
                                                    goto IL_2F05;
                                                case 69:
                                                    {
                                                        StringBuilder stringBuilder2 = new StringBuilder();
                                                        for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                                                        {
                                                            class6 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                                            if (class6 != null)
                                                            {
                                                                stringBuilder2.Append(string.Concat(new object[]
											{
												"UserID: ",
												class6.UId,
												" RoomUID: ",
												class6.int_20,
												" VirtualID: ",
												class6.VirtualId,
												" IsBot:",
												class6.IsBot.ToString(),
												" X: ",
												class6.X,
												" Y: ",
												class6.Y,
												" Z: ",
												class6.double_0,
												" \r\r"
											}));
                                                            }
                                                        }
                                                        Session.SendNotification(stringBuilder2.ToString());
                                                        Session.SendNotification("RoomID: " + Session.GetHabbo().CurrentRoomId);
                                                        return true;
                                                    }
                                                case 70:
                                                    {
                                                        return false;
                                                    }
                                                case 71:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_dance"))
                                                    {
                                                        class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        GameClient class10 = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                        RoomUser class3 = class2.GetRoomUserByHabbo(class10.GetHabbo().Id);
                                                        class3.DanceId = 1;
                                                        ServerMessage Message6 = new ServerMessage(Outgoing.Dance);
                                                        Message6.AppendInt32(class3.VirtualId);
                                                        Message6.AppendInt32(1);
                                                        class2.SendMessage(Message6, null);
                                                        return true;
                                                    }
                                                    return false;
                                                case 72:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_rave"))
                                                    {
                                                        class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        class2.Rave();
                                                        return true;
                                                    }
                                                    return false;
                                                case 73:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_roll"))
                                                    {
                                                        GameClient class10 = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                        class10.GetHabbo().int_1 = (int)Convert.ToInt16(Params[2]);
                                                        return true;
                                                    }
                                                    return false;
                                                case 74:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_control"))
                                                    {
                                                        string text = Params[1];
                                                        try
                                                        {
                                                            TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                                            class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (Session == null || TargetClient == null)
                                                            {
                                                                return false;
                                                            }
                                                            RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                            class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class6.RoomUser_0 = class4;
                                                        }
                                                        catch
                                                        {
                                                            class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (Session == null || TargetClient == null)
                                                            {
                                                                return false;
                                                            }
                                                            class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class6.RoomUser_0 = null;
                                                        }
                                                        return true;
                                                    }
                                                    return false;
                                                case 75:
                                                    {
                                                        if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_makesay"))
                                                        {
                                                            string text2 = Params[1];
                                                            TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text2);
                                                            class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (Session == null || TargetClient == null)
                                                            {
                                                                return false;
                                                            }
                                                            RoomUser roomUser = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                            roomUser.HandleSpeech(TargetClient, Input.Substring(9 + text2.Length), false);
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 76:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_sitdown"))
                                                    {
                                                        class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        class2.method_55();
                                                        return true;
                                                    }
                                                    return false;
                                                case 77:
                                                    {
                                                        return false;
                                                    }
                                                case 78:
                                                    goto IL_3F91;
                                                case 79:
                                                    {
                                                        if (!Session.GetHabbo().InRoom)
                                                        {
                                                            return false;
                                                        }
                                                        class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        int int_2 = class2.method_56(Session.GetHabbo().Username).CarryItemID;
                                                        if (int_2 <= 0)
                                                        {
                                                            Session.GetHabbo().Whisper("Du hasts nichts in der Hand. Nimm dir erstmal was.");
                                                            return true;
                                                        }
                                                        string text = Params[1];
                                                        TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                                        class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                        if (Session == null || TargetClient == null)
                                                        {
                                                            return false;
                                                        }
                                                        if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                        {
                                                            return true;
                                                        }
                                                        if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(class6.X - class4.X) < 3 && Math.Abs(class6.Y - class4.Y) < 3)
                                                        {
                                                            try
                                                            {
                                                                class2.method_56(Params[1]).CarryItem(int_2);
                                                                class2.method_56(Session.GetHabbo().Username).CarryItem(0);
                                                            }
                                                            catch
                                                            {
                                                            }
                                                            return true;
                                                        }
                                                        Session.GetHabbo().Whisper("Du bist zu weit weg von " + Params[1] + ". Geh näher ran.");
                                                        return true;
                                                    }
                                                case 80:
                                                    if (!Session.GetHabbo().InRoom)
                                                    {
                                                        return false;
                                                    }
                                                    class6 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);
                                                    if (class6.Statusses.ContainsKey("sit") || class6.Statusses.ContainsKey("lay") || class6.BodyRotation == 1 || class6.BodyRotation == 3 || class6.BodyRotation == 5 || class6.BodyRotation == 7)
                                                    {
                                                        return true;
                                                    }
                                                    if (class6.byte_1 > 0 || class6.class34_1 != null)
                                                    {
                                                        return true;
                                                    }
                                                    if (!(class6.X == Session.GetHabbo().CurrentRoom.RoomModel.DoorX && class6.Y == Session.GetHabbo().CurrentRoom.RoomModel.DoorY))
                                                    {
                                                        class6.AddStatus("sit", ((class6.double_0 + 1.0) / 2.0 - class6.double_0 * 0.5).ToString().Replace(",", "."));
                                                        class6.UpdateNeeded = true;
                                                    }
                                                    return true;
                                                case 81:
                                                case 82:
                                                    class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                    if (class6.class34_1 != null)
                                                    {
                                                        Session.GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                                        class6.class34_1.RoomUser_0 = null;
                                                        class6.class34_1 = null;
                                                        class6.double_0 -= 1.0;
                                                        class6.Statusses.Clear();
                                                        class6.UpdateNeeded = true;
                                                        int int_3 = Essential.smethod_5(0, class2.RoomModel.int_4);
                                                        int int_4 = Essential.smethod_5(0, class2.RoomModel.int_5);
                                                        class6.RoomUser_0.MoveTo(int_3, int_4);
                                                        class6.RoomUser_0 = null;
                                                        class2.method_87(class6, false, false);
                                                    }
                                                    return true;
                                                case 83:
                                                    Session.GetHabbo().GetInventoryComponent().RemovePetsFromInventory();
                                                    Session.SendNotification(EssentialEnvironment.GetExternalText("cmd_emptypets_success"));
                                                    Essential.GetGame().GetClientManager().StoreCommand(Session, Params[0].ToLower(), Input);
                                                    return true;
                                                case 85:
                                                    if (!Session.GetHabbo().HasFuse("cmd_handitem"))
                                                    {
                                                        return false;
                                                    }
                                                    Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("13");
                                                    /*class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    if (class2 == null)
                                                    {
                                                        return false;
                                                    }
                                                    class2.method_56(Session.GetHabbo().Username).CarryItem(int.Parse(Params[1]));
                                                    */
                                                    return true;
                                                case 86:
                                                    {
                                                        if (!Session.GetHabbo().HasFuse("cmd_lay"))
                                                        {
                                                            return false;
                                                        }
                                                        Room currentRoom = Session.GetHabbo().CurrentRoom;
                                                        if (currentRoom == null)
                                                        {
                                                            return false;
                                                        }
                                                        RoomUser roomUserByHabbo2 = currentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (roomUserByHabbo2 == null)
                                                        {
                                                            return false;
                                                        }
                                                        if (!roomUserByHabbo2.Statusses.ContainsKey("lay"))
                                                        {
                                                            if (roomUserByHabbo2.BodyRotation % 2 == 0)
                                                            {
                                                                if (!(roomUserByHabbo2.X == Session.GetHabbo().CurrentRoom.RoomModel.DoorX && roomUserByHabbo2.Y == Session.GetHabbo().CurrentRoom.RoomModel.DoorY))
                                                                {
                                                                    roomUserByHabbo2.Statusses.Add("lay", Convert.ToString((double)Session.GetHabbo().CurrentRoom.Byte_0[roomUserByHabbo2.X, roomUserByHabbo2.Y] + 0.55).ToString().Replace(",", "."));
                                                                    roomUserByHabbo2.UpdateNeeded = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Session.GetHabbo().Whisper("Du kannst nicht diagonal liegen.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            roomUserByHabbo2.Statusses.Remove("lay");
                                                            roomUserByHabbo2.UpdateNeeded = true;
                                                        }
                                                        return true;
                                                    }
                                                case 95:
                                                    class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                    if (class6.Boolean_3)
                                                    {
                                                        Session.GetHabbo().Whisper("Befehl während einem Tausch nicht möglich!");
                                                        return true;
                                                    }
                                                    if (ServerConfiguration.EnableRedeemPixels)
                                                    {
                                                        Session.GetHabbo().GetInventoryComponent().RedeemPixel(Session);
                                                    }
                                                    else
                                                    {
                                                        Session.GetHabbo().Whisper(Essential.smethod_1("cmd_error_disabled"));
                                                    }
                                                    return true;
                                                case 96:
                                                    class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                    if (class6.Boolean_3)
                                                    {
                                                        Session.GetHabbo().Whisper("Befehl während einem Tausch nicht möglich!");
                                                        return true;
                                                    }
                                                    if (ServerConfiguration.EnableRedeemShells)
                                                    {
                                                        Session.GetHabbo().GetInventoryComponent().RedeemShell(Session);
                                                    }
                                                    else
                                                    {
                                                        Session.GetHabbo().Whisper(Essential.smethod_1("cmd_error_disabled"));
                                                    }
                                                    return true;
                                                case 97:
                                                    try
                                                    {

                                                        RoomUser class6_001;
                                                        GameClient User2 = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                        class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                        RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                        if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                        {
                                                            if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                            {
                                                                return false;
                                                            }
                                                            else
                                                            {



                                                                bool AreFriends = false;

                                                                using (DatabaseClient @class = Essential.GetDatabase().GetClient())
                                                                {
                                                                    DataTable dt1 = @class.ReadDataTable("SELECT * FROM messenger_friendships");


                                                                    foreach (DataRow dr1 in dt1.Rows)
                                                                    {
                                                                        if (Convert.ToInt32(dr1["user_one_id"].ToString()) == User2.GetHabbo().Id && Convert.ToInt32(dr1["user_two_id"]) == Session.GetHabbo().Id || Convert.ToInt32(dr1["user_one_id"].ToString()) == Session.GetHabbo().Id && Convert.ToInt32(dr1["user_two_id"]) == User2.GetHabbo().Id)
                                                                        {
                                                                            AreFriends = true;
                                                                        }
                                                                    }
                                                                }
                                                                if (AreFriends == true)
                                                                {
                                                                    class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                    RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                    RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                    if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                    {

                                                                        Aktuelleruser.HandleSpeech(Session, "*küsst " + User2.GetHabbo().Username + " *", false);
                                                                        Aktuelleruser.Kiss();
                                                                        Usr2.Kiss();

                                                                        return true;
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                        return true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("Ihr müsst Freunde sein, um euch zu küssen");
                                                                    return true;
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }
                                                    }

                                                    catch
                                                    {
                                                        return false;
                                                    }
                                                case 98:
                                                    if (Session.GetHabbo().HasFuse("cmd_summon"))
                                                    {
                                                        Essential.GetGame().GetClientManager().HotelSummon(Session.GetHabbo().CurrentRoomId, false);
                                                        return true;
                                                    }
                                                    return false;
                                                case 99:
                                                    Session.SendNotification("Benutz stattdessen die Funktion im Client :)");
                                                    return true;
                                                case 100:

                                                    Room TargetRoom;
                                                    TargetRoom = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    if (TargetRoom != null && TargetRoom.CheckRights(Session, true))
                                                    {
                                                        try
                                                        {
                                                            if (TargetRoom.CanBuy == true)
                                                            {
                                                                TargetRoom.CanBuy = false;
                                                                Session.SendNotification("Der Verkauf wurde gestoppt!");
                                                            }
                                                            else
                                                            {
                                                                TargetRoom.roomcost = int.Parse(Params[1]);
                                                                TargetRoom.CanBuy = true;
                                                                Session.SendNotification("Der Raum steht nun zum Verkauf! Verkauf kann mit :sellroom beendet werden.");
                                                                for (int i = 0; i < TargetRoom.RoomUsers.Length; i++)
                                                                {
                                                                    RoomUser User = TargetRoom.RoomUsers[i];
                                                                    if (User == null)
                                                                        continue;
                                                                    User.GetClient().SendNotification("Dieser Raum wird gerade für " + TargetRoom.roomcost + " Taler verkauft.\r\nWenn du den Raum kaufen willst, dann Tippe :buyroom");
                                                                }
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            Session.SendNotification("Es ist ein Fehler aufgetreten beim Command :sellroom <Preis>");
                                                        }
                                                    }
                                                    return true;
                                                case 101:
                                                    TargetRoom = Session.GetHabbo().CurrentRoom;
                                                    if (TargetRoom.CanBuy)
                                                    {
                                                        if (Session.GetHabbo().GetCredits() >= TargetRoom.roomcost)
                                                        {
                                                            TargetRoom.CanBuy = false;
                                                            Session.GetHabbo().TakeCredits(TargetRoom.roomcost, "Bought Room", "Room-ID: " + TargetRoom.Id);
                                                            Session.GetHabbo().UpdateCredits(true);
                                                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                            {
                                                                GameClient RoomOwner = Essential.GetGame().GetClientManager().GetClientByHabbo(TargetRoom.Owner);
                                                                RoomOwner.GetHabbo().SetCredits(TargetRoom.roomcost, "Sold Room", "Room-ID: " + TargetRoom.Id);
                                                                RoomOwner.GetHabbo().UpdateCredits(true);
                                                                dbClient.AddParamWithValue("uname", Session.GetHabbo().Username);
                                                                dbClient.ExecuteQuery("UPDATE rooms SET owner = @uname WHERE id = " + TargetRoom.Id + " LIMIT 1");
                                                            }
                                                            for (int i = 0; i < TargetRoom.RoomUsers.Length; i++)
                                                            {
                                                                RoomUser User = TargetRoom.RoomUsers[i];
                                                                if (User == null || User.GetClient() == null)
                                                                    continue;
                                                                User.GetClient().SendNotification("Dieser Raum wurde gerade an " + Session.GetHabbo().Username + " verkauft! Deshalb wurden alle aus dem Raum geworfen, damit der Raum nun " + Session.GetHabbo().Username + " gehört.");
                                                            }
                                                            Essential.GetGame().GetRoomManager().method_16(TargetRoom);
                                                        }
                                                        else
                                                        {
                                                            Session.SendNotification("Du hast zu wenig Taler um den Raum zu kaufen!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Session.SendNotification("Dieser Raum steht nicht zum Verkauf");
                                                    }
                                                    return true;
                                                case 104:
                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.handitem.minrank")))
                                                    {
                                                        Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("13");
                                                        /*class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        int int_23 = int.Parse(Params[1]);
                                                        class2.method_56(Session.GetHabbo().Username).CarryItem(int_23);*/
                                                        return true;
                                                    }
                                                    return false;
                                                case 105:
                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.hipster.minrank")))
                                                    {
                                                        Session.GetHabbo().Figure = Session.GetHabbo().Gender == "m" ? "hr-3278-36-35.ca-3292-63.hd-209-27.sh-3252-90-1408.ea-3168-63.ch-3222-94.lg-3257-81" : "hd-600-1370.sh-3184-68.ea-3168-63.ch-3266-70-1408.lg-3235-110.hr-3044-38";
                                                        Session.GetHabbo().UpdateLook(false, Session);
                                                        return true;
                                                    }
                                                    return false;
                                                case 106:
                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.noob.minrank")))
                                                    {
                                                        Session.GetHabbo().Figure = Session.GetHabbo().Gender == "m" ? "ha-1002-70.hd-180-7.sh-305-62.wa-2007-0.ch-215-66.lg-270-79.hr-100-0" : "hd-600-10.sh-725-75.wa-2002-63.ea-1401-63.ch-630-70.lg-705-76.hr-500-38";
                                                        Session.GetHabbo().UpdateLook(false, Session);
                                                        return true;
                                                    }
                                                    return false;
                                                case 107:
                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.bkg.minrank")))
                                                    {
                                                        Session.GetHabbo().Figure = Session.GetHabbo().Gender == "m" ? "fa-1202-63.hd-209-3.sh-3027-110-1408.cc-3007-63-1408.ch-3015-108.lg-270-110.hr-3043-40" : "cc-3008-70-1411.hd-600-19.sh-725-75.ea-3107-64-64.ch-3013-110.lg-3174-94-1323.hr-3040-38";
                                                        Session.GetHabbo().UpdateLook(false, Session);
                                                        return true;
                                                    }
                                                    return false;
                                                case 108:

                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.fly.minrank")))
                                                    {
                                                        Room currentRoom1 = Session.GetHabbo().CurrentRoom;
                                                        RoomUser roomUserByHabbo = null;
                                                        currentRoom1 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (currentRoom1 != null)
                                                        {
                                                            roomUserByHabbo = currentRoom1.GetRoomUserByHabbo(Session.GetHabbo().Id);

                                                            if (roomUserByHabbo != null)
                                                            {
                                                                roomUserByHabbo.isFlying = true;
                                                                roomUserByHabbo.bool_1 = true;
                                                            }
                                                        }
                                                        return true;
                                                    }
                                                    return false;
                                                case 109:
                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(22, true);
                                                    return true;
                                                case 110:
                                                    if (Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username && Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.roomalert.minrank")))
                                                    {
                                                        class2 = Session.GetHabbo().CurrentRoom;
                                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                        {
                                                            RoomUser class16 = class2.RoomUsers[i];
                                                            if (class16 != null)
                                                            {
                                                                class16.GetClient().SendNotification(Input.Remove(0, 3));
                                                            }
                                                        }
                                                        return true;
                                                    }
                                                    return false;
                                                case 111:
                                                    class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                                                    RoomUser class33 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                    class33.DanceId = 1;
                                                    ServerMessage Message62 = new ServerMessage(Outgoing.Dance);
                                                    Message62.AppendInt32(class33.VirtualId);
                                                    Message62.AppendInt32(1);
                                                    class2.SendMessage(Message62, null);
                                                    return true;
                                                case 112:
                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.rotate.minrank")))
                                                    {
                                                        class2 = Session.GetHabbo().CurrentRoom;
                                                        RoomUser rUser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (int.Parse(Params[1]) > 0 && int.Parse(Params[1]) < 7)
                                                        {
                                                            rUser.Unidle();
                                                            rUser.method_9(int.Parse(Params[1]));
                                                        }
                                                        return true;
                                                    }
                                                    return false;
                                                case 114:
                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.faceless.minrank")))
                                                    {
                                                        string[] figureParts;
                                                        string[] headParts;
                                                        figureParts = Session.GetHabbo().Figure.Split('.');
                                                        foreach (string Part in figureParts)
                                                        {
                                                            if (Part.StartsWith("hd"))
                                                            {
                                                                headParts = Part.Split('-');
                                                                if (!headParts[1].Equals("99999"))
                                                                    headParts[1] = "99999";
                                                                else
                                                                    break;
                                                                string NewHead = "hd-" + headParts[1] + "-" + headParts[2];
                                                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, NewHead);
                                                                break;
                                                            }
                                                        }
                                                        Session.GetHabbo().UpdateLook(false, Session);
                                                        return true;
                                                    }
                                                    return false;
                                                case 115:
                                                    if (Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username && Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.roomfreeze.minrank")))
                                                    {
                                                        class2 = Session.GetHabbo().CurrentRoom;
                                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                        {
                                                            RoomUser class16 = class2.RoomUsers[i];
                                                            if (class16 != null)
                                                            {
                                                                class16.bool_5 = !class16.bool_5;
                                                            }
                                                        }
                                                        return true;
                                                    }
                                                    return false;
                                                case 120:
                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(140, true);
                                                    return true;
                                                case 121:
                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(59, true);
                                                    return true;
                                                case 122:
                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.laydown.minrank")))
                                                    {
                                                        class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        class2.laydown();
                                                        return true;
                                                    }
                                                    return false;
                                                case 123:
                                                    if (Session.GetHabbo().PetData == null)
                                                    {
                                                        Session.GetHabbo().PetData = "1 0 D4D4D4";
                                                        Session.SendNotification("Du du bist nun eine #Miau Katze. Bitte Raum neuladen damit du aussiehst wie eine Katze.");
                                                    }
                                                    else
                                                    {
                                                        Session.GetHabbo().PetData = null;
                                                        Session.SendNotification("Du bist nun wieder ein Habbo. Bitte Raum wechseln oder neu laden, damit du wieder normal aussiehst.");
                                                    }
                                                    return true;
                                                case 124:
                                                    Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id).bool_8 = true;
                                                    ServerMessage Idle = new ServerMessage(Outgoing.IdleStatus);
                                                    Idle.AppendInt32(Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id).VirtualId);
                                                    Idle.AppendBoolean(true);
                                                    Session.GetHabbo().CurrentRoom.SendMessage(Idle, null);
                                                    return true;
                                                case 128:
                                                    //Session.SendNotification("Aktuelle CPU Nutzung: " + LowPriorityWorker.cpusage + "%\nAktueller RAM Verbrauch: " + LowPriorityWorker.ramusage / (1024 * 1024) + " MB");
                                                    return false;
                                                case 131:
                                                    TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                    class2 = Session.GetHabbo().CurrentRoom;
                                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                    RoomUser class69 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                    if (TargetClient.GetHabbo().protectMeFromPushAndPull || Session.GetHabbo().protectMeFromPushAndPull)
                                                    {
                                                        return false;
                                                    }
                                                    string a = "down";
                                                    if (class6 != class69)
                                                    {
                                                        if (Math.Abs(class6.X - class69.X) < 2 && Math.Abs(class6.Y - class69.Y) < 2)
                                                        {
                                                            class6.HandleSpeech(Session, config.getData("cmd.slap.message").Replace("%target%", Params[1]), true);
                                                            class69.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                            class69.HandleSpeech(Session, config.getData("cmd.slap.message.ouch"), true);
                                                            bool arg_3DD2_0;

                                                            if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if ((class6.X + 1 != class69.X || class6.Y != class69.Y) && (class6.X - 1 != class69.X || class6.Y != class69.Y) && (class6.Y + 1 != class69.Y || class6.X != class69.X))
                                                                {
                                                                    if (class6.Y - 1 == class69.Y)
                                                                    {
                                                                        if (class6.X == class69.X)
                                                                        {
                                                                            goto IL_3DA6;
                                                                        }
                                                                    }
                                                                    arg_3DD2_0 = (class6.X != class69.X || class6.Y != class69.Y);
                                                                    goto IL_3DD2;
                                                                }
                                                            IL_3DA6:
                                                                arg_3DD2_0 = false;
                                                            }
                                                            else
                                                            {
                                                                arg_3DD2_0 = true;
                                                            }
                                                        IL_3DD2:
                                                            if (!arg_3DD2_0)
                                                            {
                                                                if (class6.BodyRotation == 0)
                                                                {
                                                                    a = "up";
                                                                }
                                                                if (class6.BodyRotation == 2)
                                                                {
                                                                    a = "right";
                                                                }
                                                                if (class6.BodyRotation == 4)
                                                                {
                                                                    a = "down";
                                                                }
                                                                if (class6.BodyRotation == 6)
                                                                {
                                                                    a = "left";
                                                                }
                                                                if (a == "up")
                                                                {
                                                                    if (ServerConfiguration.PreventDoorPush)
                                                                    {
                                                                        if ((!(class69.X == class2.RoomModel.DoorX && class69.Y - 1 == class2.RoomModel.DoorY)) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                        {
                                                                            class69.MoveTo(class69.X, class69.Y - 2);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        class69.MoveTo(class69.X, class69.Y - 2);
                                                                    }
                                                                }
                                                                if (a == "right")
                                                                {
                                                                    if (ServerConfiguration.PreventDoorPush)
                                                                    {
                                                                        if (!(class69.X + 1 == class2.RoomModel.DoorX && class69.Y == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                            class69.MoveTo(class69.X + 2, class69.Y);
                                                                    }
                                                                    else
                                                                    {
                                                                        class69.MoveTo(class69.X + 2, class69.Y);
                                                                    }
                                                                }
                                                                if (a == "down")
                                                                {
                                                                    if (ServerConfiguration.PreventDoorPush)
                                                                    {
                                                                        if (!(class69.X == class2.RoomModel.DoorX && class69.Y + 1 == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                            class69.MoveTo(class69.X, class69.Y + 2);
                                                                    }
                                                                    else
                                                                    {
                                                                        class69.MoveTo(class69.X, class69.Y + 2);
                                                                    }
                                                                }
                                                                if (a == "left")
                                                                {
                                                                    if (ServerConfiguration.PreventDoorPush)
                                                                    {
                                                                        if (!(class69.X - 1 == class2.RoomModel.DoorX && class69.Y == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                            class69.MoveTo(class69.X - 2, class69.Y);
                                                                    }
                                                                    else
                                                                    {
                                                                        class69.MoveTo(class69.X - 2, class69.Y);
                                                                    }
                                                                }
                                                            }
                                                            return true;
                                                        }
                                                        else
                                                        {
                                                            Session.GetHabbo().Whisper(config.getData("cmd.slap.message.toofaraway"));
                                                            return true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Session.GetHabbo().Whisper(config.getData("cmd.slap.message.cantslapyourself"));
                                                        return true;
                                                    }
                                                case 132:
                                                    try
                                                    {
                                                        int anzahl = 0;
                                                        string s = "Zurzeit sind %anz% Staffs online:";
                                                        foreach (string username in Essential.GetGame().GetClientManager().OnlineStaffs(3))
                                                        {
                                                            anzahl = anzahl + 1;
                                                            s = s + "\n- " + username;
                                                        }
                                                        s = s.Replace("%anz%", anzahl.ToString());
                                                        if (anzahl == 0)
                                                            s = "Zurzeit sind leider keine Staffs online!";

                                                        Session.SendNotification(s);
                                                    }
                                                    catch { }
                                                    return true;
                                                case 133:
                                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                    {
                                                        dbClient.AddParamWithValue("username", Params[1]);
                                                        DataTable drow = dbClient.ReadDataTable("SELECT caption FROM rooms WHERE owner=@username");
                                                        int Anzahl = 0;
                                                        string s = "Der User " + Params[1] + " hat %anz% Räume:";
                                                        foreach (DataRow dr in drow.Rows)
                                                        {
                                                            Anzahl = Anzahl + 1;
                                                            s = s + "\n- " + dr["caption"].ToString();
                                                        }
                                                        if (Anzahl == 0)
                                                            s = "Der User " + Params[1] + " hat im Moment keine Räume!";
                                                        Session.SendNotification(s.Replace("%anz%", Anzahl.ToString()));
                                                    }
                                                    return true;
                                                case 134:
                                                    Session.GetHabbo().protectMeFromPushAndPull = Session.GetHabbo().protectMeFromPushAndPull ? false : true;
                                                    Session.GetHabbo().Whisper(Session.GetHabbo().protectMeFromPushAndPull ? config.getData("cmd.protect.message.on") : config.getData("cmd.protect.message.off"));
                                                    return true;
                                                case 135:
                                                    if (Session.GetHabbo().CurrentRoom.CheckRights(Session, false))
                                                    {
                                                        if (Params[1] == "on")
                                                        {
                                                            Session.GetHabbo().CurrentRoom.Category = 9;
                                                            Session.GetHabbo().Whisper(config.getData("cmd.trade.message.tradeon"));
                                                        }
                                                        else
                                                        {
                                                            Session.GetHabbo().CurrentRoom.Category = 0;
                                                            Session.GetHabbo().Whisper(config.getData("cmd.trade.message.tradeoff"));
                                                        }
                                                        return true;
                                                    }
                                                    return false;
                                                case 136:
                                                    TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                    if (TargetClient == null || TargetClient.GetHabbo().Rank >= 4)
                                                    {
                                                        Session.SendNotification("Konnte den User nicht finden / Du kannst diesen User nicht melden.");
                                                        return true;
                                                    }
                                                    Session.SendNotification("Vielen Dank für deinen Bericht. Falls ein Staff online ist, wird er den Werber direkt bannen!");

                                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                    {
                                                        dbClient.AddParamWithValue("useridwerber", TargetClient.GetHabbo().Id);
                                                        dbClient.AddParamWithValue("useridmelder", Session.GetHabbo().Id);
                                                        dbClient.AddParamWithValue("timestamp", Essential.GetUnixTimestamp());
                                                        dbClient.ExecuteQuery("INSERT INTO hp_modlog (user_id, action, bemerkung, timestamp) VALUES (@useridwerber, 'werber', @useridmelder, @timestamp)");
                                                    }
                                                    /*foreach (string username in Essential.GetGame().GetClientManager().OnlineStaffs(4))
                                                    {
                                                        Essential.getWebSocketManager().getWebSocketByName(username).Send("3|" + Params[1].Replace("|", "") + "|" + Session.GetHabbo().Username.Replace("|", ""));
                                                    }*/
                                                    return true;
                                                case 137:
                                                    if (Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.customhotelalert.minrank")))
                                                    {
                                                        string pt1 = "2|" + Session.GetHabbo().Username.Replace("|", "") + "|";
                                                        Essential.getWebSocketManager().SendMessageToEveryConnection(pt1 + Input.Replace("|", "").Substring(config.getData("cmd.customhotelalert.name").Length + 1));

                                                        return true;
                                                    }
                                                    return false;
                                                case 138:
                                                    Session.GetHabbo().TradingDisabled = Session.GetHabbo().TradingDisabled ? false : true;
                                                    Session.SendNotification("Dich kann man " + (Session.GetHabbo().TradingDisabled ? "nicht mehr antauschen" : "antauschen"));

                                                    return true;
                                                case 139:
                                                    Session.LoadRoom(uint.Parse(config.getData("cmd.eingang.roomid")));
                                                    return true;
                                                case 140:
                                                    Session.LoadRoom(Session.GetHabbo().HomeRoomId);
                                                    return true;
                                                case 141:
                                                    Session.LoadRoom(uint.Parse(config.getData("cmd.infocenter.roomid")));
                                                    return true;
                                                case 143:
                                                    {
                                                        if (Session.GetHabbo().Rank >= uint.Parse(config.getData("cmd.eventha.minrank")))
                                                        {
                                                            string text = Input.Substring(1 + config.getData("cmd.eventha.name").Length);
                                                            if (text.Length > 1)
                                                            {
                                                                text = AntiAd.AntiAd.Utf8ToUtf16(text);
                                                                
                                                                string toSend = "5|" + text + "|" + Session.GetHabbo().CurrentRoom.Owner + "|" + Session.GetHabbo().CurrentRoomId;
                                                                Essential.getWebSocketManager().SendMessageToEveryConnection(toSend);
                                                                Session.GetHabbo().StoreActivity("makeevent", Session.GetHabbo().CurrentRoomId, Essential.GetUnixTimestamp(), text);
                                                            }
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 144:
                                                    {
                                                        if (Session.GetHabbo().Rank >= uint.Parse(config.getData("cmd.backup.minrank")))
                                                        {
                                                            WebClient wc = new WebClient();
                                                            System.Threading.Thread thrd = new Thread(delegate()
                                                            {
                                                                wc.DownloadString(config.getData("cmd.backup.url"));
                                                                Session.SendNotification("Backup erstellt!");
                                                            });
                                                            thrd.Start();
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 145:
                                                    {
                                                        Session.GetHabbo().GetInventoryComponent().ClearBots();
                                                        return true;
                                                    }
                                                case 146:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {

                                                            foreach (RoomUser ru in Session.GetHabbo().CurrentRoom.RoomUsers)
                                                            {
                                                                if (ru != null && ru.IsBot && !ru.IsPet)
                                                                {
                                                                    RoomUser ru2 = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                    int int_ = Rotation.GetRotation(ru.X, ru.Y, ru2.X, ru2.Y);
                                                                    ru.method_9(int_);
                                                                    if (ru.class34_1 != null && ru.RoomUser_0 != null)
                                                                    {
                                                                        ru.RoomUser_0.method_9(int_);
                                                                    }
                                                                    return true;
                                                                }
                                                            }
                                                        }
                                                        return false;
                                                    }
                                                case 147:
                                                    {
                                                        RoomUser rUser = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (rUser.Statusses.ContainsKey("sit"))
                                                            rUser.Statusses.Remove("sit");
                                                        if (rUser.Statusses.ContainsKey("lay"))
                                                            rUser.Statusses.Remove("lay");
                                                        rUser.UpdateNeeded = true;
                                                        return true;
                                                    }
                                                case 148:
                                                    {
                                                        Session.GetHabbo().MutePets = !Session.GetHabbo().MutePets;
                                                        return true;
                                                    }
                                                case 149:
                                                    {
                                                        Session.GetHabbo().MuteBots = !Session.GetHabbo().MuteBots;
                                                        return true;
                                                    }
                                                case 150:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {
                                                            #region "Delete Pet from Room"
                                                            Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (@class != null && !@class.IsPublic && (@class.AllowPet || @class.CheckRights(Session, true)))
                                                            {

                                                                foreach (RoomUser petUser in @class.RoomUsers)
                                                                {
                                                                    if (petUser == null || (petUser != null && !petUser.IsPet))
                                                                        continue;
                                                                    if (petUser != null && petUser.PetData != null)
                                                                    {
                                                                        using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
                                                                        {
                                                                            if (petUser.PetData.DBState == DatabaseUpdateState.NeedsInsert)
                                                                            {
                                                                                class3.AddParamWithValue("petname" + petUser.PetData.PetId, petUser.PetData.Name);
                                                                                class3.AddParamWithValue("petcolor" + petUser.PetData.PetId, petUser.PetData.Color);
                                                                                class3.AddParamWithValue("petrace" + petUser.PetData.PetId, petUser.PetData.Race);
                                                                                class3.ExecuteQuery(string.Concat(new object[]
							                                                {
								                                                "INSERT INTO `user_pets` VALUES ('",
								                                                petUser.PetData.PetId,
								                                                "', '",
								                                                petUser.PetData.OwnerId,
								                                                "', '0', @petname",
								                                                petUser.PetData.PetId,
								                                                ", @petrace",
								                                                petUser.PetData.PetId,
								                                                ", @petcolor",
								                                                petUser.PetData.PetId,
								                                                ", '",
								                                                petUser.PetData.Type,
								                                                "', '",
								                                                petUser.PetData.Expirience,
								                                                "', '",
								                                                petUser.PetData.Energy,
								                                                "', '",
								                                                petUser.PetData.Nutrition,
								                                                "', '",
								                                                petUser.PetData.Respect,
								                                                "', '",
								                                                petUser.PetData.CreationStamp,
								                                                "', '",
								                                                petUser.PetData.X,
								                                                "', '",
								                                                petUser.PetData.Y,
								                                                "', '",
								                                                petUser.PetData.Z,
								                                                "');"
							                                                }));
                                                                            }
                                                                            else
                                                                            {
                                                                                class3.ExecuteQuery(string.Concat(new object[]
							                                                {
								                                                "UPDATE user_pets SET room_id = '0', expirience = '",
								                                                petUser.PetData.Expirience,
								                                                "', energy = '",
								                                                petUser.PetData.Energy,
								                                                "', nutrition = '",
								                                                petUser.PetData.Nutrition,
								                                                "', respect = '",
								                                                petUser.PetData.Respect,
								                                                "' WHERE Id = '",
								                                                petUser.PetData.PetId,
								                                                "' LIMIT 1; "
							                                                }));
                                                                            }
                                                                            petUser.PetData.DBState = DatabaseUpdateState.Updated;
                                                                        }
                                                                        try
                                                                        {
                                                                            Essential.GetGame().GetClientManager().GetClientById(petUser.PetData.OwnerId).GetHabbo().GetInventoryComponent().AddPet(petUser.PetData);

                                                                        }
                                                                        catch { }
                                                                        @class.method_6(petUser.VirtualId, false);
                                                                        petUser.RoomId = 0u;
                                                                    }
                                                                }
                                                            }
                                                            #endregion
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 151:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {
                                                            #region "Delete Bots from Room"
                                                            Room @class = Session.GetHabbo().CurrentRoom;
                                                            if (@class != null)
                                                            {
                                                                foreach (RoomUser rUser in @class.RoomUsers)
                                                                {
                                                                    if (rUser == null || (rUser != null && rUser.IsBot && !rUser.IsPet))
                                                                        continue;
                                                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                                    {
                                                                        dbClient.ExecuteQuery("UPDATE user_bots SET room_id=0 WHERE id=" + (rUser.UId - 1000));
                                                                    }
                                                                    Session.GetHabbo().GetInventoryComponent().AddBot(rUser.UId - 1000);
                                                                    @class.method_6(rUser.VirtualId, false);
                                                                }
                                                            }
                                                            #endregion
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 152:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {
                                                            Session.GetHabbo().CurrentRoom.CanPush = !Session.GetHabbo().CurrentRoom.CanPush;
                                                            Session.GetHabbo().Whisper(config.getData("cmd.roompush.message").Replace("%str%", Session.GetHabbo().CurrentRoom.CanPush ? config.getData("cmd.roompush.positive") : config.getData("cmd.roompush.negative")));
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 153:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {
                                                            Session.GetHabbo().CurrentRoom.CanPull = !Session.GetHabbo().CurrentRoom.CanPull;
                                                            Session.GetHabbo().Whisper(config.getData("cmd.roompull.message").Replace("%str%", Session.GetHabbo().CurrentRoom.CanPull ? config.getData("cmd.roompull.positive") : config.getData("cmd.roompull.negative")));
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 154:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {
                                                            Session.GetHabbo().CurrentRoom.CanEnables = !Session.GetHabbo().CurrentRoom.CanEnables;
                                                            Session.GetHabbo().Whisper(config.getData("cmd.roomenable.message").Replace("%str%", Session.GetHabbo().CurrentRoom.CanEnables ? config.getData("cmd.roomenable.positive") : config.getData("cmd.roomenable.negative")));
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 155:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {
                                                            Session.GetHabbo().CurrentRoom.CanRespect = !Session.GetHabbo().CurrentRoom.CanRespect;
                                                            Session.GetHabbo().Whisper(config.getData("cmd.roomrespect.message").Replace("%str%", Session.GetHabbo().CurrentRoom.CanRespect ? config.getData("cmd.roomrespect.positive") : config.getData("cmd.roomrespect.negative")));
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 156:
                                                    {
                                                        Session.GetHabbo().GiftAlert = !Session.GetHabbo().GiftAlert;
                                                        return true;
                                                    }
                                                case 157:
                                                    {
                                                        if (Session.GetHabbo().IsVIP)
                                                        {
                                                            Session.GetHabbo().MimicDisabled = !Session.GetHabbo().MimicDisabled;
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 158:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.RoomData.GuildId != 0)
                                                        {
                                                            Groups.RemoveGroup(Groups.GetRoomGroup(Session.GetHabbo().CurrentRoomId));
                                                            Essential.GetGame().GetRoomManager().method_16(Session.GetHabbo().CurrentRoom);
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 159:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {
                                                            Session.GetHabbo().CurrentRoom.CanWalkUnder = true;
                                                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                            {
                                                                dbClient.ExecuteQuery("UPDATE rooms SET can_walkunder='1' WHERE id=" + Session.GetHabbo().CurrentRoomId);
                                                            }
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 160:
                                                    {
                                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                                        {
                                                            Session.GetHabbo().CurrentRoom.CanWalkUnder = false;
                                                            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                            {
                                                                dbClient.ExecuteQuery("UPDATE rooms SET can_walkunder='0' WHERE id=" + Session.GetHabbo().CurrentRoomId);
                                                            }
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 161:
                                                    {
                                                        if(Session.GetHabbo().Rank >= int.Parse(config.getData("cmd.aws.minrank")))
                                                        {
                                                            
                                                                if(!Params[1].Contains(".") && !Params[1].ToLower().Contains("www") && !Params[1].ToLower().Contains("http"))
                                                                {
                                                                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                                    {
                                                                        dbClient.AddParamWithValue("word", Params[1].ToLower());
                                                                        if (dbClient.ReadInt32("SELECT COUNT(*) FROM wordfilter WHERE word=@word") == 0)
                                                                        {
                                                                            dbClient.ExecuteQuery("INSERT INTO wordfilter (`word`,`replacement`,`strict`) VALUES (@word,'***','1');");
                                                                            Essential.GetAntiAd().Refresh();
                                                                            Session.GetHabbo().Whisper("Wort erfolgreich hinzugefügt!");
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Das Wort existiert bereits im Wortfilter.");
                                                                            return true;
                                                                        }
                                                                    }
                                                                }
                                                                Session.GetHabbo().Whisper("Das Wort enthält entweder \"http\" ,  \"www\" oder einen Punkt!");
                                                                return true;
                                                        }
                                                        return false;
                                                    }
                                                case 162:
                                                    {
                                                        RoomUser SessionUser;
                                                        GameClient User2 = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                        SessionUser = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        RoomUser TargerUser = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                        if (Math.Abs(SessionUser.X - TargerUser.X) < 3 && Math.Abs(SessionUser.Y - TargerUser.Y) < 3)
                                                        {

                                                            SessionUser.HandleSpeech(Session, "*umarmt " + User2.GetHabbo().Username + " *", false);
                                                            SessionUser.Kiss();
                                                            TargerUser.Kiss();
                                                        }
                                                        else
                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                        return true;
                                                    }
                                                case 163:
                                                    {
                                                        RoomUser sessionUser = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        GameClient targetGC = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                        RoomUser targetRoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(targetGC.GetHabbo().Id);
                                                        
                                                        return true;
                                                    }
                                                case 164:
                                                    {
                                                        return false;
                                                        try {
                                                            switch (Params[1].ToLower())
                                                            {
                                                                case "definecredits":
                                                                    {
                                                                        if (Session.GetHabbo().Rank >= uint.Parse(config.getData("cmd.eventwin.minrank.define")))
                                                                        {
                                                                            File.WriteAllText("commands.conf", File.ReadAllText("commands.conf").Replace("cmd.eventwin.credits=" + config.getData("cmd.eventwin.credits"), "cmd.eventwin.credits=" + int.Parse(Params[2])));
                                                                            config = new Configuration();
                                                                            Essential.GetGame().GetRoleManager().UpdateConfiguration();
                                                                        }
                                                                        return true;
                                                                    }
                                                                case "defineduckets":
                                                                    {
                                                                        if (Session.GetHabbo().Rank >= uint.Parse(config.getData("cmd.eventwin.minrank.define")))
                                                                        {
                                                                            File.WriteAllText("commands.conf", File.ReadAllText("commands.conf").Replace("cmd.eventwin.duckets=" + config.getData("cmd.eventwin.duckets"), "cmd.eventwin.duckets=" + int.Parse(Params[2])));
                                                                            config = new Configuration();
                                                                            Essential.GetGame().GetRoleManager().UpdateConfiguration();
                                                                        }
                                                                        return true;
                                                                    }
                                                                case "duckets":
                                                                    {
                                                                        if (Session.GetHabbo().Rank >= uint.Parse(config.getData("cmd.eventwin.minrank")))
                                                                        {
                                                                            Console.WriteLine(Params[2]);
                                                                            GameClient TargtUser = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[2]);
                                                                            if (TargtUser != null && TargtUser.GetHabbo() != null && TargtUser.GetHabbo().Username != Session.GetHabbo().Username && TargtUser.GetHabbo().EventWinLast + 900 >= Essential.GetUnixTimestamp())
                                                                            {
                                                                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                                                {
                                                                                    TargtUser.GetHabbo().EventWinLast = Essential.GetUnixTimestamp();
                                                                                    dbClient.ExecuteQuery("UPDATE users SET eventwin_last='" + TargtUser.GetHabbo().EventWinLast + "' WHERE Id=" + TargtUser.GetHabbo().Id);
                                                                                    TargtUser.GetHabbo().ActivityPoints += int.Parse(config.getData("cmd.eventwin.duckets"));
                                                                                    TargtUser.GetHabbo().UpdateActivityPoints(true);
                                                                                    TargtUser.SendNotification("Du hast " + int.Parse(config.getData("cmd.eventwin.duckets")) + " Duckets erhalten.");
                                                                                    Session.Whisper("Du hast dem User " + TargtUser.GetHabbo().Username + " " + int.Parse(config.getData("cmd.eventwin.duckets")) + " Duckets gegeben.");
                                                                                }
                                                                            }
                                                                        }
                                                                        return true;
                                                                    }
                                                                case "coins":
                                                                    {
                                                                        if (Session.GetHabbo().Rank >= uint.Parse(config.getData("cmd.eventwin.minrank")))
                                                                        {
                                                                            GameClient TargtUser = Essential.GetGame().GetClientManager().GetClientByHabbo(Params[2]);
                                                                            if (TargtUser != null && TargtUser.GetHabbo() != null && TargtUser.GetHabbo().Username != Session.GetHabbo().Username && TargtUser.GetHabbo().EventWinLast + 900 >= Essential.GetUnixTimestamp())
                                                                            {
                                                                                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                                                                                {
                                                                                    TargtUser.GetHabbo().EventWinLast = Essential.GetUnixTimestamp();
                                                                                    dbClient.ExecuteQuery("UPDATE users SET eventwin_last='" + TargtUser.GetHabbo().EventWinLast + "' WHERE Id=" + TargtUser.GetHabbo().Id);
                                                                                    TargtUser.GetHabbo().GiveCredits(int.Parse(config.getData("cmd.eventwin.credits")),"Command (Eventwin)",Session.GetHabbo().Username);
                                                                                    TargtUser.GetHabbo().UpdateCredits(true);
                                                                                }
                                                                            }
                                                                        }
                                                                        return true;
                                                                    }
                                                            }
                                                        }
                                                        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                                                        return false;
                                                    }
                                                default:
                                                    goto IL_3F91;
                                            }
                                    }
                                    try
                                    {
                                        if (!(ServerConfiguration.UnknownBoolean1 || Session.GetHabbo().HasFuse("cmd_push")))
                                        {
                                            Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_disabled"));
                                            return true;
                                        }
                                        if (!Session.GetHabbo().CurrentRoom.CanPush)
                                            return false;
                                        if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_push")))
                                        {
                                            Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_permission_vip"));
                                            return true;
                                        }
                                        string a = "down";
                                        string text = Params[1];
                                        TargetClient = Essential.GetGame().GetClientManager().GetClientByHabbo(text);
                                        class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                        if (Session == null || TargetClient == null)
                                        {
                                            return false;
                                        }
                                        class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                        RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                        if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                        {
                                            Session.GetHabbo().Whisper("Du kannst dich nicht schubsen. Es ist unmöglich :)");
                                            return true;
                                        }
                                        bool arg_3DD2_0;
                                        if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                        {
                                            if ((class6.X + 1 != class4.X || class6.Y != class4.Y) && (class6.X - 1 != class4.X || class6.Y != class4.Y) && (class6.Y + 1 != class4.Y || class6.X != class4.X))
                                            {
                                                if (class6.Y - 1 == class4.Y)
                                                {
                                                    if (class6.X == class4.X)
                                                    {
                                                        goto IL_3DA6;
                                                    }
                                                }
                                                arg_3DD2_0 = (class6.X != class4.X || class6.Y != class4.Y);
                                                goto IL_3DD2;
                                            }
                                        IL_3DA6:
                                            arg_3DD2_0 = false;
                                        }
                                        else
                                        {
                                            arg_3DD2_0 = true;
                                        }
                                    IL_3DD2:
                                        if (!arg_3DD2_0)
                                        {
                                            class6.HandleSpeech(Session, "*schubst " + TargetClient.GetHabbo().Username + "*", false);
                                            if (class6.BodyRotation == 0)
                                            {
                                                a = "up";
                                            }
                                            if (class6.BodyRotation == 2)
                                            {
                                                a = "right";
                                            }
                                            if (class6.BodyRotation == 4)
                                            {
                                                a = "down";
                                            }
                                            if (class6.BodyRotation == 6)
                                            {
                                                a = "left";
                                            }
                                            if (a == "up")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if ((!(class4.X == class2.RoomModel.DoorX && class4.Y - 1 == class2.RoomModel.DoorY)) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                    {
                                                        class4.MoveTo(class4.X, class4.Y - 1);
                                                    }
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.X, class4.Y - 1);
                                                }
                                            }
                                            if (a == "right")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (!(class4.X + 1 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                        class4.MoveTo(class4.X + 1, class4.Y);
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.X + 1, class4.Y);
                                                }
                                            }
                                            if (a == "down")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (!(class4.X == class2.RoomModel.DoorX && class4.Y + 1 == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                        class4.MoveTo(class4.X, class4.Y + 1);
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.X, class4.Y + 1);
                                                }
                                            }
                                            if (a == "left")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (!(class4.X - 1 == class2.RoomModel.DoorX && class4.Y == class2.RoomModel.DoorY) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                        class4.MoveTo(class4.X - 1, class4.Y);
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.X - 1, class4.Y);
                                                }
                                            }
                                        }
                                        return true;
                                    }
                                    catch
                                    {
                                        return false;
                                    }
                                IL_3F03:
                                    class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    if (class6.Boolean_3)
                                    {
                                        Session.GetHabbo().Whisper("Befehl während einem Tausch nicht möglich!");
                                        return true;
                                    }
                                    if (ServerConfiguration.EnableRedeemCredits)
                                    {
                                        Session.GetHabbo().GetInventoryComponent().ConvertCoinsToCredits();
                                    }
                                    else
                                    {
                                        Session.GetHabbo().Whisper(EssentialEnvironment.GetExternalText("cmd_error_disabled"));
                                    }
                                    return true;
                                }
                        }
                    }
                IL_2F05:
                    try
                    {
                        DateTime now = DateTime.Now;
                        TimeSpan timeSpan = now - Essential.ServerStarted;
                        int clients = Essential.GetGame().GetClientManager().ClientCount;
                        int rooms = Essential.GetGame().GetRoomManager().LoadedRoomsCount;
                        string text10 = "";
                        if (ServerConfiguration.ShowUsersAndRoomsInAbout)
                        {
                            text10 = string.Concat(new object[]
						{
							"\nHabbos online: ",
							clients,
							"\nOffene Räume: ",
							rooms
						});
                        }
                        Session.AboutMessage(string.Concat(new object[]
					    {
						    "Online seit: ",
						    timeSpan.Days,
						    " Tage, ",
						    timeSpan.Hours,
						    " Stunden und ",
						    timeSpan.Minutes,
						    " Minuten",
						    text10,
                            "\n\n\nPowered by Essential 5. Only for Habbo.TL"
					    }), "http://rootkit.ch");
                        // no one cares who made this shit anyways. So I didn't give Credits for anyone, neither me or Aaron.
                    }
                    catch { }
                    return true;
                IL_3F91: ;
                }
                catch { /*throws Exception everytime an argument isn't set!*/}
                return false;
            }
        }
        public static string MergeParams(string[] Params, int Start)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Params.Length; i++)
            {
                if (i >= Start)
                {
                    if (i > Start)
                    {
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(Params[i]);
                }
            }
            return stringBuilder.ToString();
        }
    }
}