using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Games
{
    internal sealed class GetGameMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int GameId = Event.PopWiredInt32();


            ServerMessage InitGame = new ServerMessage(Outgoing.GameID);
            InitGame.AppendInt32(GameId);
            InitGame.AppendInt32(0);
            Session.SendMessage(InitGame);


           ServerMessage Game2WeeklyLeaderboardEvent = new ServerMessage(Outgoing.Game2WeeklyLeaderboard);
            Game2WeeklyLeaderboardEvent.AppendInt32(DateTime.Now.Year);
            Game2WeeklyLeaderboardEvent.AppendInt32(1); // ??
            Game2WeeklyLeaderboardEvent.AppendInt32(0); // ??
            Game2WeeklyLeaderboardEvent.AppendInt32(0); // ??
            Game2WeeklyLeaderboardEvent.AppendInt32(5231); // ??
            Game2WeeklyLeaderboardEvent.AppendInt32(1); // count:
            // foreach...
            Game2WeeklyLeaderboardEvent.AppendUInt(Session.GetHabbo().Id); // user id
            Game2WeeklyLeaderboardEvent.AppendInt32(DateTime.Now.Year); // time of clasification
            Game2WeeklyLeaderboardEvent.AppendInt32(1); // position
            Game2WeeklyLeaderboardEvent.AppendString(Session.GetHabbo().Username); // username
            Game2WeeklyLeaderboardEvent.AppendString(Session.GetHabbo().Figure);
            Game2WeeklyLeaderboardEvent.AppendString(Session.GetHabbo().Gender.ToLower());

            Game2WeeklyLeaderboardEvent.AppendInt32(3);
            Game2WeeklyLeaderboardEvent.AppendInt32(GameId);
            Session.SendMessage(Game2WeeklyLeaderboardEvent);


            ServerMessage Game2AccountGameStatusMessageEvent = new ServerMessage(Outgoing.Game2AccountGameStatus); 
            Game2AccountGameStatusMessageEvent.AppendInt32(GameId);
            Game2AccountGameStatusMessageEvent.AppendInt32(-1);
            Game2AccountGameStatusMessageEvent.AppendInt32(132);
            Session.SendMessage(Game2AccountGameStatusMessageEvent);

            ServerMessage InitGame2 = new ServerMessage(Outgoing.InitGame);
            InitGame2.AppendInt32(GameId);
            InitGame2.AppendInt32(0);
            Session.SendMessage(InitGame2);


            ServerMessage GameCenterLayoutBestOfWeek = new ServerMessage(Outgoing.BestOfWeek);
            GameCenterLayoutBestOfWeek.AppendInt32(GameId);
            GameCenterLayoutBestOfWeek.AppendInt32(0); // enabled 0 : 1
            // if enabled
            GameCenterLayoutBestOfWeek.AppendString("s");
            GameCenterLayoutBestOfWeek.AppendInt32(4313);
            GameCenterLayoutBestOfWeek.AppendString("200");
            GameCenterLayoutBestOfWeek.AppendInt32(1);
            GameCenterLayoutBestOfWeek.AppendInt32(-1);
            GameCenterLayoutBestOfWeek.AppendBoolean(false);
            //
            GameCenterLayoutBestOfWeek.AppendInt32(5231); // minutos que quedan.
            GameCenterLayoutBestOfWeek.AppendBoolean(false); // enabled??
            Session.SendMessage(GameCenterLayoutBestOfWeek);

        }
    }
}
