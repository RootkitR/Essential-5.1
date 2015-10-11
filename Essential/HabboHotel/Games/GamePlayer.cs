using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essential.HabboHotel.GameClients;
using System.Timers;
namespace Essential.HabboHotel.Games
{
    class GamePlayer
    {
        internal string Username;
        internal int UserId;
        internal int Stars;
        internal int LobbyId;
        internal List<string> Badges;
        internal int Score;
        internal int CurrentPlate = 1;
        internal GameClient UClient;

        internal int ShieldStatus = 0;
        internal double PlateWaiter;
        internal Timer PlateTimer = new Timer();
        internal double PlateLocation = 1.0;
        internal double PlateSpeed = 0.0;
         public GamePlayer(string Username, int UserId, int Stars, int LobbyId, List<string> Badges, int Score, GameClient UClient)
        {
            this.Username = Username;
            this.UserId = UserId;
            this.Stars = Stars;
            this.LobbyId = LobbyId;
            this.Badges = Badges;
            this.Score = 0;
            this.UClient = UClient;
        }

    }
}
