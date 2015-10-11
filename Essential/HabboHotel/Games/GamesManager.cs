using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
using Essential.Core;
using System.Security.Cryptography;
using Essential.HabboHotel.GameClients;
using System.Timers;

namespace Essential.HabboHotel.Games
{
    internal sealed class GamesManager
    {
        public List<GameLocale> GameLocales = new List<GameLocale>();
        public Dictionary<int, GameLobby> Lobbys = new Dictionary<int, GameLobby>();
        public Dictionary<int, PowerupPackage> PowerupPackages = new Dictionary<int, PowerupPackage>();

        public void LoadGameLocales(DatabaseClient dbClient)
        {
            try
            {
                DataTable Table = dbClient.ReadDataTable("SELECT * FROM basejump_locales");

                GameLocales = new List<GameLocale>();

                foreach (DataRow Locale in Table.Rows)
                {
                    GameLocales.Add(new GameLocale(Locale["lang_id"].ToString(), Locale["lkey"].ToString(), Locale["lvalue"].ToString()));
                }
            }
            catch(Exception e)
            {
                Logging.LogException("GamesManager error in LoadGameLocales. Data for developer: " + e.Data);
              Logging.WriteLine("Error in GamesManager", ConsoleColor.Red);

            }
        }
        static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
        public string CreateAuthTicket(uint UserId)
        {
            string AuthTokenGen = "FF-GAMEMANAGER-" + UserId + EssentialEnvironment.GetRandomNumber(1, 10000) + Essential.GetUnixTimestamp();
            return Hash(AuthTokenGen);
        }
        public void LoadPowerupPackages(DatabaseClient dbClient)
        {
            try
            {
                DataTable Table = dbClient.ReadDataTable("SELECT * FROM basejump_powerup_packages");

                PowerupPackages = new Dictionary<int, PowerupPackage>();

                foreach (DataRow Powerup in Table.Rows)
                {
                    PowerupPackages.Add(int.Parse(Powerup["id"].ToString()), new PowerupPackage(int.Parse(Powerup["id"].ToString()), Powerup["package_name"].ToString(), Powerup["powerup_type"].ToString(), int.Parse(Powerup["cost_credits"].ToString()), int.Parse(Powerup["amount"].ToString())));
                }
            }
            catch (Exception e)
            {
                Logging.LogException("GamesManager error in LoadPowerupPackages. Data for developer: " + e.Data);
                Logging.WriteLine("Error in GamesManager", ConsoleColor.Red);
            }
        }
     
        public List<GameLocale> GetLocales(string LangId)
        {
            List<GameLocale> locca = new List<GameLocale>();

            foreach (GameLocale GLoc in GameLocales)
            {
                if (GLoc.LanguageId == LangId)
                {
                    locca.Add(GLoc);
                }
            }
            return locca;
        }

        internal GameLobby CreateLobby()
        {
               int NlobbId = Lobbys.Count + 1;
               GameLobby NLobby = new GameLobby(NlobbId, Essential.GetUnixTimestamp());
               Lobbys.Add(NlobbId, NLobby);
               return Lobbys[NlobbId];
        }

        internal void AddUserToLobby(int LobbyId, string Username, int UserId, List<string> Badges, GameClient PlayClient)
        {
            Lobbys[LobbyId].Players.Add(new GamePlayer(Username, UserId, 0, LobbyId, Badges, 0, PlayClient));
        }

        private void PlateHandl(object source, ElapsedEventArgs e, GamePlayer Player)
        {
            double kerroin = 0.03;
            if (Player.ShieldStatus == 0)
            {
                kerroin = 0.15;
            }
            double Next = Player.PlateLocation - kerroin;
            Console.WriteLine("Next " + Next);
             foreach (GamePlayer Playeri in Lobbys[Player.LobbyId].Players)
            {
                ServerMessage UpdatePlayer = new ServerMessage(4);
                UpdatePlayer.AppendInt32(Player.CurrentPlate); //plate id(?)
                UpdatePlayer.AppendInt32(Player.UserId); //user id
                UpdatePlayer.AppendString(Next.ToString()); //distance
                UpdatePlayer.AppendString(Player.PlateSpeed.ToString()); //speed
                UpdatePlayer.AppendInt32(Player.ShieldStatus); //state
                UpdatePlayer.AppendBoolean(false); //failed
                Playeri.UClient.SendMessage(UpdatePlayer);
            }
            if (Next < 0.0)
            {
                Console.WriteLine("Pissa");
                UpdatePlate(Player.LobbyId, 2, Player.UserId, 0.00, 0.00, true);
                Player.PlateTimer.Enabled = false;
                Player.PlateLocation = 1.00;
               
               
            }

            Player.PlateLocation = Next;
        }
        internal void EmptyPlate(int LobbyId, int ShieldId, int UserId, int PlateId)
        {
            foreach (GamePlayer Player in Lobbys[LobbyId].Players)
            {
                ServerMessage UpdatePlayer = new ServerMessage(Outgoing.UpdatePlayer);
                UpdatePlayer.AppendInt32(PlateId); //plate id(?)
                UpdatePlayer.AppendInt32(UserId); //user id
                UpdatePlayer.AppendString("1.00"); //distance
                UpdatePlayer.AppendString("0.00"); //speed
                UpdatePlayer.AppendInt32(0); //state
                UpdatePlayer.AppendBoolean(false); //failed
                Player.UClient.SendMessage(UpdatePlayer);
            }
        }
        internal void UpdatePlate(int LobbyId, int ShieldId, int UserId, double Distance, double Speed, bool Failed)
        {
               int ShieldStatue = 0;
               int Stars = 0;
               int CurrentPlate = 1;
               foreach (GamePlayer Player in Lobbys[LobbyId].Players)
            {
             
                if (Player.UserId == UserId)
                {
                    ShieldStatue = Player.ShieldStatus;
                    Stars = Player.Stars;
                    Player.PlateLocation = 1.00;
                    Player.PlateTimer.Enabled = false;
                    if (ShieldStatue == 1)
                    {
                        Player.Stars++;
                        Stars = Player.Stars;
                        CurrentPlate = Player.CurrentPlate + 1;
                        Player.CurrentPlate = CurrentPlate;
                        Player.ShieldStatus = 0;
                        Player.PlateLocation = 1.00;
                        Player.PlateWaiter = Essential.GetUnixTimestamp() + 3;
                    }
                   
                }
            
            }
                   foreach (GamePlayer Playeri in Lobbys[LobbyId].Players)
            {
                ServerMessage UpdatePlayer = new ServerMessage(Outgoing.UpdatePlayer);
            UpdatePlayer.AppendInt32(CurrentPlate); //plate id(?)
            UpdatePlayer.AppendInt32(UserId); //user id
            UpdatePlayer.AppendString(Distance.ToString()); //distance
            UpdatePlayer.AppendString(Speed.ToString()); //speed
            UpdatePlayer.AppendInt32(ShieldStatue == 1 ? 3 : 4); //state
            UpdatePlayer.AppendBoolean(false); //failed
            Playeri.UClient.SendMessage(UpdatePlayer);

                       if (ShieldStatue == 1)
                       {
                        
                           ServerMessage PlateDown = new ServerMessage(Outgoing.PlateDown);
                           PlateDown.AppendInt32(Stars);
                           PlateDown.AppendInt32(Playeri.UserId);
                           PlateDown.AppendInt32(3);
                           PlateDown.AppendInt32(CurrentPlate);
                           PlateDown.AppendInt32(12);
                           Playeri.UClient.SendMessage(PlateDown);
                           
                       }
                       else
                       {
                           ServerMessage PlateDown = new ServerMessage(Outgoing.PlateDown);
                           PlateDown.AppendInt32(Stars);
                           PlateDown.AppendInt32(Playeri.UserId);
                           PlateDown.AppendInt32(3);
                           PlateDown.AppendInt32(CurrentPlate);
                           PlateDown.AppendInt32(12);
                           Playeri.UClient.SendMessage(PlateDown);
                       }
           
            
                       
            }
        }
        internal void ActivateShield(int LobbyId, int ShieldId, int UserId)
        {
            foreach (GamePlayer Player in Lobbys[LobbyId].Players)
            {
                switch (ShieldId)
                {
                    case 0:

                      if (Player.UserId == UserId)
                      {
                          if (Player.PlateWaiter > Essential.GetUnixTimestamp())
                          {
                              EmptyPlate(LobbyId, 0, UserId, Player.CurrentPlate);
                              return;
                          }
                         
                          if (Player.PlateLocation < 0.00)
                          {
                              Player.PlateLocation = 1.00;
                          }
                          Player.PlateTimer.Elapsed += (sender, e) => PlateHandl(sender, e, Player);

                          Player.PlateTimer.Interval = 150;

                          Player.PlateTimer.Enabled = true;
                      }
                    

               ServerMessage UpdatePlayer = new ServerMessage(Outgoing.UpdatePlayer);
                UpdatePlayer.AppendInt32(0); //plate id(?)
                UpdatePlayer.AppendInt32(UserId); //user id
                UpdatePlayer.AppendString("1.0"); //distance
                UpdatePlayer.AppendString("0.0"); //speed
                UpdatePlayer.AppendInt32(1); //state
                UpdatePlayer.AppendBoolean(false); //failed
                Player.UClient.SendMessage(UpdatePlayer);
                        break;

                    case 1:

                     
                        double distanssi = 1.00;

                        if (Player.UserId == UserId)
                        {
                            if (!Player.PlateTimer.Enabled)
                            {
                                return;
                            }

                            Player.PlateTimer.Interval = 350;


                            distanssi = Player.PlateLocation;
                            Player.ShieldStatus = 1;
                        }

                        ServerMessage UpdatePlayer2 = new ServerMessage(Outgoing.UpdatePlayer);
                        UpdatePlayer2.AppendInt32(1); //plate id(?)
                        UpdatePlayer2.AppendInt32(UserId); //user id
                        UpdatePlayer2.AppendString(distanssi.ToString()); //distance
                        UpdatePlayer2.AppendString("-5.9"); //speed
                        UpdatePlayer2.AppendInt32(2); //state
                        UpdatePlayer2.AppendBoolean(false); //failed
                        Player.UClient.SendMessage(UpdatePlayer2);
                        
                        break;

                    case 2:
                        break;

                    case 3:

                        break;

                    case 4:
            ServerMessage ActivateShield = new ServerMessage(Outgoing.ActivateShield);
            ActivateShield.AppendInt32(UserId);
            ActivateShield.AppendInt32(ShieldId);
            ActivateShield.AppendBoolean(true);
            Player.UClient.SendMessage(ActivateShield);
                        break;
                }
            
            }
        }
        internal void SendPlayMessageToLobby(int LobbyId)
        {
            Lobbys[LobbyId].Started = true;
            Lobbys[LobbyId].StartStamp = Essential.GetUnixTimestamp();

            foreach (GamePlayer Player in Lobbys[LobbyId].Players)
            {
                ServerMessage SerializeLobby = new ServerMessage(Outgoing.SerializeLobby);
                SerializeLobby.AppendInt32(Player.UserId); // Player ID
                SerializeLobby.AppendBoolean(true);
                SerializeLobby.AppendBoolean(true);
                SerializeLobby.AppendBoolean(true);
                SerializeLobby.AppendUInt(0);
                SerializeLobby.AppendUInt(6);
                SerializeLobby.AppendUInt(0);
                SerializeLobby.AppendString("0.4");
                SerializeLobby.AppendString("-2.0");
                SerializeLobby.AppendString("0.1");
                SerializeLobby.AppendString("0.2");
                SerializeLobby.AppendUInt(100);
                SerializeLobby.AppendUInt(1);
                SerializeLobby.AppendString("0.5");
                SerializeLobby.AppendString("-2.0");
                SerializeLobby.AppendString("0.1");
                SerializeLobby.AppendString("0.15");
                SerializeLobby.AppendUInt(150);
                SerializeLobby.AppendUInt(2);
                SerializeLobby.AppendString("0.7");
                SerializeLobby.AppendString("-1.2");
                SerializeLobby.AppendString("0.2");
                SerializeLobby.AppendString("0.2");
                SerializeLobby.AppendUInt(100);
                SerializeLobby.AppendUInt(3);
                SerializeLobby.AppendString("0.9");
                SerializeLobby.AppendString("-1.5");
                SerializeLobby.AppendString("0.2");
                SerializeLobby.AppendString("0.2");
                SerializeLobby.AppendUInt(200);
                SerializeLobby.AppendUInt(4);
                SerializeLobby.AppendString("1.1");
                SerializeLobby.AppendString("-1.5");
                SerializeLobby.AppendString("0.15");
                SerializeLobby.AppendString("0.15");
                SerializeLobby.AppendUInt(300);
                SerializeLobby.AppendUInt(5);
                SerializeLobby.AppendString("1.5");
                SerializeLobby.AppendString("-2.0");
                SerializeLobby.AppendString("0.15");
                SerializeLobby.AppendString("0.2");
                SerializeLobby.AppendUInt(200);
                SerializeLobby.AppendUInt(3);
                SerializeLobby.AppendUInt(0);
                SerializeLobby.AppendInt32(Player.UClient.Basejump_Bigparachutes > 3 ? 3 : Player.UClient.Basejump_Bigparachutes);
                SerializeLobby.AppendUInt(1);
                SerializeLobby.AppendInt32(Player.UClient.Basejump_Missiles > 3 ? 3 : Player.UClient.Basejump_Missiles);
                SerializeLobby.AppendUInt(2);
                SerializeLobby.AppendInt32(Player.UClient.Basejump_Shields > 3 ? 3 : Player.UClient.Basejump_Shields);
                SerializeLobby.AppendInt32(Lobbys[LobbyId].Players.Count);
                foreach (GamePlayer AllPlayboy in Lobbys[LobbyId].Players)
                {
                    SerializeLobby.AppendInt32(AllPlayboy.UserId);
                    SerializeLobby.AppendString(AllPlayboy.Username);
                    SerializeLobby.AppendString("http://localhost/getlook.php?figure=" + Essential.GetGame().GetClientManager().GetClientByHabbo(AllPlayboy.Username).GetHabbo().Figure);
                    SerializeLobby.AppendString(Essential.GetGame().GetClientManager().GetClientByHabbo(AllPlayboy.Username).GetHabbo().Gender);
                    SerializeLobby.AppendString("hhfi");
                    SerializeLobby.AppendUInt(1);
                    SerializeLobby.AppendString("BaseJumpMissile");
                    SerializeLobby.AppendInt32(1);
                        SerializeLobby.AppendString("http://localhost/swf/c_images/album1584/ADM.gif");
                }
           /*     SerializeLobby.AppendInt32(2);
                SerializeLobby.AppendString("Testityyppi");
                SerializeLobby.AppendString("http://localhost/swf/games/gamecenter_basejump/avatarimage.png");
                SerializeLobby.AppendString("F");
                SerializeLobby.AppendString("hhfi");
                SerializeLobby.AppendUInt(1);
                SerializeLobby.AppendString("BaseJumpMissile");
                SerializeLobby.AppendUInt(1);
                SerializeLobby.AppendString("http://localhost/swf/c_images/album1584/ADM.gif");
                SerializeLobby.AppendInt32(4);
                SerializeLobby.AppendString("Testityyppi2");
                SerializeLobby.AppendString("http://localhost/swf/games/gamecenter_basejump/avatarimage.png");
                SerializeLobby.AppendString("F");
                SerializeLobby.AppendString("hhfi");
                SerializeLobby.AppendUInt(1);
                SerializeLobby.AppendString("BaseJumpMissile");
                SerializeLobby.AppendUInt(1);
                SerializeLobby.AppendString("http://localhost/swf/c_images/album1584/ADM.gif");
                SerializeLobby.AppendInt32(3);
                SerializeLobby.AppendString("Testityyppi5");
                SerializeLobby.AppendString("http://localhost/swf/games/gamecenter_basejump/avatarimage.png");
                SerializeLobby.AppendString("F");
                SerializeLobby.AppendString("hhfi");
                SerializeLobby.AppendUInt(1);
                SerializeLobby.AppendString("BaseJumpMissile");
                SerializeLobby.AppendUInt(1);
                SerializeLobby.AppendString("http://localhost/swf/c_images/album1584/ADM.gif");
                */
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\filut\testit.txt", true))
                {
                    file.WriteLine(SerializeLobby.ToString());
                }
                Player.UClient.SendMessage(SerializeLobby);
            }
          
        }
        internal bool CheckIsPlayersReady(int LobbyId)
        {
            if (Lobbys[LobbyId].Players.Count == 3)
            {
                return true;
            }
            return false;
        }
        internal GameLobby GetWaitingLobby()
        {
            foreach (GameLobby Lobby in Lobbys.Values)
            {
                if(!Lobby.Started)
                {
                    if (Lobby.Players.Count < 3)
                    {
                        return Lobby;
                    }
                }
            }
            return null;
        }

        internal PowerupPackage GetPowerupPackage(string PackageName)
        {
            foreach (PowerupPackage GPP in PowerupPackages.Values)
            {
                if (GPP.PackageName == PackageName)
                {
                    return GPP;
                }
            }
            return null;
        }
        internal PowerupPackage GetPowerupPackage(int PackageId)
        {
            foreach (PowerupPackage GPP in PowerupPackages.Values)
            {
                if (GPP.Id == PackageId)
                {
                    return GPP;
                }
            }
            return null;
        }
        internal GameLocale GetLocale(string LangId, string LocKey)
        {
            foreach (GameLocale GLoc in GameLocales)
            {
                if (GLoc.LanguageId == LangId && GLoc.LocaleKey == LocKey)
                {
                    return GLoc;
                }
            }
            return null;
        }
    }
}
