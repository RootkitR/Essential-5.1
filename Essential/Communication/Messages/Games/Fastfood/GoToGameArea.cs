using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Games;
using System.Data;
using Essential.Storage;
using System.Collections.Generic;

namespace Essential.Communication.Messages.Games
{
    internal sealed class GoToGameArea : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            GameLobby Lobby = Essential.GetGame().GetGamesManager().GetWaitingLobby();
            List<string> UserBadges = new List<string>();
            int Bigparachutes = 0;
            int Missiles = 0;
            int Shields = 0;
            string Username = "Anonymous";

            if (Lobby == null)
            {
                Lobby = Essential.GetGame().GetGamesManager().CreateLobby();
            }

              using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
			{
                Username = dbClient.ReadString("SELECT username FROM users WHERE id = '" + Session.Basejump_UserId + "' LIMIT 1");
            
                DataTable Table = dbClient.ReadDataTable("SELECT * FROM user_badges WHERE user_id = '" + Session.Basejump_UserId + "'");

               
                    DataRow PuRow = dbClient.ReadDataRow("SELECT * FROM basejump_users_powerups WHERE user_id = '" + Session.Basejump_UserId + "' LIMIT 1;");

                    Bigparachutes = int.Parse(PuRow["bigparachutes"].ToString());

                    Missiles = int.Parse(PuRow["missiles"].ToString());

                    Shields = int.Parse(PuRow["shields"].ToString());


                foreach (DataRow Badge in Table.Rows)
                {
                    if (int.Parse(Badge["badge_slot"].ToString()) > 0)
                    {
                        UserBadges.Add(Badge["badge_id"].ToString());
                    }

                }

              }

              Essential.GetGame().GetGamesManager().AddUserToLobby(Lobby.LobbyId, Username, Session.Basejump_UserId, UserBadges, Session);
              Session.Basejump_LobbyId = Lobby.LobbyId;
            if (Essential.GetGame().GetGamesManager().CheckIsPlayersReady(Lobby.LobbyId))
            {
                Essential.GetGame().GetGamesManager().SendPlayMessageToLobby(Lobby.LobbyId);
            }
        

            

        }
    }
}