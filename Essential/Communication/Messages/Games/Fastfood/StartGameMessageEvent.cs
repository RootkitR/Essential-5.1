using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System.Data;

namespace Essential.Communication.Messages.Games
{
    internal sealed class StartGameMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int GameId = Event.PopWiredInt32();

            string UserAuthTicket = Essential.GetGame().GetGamesManager().CreateAuthTicket(Session.GetHabbo().Id);

            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                // Check exist auth tokens and delete them.

                if (dbClient.ReadInt32("SELECT COUNT(token) FROM basejump_auth_tokens WHERE user_id = '" + Session.GetHabbo().Id + "' LIMIT 1") == 1)
                {
                    dbClient.ExecuteQuery("DELETE FROM `basejump_auth_tokens` WHERE `user_id`='" + Session.GetHabbo().Id + "'");
                }

                dbClient.AddParamWithValue("auth_token", UserAuthTicket);
                dbClient.AddParamWithValue("user_id", Session.GetHabbo().Id);
                dbClient.AddParamWithValue("expire", Essential.GetUnixTimestamp() + 1800); // +30 min expires

                dbClient.ExecuteQuery("INSERT INTO `basejump_auth_tokens` (`token`, `user_id`, `expire`) VALUES (@auth_token, @user_id, @expire)");
            }

            ServerMessage LoadGame = new ServerMessage(Outgoing.LoadGame);
            LoadGame.AppendInt32(GameId);
            LoadGame.AppendString(Session.GetHabbo().Id.ToString()); // ??
            LoadGame.AppendString(Session.ClientVar + "/games/BaseJump.swf"); // url
            LoadGame.AppendString("best");
            LoadGame.AppendString("showAll");
            LoadGame.AppendInt32(60);
            LoadGame.AppendInt32(10);
            LoadGame.AppendInt32(0);
            LoadGame.AppendInt32(6);
            LoadGame.AppendString("assetUrl");
            LoadGame.AppendString(Session.ClientVar + "/games/BasicAssets.swf");
            LoadGame.AppendString("habboHost");
            LoadGame.AppendString("http://fusees-private-httpd-fe-1");
            LoadGame.AppendString("accessToken");
            LoadGame.AppendString(UserAuthTicket);
            LoadGame.AppendString("gameServerHost");
            LoadGame.AppendString(Essential.GetConfig().data["game.tcp.bindip"]);
            LoadGame.AppendString("gameServerPort");
            LoadGame.AppendString(Essential.GetConfig().data["game.tcp.port"]);
            LoadGame.AppendString("socketPolicyPort");
            LoadGame.AppendString(Essential.GetConfig().data["game.tcp.port"]);
            Session.SendMessage(LoadGame);

        }
    }
}