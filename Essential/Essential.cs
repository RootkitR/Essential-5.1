using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Essential.Core;
using Essential.HabboHotel;
using Essential.Net;
using Essential.Storage;
using Essential.Util;
using Essential.Communication;
using Essential.Messages;
using System.Net;
using System.IO;
using System.Data;
using System.Collections;
using Essential.Websockets;
using System.Drawing;
using System.Drawing.Imaging;
using Essential.Web;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.AntiAd;
using Essential.HabboHotel.Users;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Users.UserDataManagement;
using System.Text.RegularExpressions;
using Essential.Communication.Messages;
using System.Reflection;
using Essential.HabboHotel.Mobile;
namespace Essential
{
    internal sealed class Essential
    {
        public static readonly int build = typeof(Essential).Assembly.GetName().Version.Build;
        public const string string_0 = "localhost";

        private static PacketManager PacketManager;

        private static ConfigurationData Configuration;

        private static DatabaseManager DatabaseManager;

        private static SocketsManager SocketsManager;
        //private static ConnectionHandeling ConnectionManage;
        private static MusListener MusListener;

        private static Game Internal_Game;

        internal static DateTime ServerStarted;
        public static bool bool_0 = false;
        public static int  int_1 = 0;
        public static int  int_2 = 0;
        public static string string_5 = null;

        private static bool bool_1 = false;
        public static string HeadImagerURL = "http://swf.habbo.tl/habbo-imaging/head.png?figure=";
        public static WebSocketServerManager webSocketServerManager;
        public static ConsoleWriter consoleWriter;
        public static WebManager webManager;
        public static AntiAd antiAdSystem;
        public static string SWFDirectory = "http://swf.habbo.tl/hof_furni/";
        private static MobileHandler mhandler;
        public static long Build
        {
            get
            {
                return typeof(Essential).Assembly.GetName().Version.Build;
            }
        }
        public static string Creator
        {
            get
            {
                return "Rootkit";
            }
        }
        public static string PrettyVersion
        {
            get
            {
                return "Essential Build " + Build;
            }
        }

        internal static Game Game
        {
            get
            {
                return Essential.Internal_Game;
            }
            set
            {
                Essential.Internal_Game = value;
            }
        }


        public static PacketManager GetPacketManager()
        {
            return Essential.PacketManager;
        }

        public static ConfigurationData GetConfig()
        {
            return Configuration;
        }

        public static DatabaseManager GetDatabase()
        {
            return DatabaseManager;
        }

        public static Encoding GetDefaultEncoding()
        {
            return Encoding.Default;
        }

        public static SocketsManager GetSocketsManager()
        {
            return Essential.SocketsManager;
        }

        //public static ConnectionHandeling smethod_14()
        //{
        //    return Essential.ConnectionManage;
        //}
        public static MobileHandler GetMobileHandler()
        {
            return Essential.mhandler;
        }
        internal static Game GetGame()
        {
            return Internal_Game;
        }

        public static string smethod_0(string string_8)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = Encoding.UTF8.GetBytes(string_8);
            array = mD5CryptoServiceProvider.ComputeHash(array);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2").ToLower());
            }
            string text = stringBuilder.ToString();
            return text.ToUpper();
        }

        public static string smethod_1(string string_8)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(string_8);
            byte[] array = new SHA1Managed().ComputeHash(bytes);
            string text = string.Empty;
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                text += b.ToString("X2");
            }
            return text;
        }
        public void Initialize()
        {
            Essential.consoleWriter = new ConsoleWriter(Console.Out);
            Console.SetOut(Essential.consoleWriter);
            try
            {
                Console.WindowWidth = 130;
                Console.WindowHeight = 36;
            }
            catch { }
            Essential.ServerStarted = DateTime.Now;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(@"                                          ______                    _   _       _ ");
            Console.WriteLine(@"                                         |  ____|                  | | (_)     | |");
            Console.WriteLine(@"                                         | |__   ___ ___  ___ _ __ | |_ _  __ _| |");
            Console.WriteLine(@"                                         |  __| / __/ __|/ _ \ '_ \| __| |/ _` | |");
            Console.WriteLine(@"                                         | |____\__ \__ \  __/ | | | |_| | (_| | |");
            Console.WriteLine(@"                                         |______|___/___/\___|_| |_|\__|_|\__,_|_|");
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                         Essential Emulator Build " + Build + " by " + Creator);
            Console.WriteLine();
            Console.WriteLine("                         Credits to: Meth0d (Uber), Sojobo (Phoenix), Juniori (GTE) & Rootkit (Essential)");
            Console.WriteLine();
            Console.ResetColor();
            try
            {
                Essential.Configuration = new ConfigurationData("config.conf");
                DateTime now = DateTime.Now;
                try
                {
                    Essential.SWFDirectory = Essential.GetConfig().data["web.api.furni.hof_furni"];
                }
                catch { }
                if (!Directory.Exists("API"))
                    Directory.CreateDirectory("API");

                DatabaseServer dbServer = new DatabaseServer(Essential.GetConfig().data["db.hostname"], uint.Parse(Essential.GetConfig().data["db.port"]), Essential.GetConfig().data["db.username"], Essential.GetConfig().data["db.password"]);
                Database database = new Database(Essential.GetConfig().data["db.name"], uint.Parse(Essential.GetConfig().data["db.pool.minsize"]), uint.Parse(Essential.GetConfig().data["db.pool.maxsize"]));
                Essential.DatabaseManager = new DatabaseManager(dbServer, database);
                GroupsPartsData.InitGroups();
                try
                {
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery("SET @@global.sql_mode= '';");
                        dbClient.ExecuteQuery("UPDATE users SET online = '0'");
                        dbClient.ExecuteQuery("UPDATE rooms SET users_now = '0'");
                    }
                    Essential.Internal_Game.ContinueLoading();
                }
                catch { }

                Essential.Internal_Game = new Game(int.Parse(Essential.GetConfig().data["game.tcp.conlimit"]));

                Essential.PacketManager = new PacketManager();
                Essential.PacketManager.Load();
                Essential.mhandler = new MobileHandler();
                Essential.mhandler.Load();
                Console.WriteLine(Essential.PacketManager.Count + " Packets loaded!");
                Essential.antiAdSystem = new AntiAd();
                Essential.MusListener = new MusListener(Essential.GetConfig().data["mus.tcp.bindip"], int.Parse(Essential.GetConfig().data["mus.tcp.port"]), Essential.GetConfig().data["mus.tcp.allowedaddr"].Split(new char[] { ';' }), 20);
                Essential.SocketsManager = new SocketsManager(Essential.GetConfig().data["game.tcp.bindip"], int.Parse(Essential.GetConfig().data["game.tcp.port"]), int.Parse(Essential.GetConfig().data["game.tcp.conlimit"]));
                //ConnectionManage = new ConnectionHandeling(Essential.GetConfig().data["game.tcp.port"], int.Parse(Essential.GetConfig().data["game.tcp.conlimit"]), int.Parse(Essential.GetConfig().data["game.tcp.conlimit"]), true);
               Essential.HeadImagerURL = Essential.GetConfig().data["eventstream.imager.url"];
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery("UPDATE server_status SET bannerdata='" + EssentialEnvironment.globalCrypto.Prime + ":" + EssentialEnvironment.globalCrypto.Generator + "';");
                }
                Essential.SocketsManager.method_3().method_0();
                webSocketServerManager = new WebSocketServerManager(Essential.GetConfig().data["websocket.url"]);
                Console.WriteLine("Server started at " + Essential.GetConfig().data["websocket.url"]);
                webManager = new WebManager();
                TimeSpan timeSpan = DateTime.Now - now;
                Logging.WriteLine(string.Concat(new object[]
				    {
					    "Server -> READY! (",
					    timeSpan.Seconds,
					    " s, ",
					    timeSpan.Milliseconds,
					    " ms)"
				    }));
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery("UPDATE server_status SET server_started='" + Convert.ToInt32(GetUnixTimestamp()) + "'");
                }
                Console.Beep();
            }
            catch (KeyNotFoundException KeyNotFoundException)
            {
                Logging.WriteLine("Failed to boot, key not found: " + KeyNotFoundException);
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                Essential.Destroy();
            }
            catch (InvalidOperationException ex)
            {
                Logging.WriteLine("Failed to initialize EssentialEmulator: " + ex.Message);
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                Essential.Destroy();
            }
        }
        public static void InitWebsocketManager()
        {
            webSocketServerManager = new WebSocketServerManager(Essential.GetConfig().data["websocket.url"]);
        }
        public static int  StringToInt(string str)
        {
            return Convert.ToInt32(str);
        }

        public static bool StringToBoolean(string str)
        {
            return (str == "1" || str == "true");
        }

        public static string BooleanToString(bool b)
        {
            if (b)
                return "1";
            else
                return "0";
        }

        public static int smethod_5(int int_3, int int_4)
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] array = new byte[4];
            rNGCryptoServiceProvider.GetBytes(array);
            int seed = BitConverter.ToInt32(array, 0);
            return new Random(seed).Next(int_3, int_4 + 1);
        }
        public static int GetRandomNumber(int min, int max)
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] array = new byte[4];
            rNGCryptoServiceProvider.GetBytes(array);
            int seed = BitConverter.ToInt32(array, 0);
            return new Random(seed).Next(min, max + 1);
        }
        public static double GetUnixTimestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public static string FilterString(string str)
        {
            return DoFilter(str, false, false);
        }

        public static string DoFilter(string Input, bool bool_2, bool bool_3)
        {
            Input = Input.Replace(Convert.ToChar(1), ' ');
            Input = Input.Replace(Convert.ToChar(2), ' ');
            Input = Input.Replace(Convert.ToChar(9), ' ');
            if (!bool_2)
            {
                Input = Input.Replace(Convert.ToChar(13), ' ');
            }
            if (bool_3)
            {
                Input = Input.Replace('\'', ' ');
            }
            return Input;
        }
        public static WebManager GetWebManager()
        {
            return Essential.webManager;
        }
        public static bool smethod_9(string string_8)
        {
            if (string.IsNullOrEmpty(string_8))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < string_8.Length; i++)
                {
                    if (!char.IsLetter(string_8[i]) && !char.IsNumber(string_8[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static void Destroy()
        {
            Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);
            Logging.WriteLine("Destroying EssentialEmu environment...");
            if (Essential.GetGame() != null)
            {
                Essential.GetGame().ContinueLoading();
                Essential.Internal_Game = null;
            }
            if (Essential.GetSocketsManager() != null)
            {
                Logging.WriteLine("Destroying connection manager.");
                Essential.GetSocketsManager().method_3().method_2();
                //Essential.smethod_14().Destroy();
                Essential.GetSocketsManager().method_0();
                Essential.SocketsManager = null;
            }
            if (Essential.GetDatabase() != null)
            {
                try
                {
                    Logging.WriteLine("Destroying database manager.");
                    MySqlConnection.ClearAllPools();
                    Essential.DatabaseManager = null;
                }
                catch
                {
                }
            }
            Logging.WriteLine("Uninitialized successfully. Closing.");
        }

        internal static void smethod_17(string string_8)
        {
            try
            {
                ServerMessage Message = new ServerMessage(Outgoing.BroadcastMessage);
                Message.AppendStringWithBreak(string_8);
                Essential.GetGame().GetClientManager().BroadcastMessage(Message);
            }
            catch
            {
            }
        }

        internal static void Close()
        {
            Essential.Destroy("", true);
        }

        internal static void Destroy(string string_8, bool ExitWhenDone, bool waitExit = false)
        {
            Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);
            try
            {
                Internal_Game.StopGameLoop();
            }
            catch { }
            try
            {
                if (Essential.GetPacketManager() != null)
                {
                    Essential.GetPacketManager().Clear();
                }
            }
            catch { }
            if (string_8 != "")
            {
                if (Essential.bool_1)
                {
                    return;
                }
                Logging.Disable();
                Essential.smethod_17("ATTENTION:\r\nThe server is shutting down. All furniture placed in rooms/traded/bought after this message is on your own responsibillity.");
                Essential.bool_1 = true;
                Console.WriteLine("Server shutting down...");
                try
                {
                    Essential.Internal_Game.GetRoomManager().method_4();
                }
                catch
                {
                }
                try
                {
                    Essential.GetSocketsManager().method_3().method_1();
                    //Essential.smethod_14().Destroy();
                    Essential.GetGame().GetClientManager().CloseAll();
                }
                catch
                {
                }
                try
                {
                    Console.WriteLine("Destroying database manager.");
                    MySqlConnection.ClearAllPools();
                    Essential.DatabaseManager = null;
                }
                catch
                {
                }
                Console.WriteLine("System disposed, goodbye!");
            }
            else
            {
                Logging.Disable();
                Essential.bool_1 = true;
                try
                {
                    if (Essential.Internal_Game != null && Essential.Internal_Game.GetRoomManager() != null)
                    {
                        Essential.Internal_Game.GetRoomManager().UnloadAllRooms();
                        Essential.Internal_Game.GetRoomManager().method_4();
                    }
                }
                catch
                {
                }
                try
                {
                    if (Essential.GetSocketsManager() != null)
                    {
                        Essential.GetSocketsManager().method_3().method_1();
                        //Essential.smethod_14().Destroy();
                        Essential.GetGame().GetClientManager().CloseAll();
                    }
                }
                catch
                {
                }
                if (SocketsManager != null)
                {
                    //Essential.ConnectionManage.method_7();
                }
                if (Essential.Internal_Game != null)
                {
                    Essential.Internal_Game.ContinueLoading();
                }
                Console.WriteLine(string_8);
            }
            if (ExitWhenDone)
            {
                if (waitExit)
                {
                    Console.WriteLine("Press any key to exit..");
                    Console.ReadKey();
                }

                Environment.Exit(0);
            }
        }

        public static bool CanBeDividedBy(int i, int j)
        {
            return i % j == 0;
        }

        public static DateTime TimestampToDate(double timestamp)
        {
            DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return result.AddSeconds(timestamp).ToLocalTime();
        }


        public static int[] IntToArray(string numbers)
        {
            string[] ColorList = numbers.Split(new char[]
			{
				'|'
			});
            var digits = new List<int>();
            if (ColorList.Count() > 1)
            {
                for (int i = 0; i < ColorList.Count(); i++)
                {
                    digits.Add(int.Parse(ColorList[i]));
                }
            }
            else
            {
                digits.Add(int.Parse(ColorList[0]));
            }
            var arr = digits.ToArray();
            Array.Reverse(arr);
            return arr;
        }

        public static void RainbowText(string text, int[] colors, int color, int interval, int count, int maxcount, bool randomcolors, int lastcolor)
        {
            if (count > maxcount)
            {
                Console.ResetColor();
                Console.Write("\r{0}   ", text);
                Console.WriteLine();
                return;
            }
            count++;
            if (randomcolors)
            {
                Random random = new Random();
                int randomcolor = random.Next(1, 15);

                while (lastcolor == randomcolor || randomcolor == 0)
                {
                    randomcolor = random.Next(1, 15);
                }

                color = randomcolor;
            }
            else
            {
                if (colors.Count() > 1)
                {
                    if (!(color >= 0 && color <= 15))
                    {
                        color = 0;
                    }

                    while (!colors.Contains(color) || lastcolor == color || (!(color >= 0 && color <= 15)))
                    {
                        color++;

                        if (!(color >= 0 && color <= 15))
                        {
                            color = 0;
                        }
                    }
                }
                else
                {
                    color = colors[1];
                }
            }
            if (color > 0 && color <= 15)
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.Write("\r{0}   ", text);
                Console.ResetColor();
                lastcolor = color;
            }
            System.Threading.Thread.Sleep(interval);
            RainbowText(text, colors, color, interval, count, maxcount, randomcolors, lastcolor);
        }

        public static bool DoYouWantContinue(string message)
        {
            Console.WriteLine(message);
            ConsoleKeyInfo ConsoleKeyInfo = Console.ReadKey();
            if (ConsoleKeyInfo.Key == ConsoleKey.Y)
            {
                return true;
            }
            else if (ConsoleKeyInfo.Key == ConsoleKey.N)
            {
                return false;
            }
            else
            {
                DoYouWantContinue(message);
            }

            return false;
        }


        private bool DownloadNewVersion()
        {
            return false;
        }
        public static WebSocketServerManager getWebSocketManager()
        {
            return Essential.webSocketServerManager;
        }
        public static ConsoleWriter GetConsoleWriter()
        {
            return Essential.consoleWriter;
        }
        public static AntiAd GetAntiAd()
        {
            return Essential.antiAdSystem;
        }
        public static bool IsValidName(string username)
        {
            if (username.Length < 3 || username.Length > 15)
                return false;
            if (username.Contains(" "))
                return false;
            if (!Regex.IsMatch(string_0, "^[-a-zA-Z0-9._:,]+$"))
                return false;
            if (username != ChatCommandHandler.ApplyFilter(username))
                return false;
            return true;
        }
    }
}
