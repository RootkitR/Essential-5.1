using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Games
{
    class GameLobby
    {
        internal int LobbyId;
        internal List<GamePlayer> Players = new List<GamePlayer>();
        internal double StartStamp;
        internal bool Started;

        public GameLobby(int LobbyId, double StartStamp)
        {
            this.LobbyId = LobbyId;
            this.StartStamp = StartStamp;
            this.Started = false;
        }
    }
}
