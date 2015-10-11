using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Games.SnowWar;
using Essential.HabboHotel.Users;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Games.Snowstorm
{
    class StartPanel : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
           // Session.SendNotification("Sorry. Diese Funktion ist im Moment noch nicht verfügbar! Wir arbeiten daran :)");
            Action<object> action2 = null;
            SnowStorm War = Session.GetClientMessageHandler().CheckAGame();
            if (War == null)
                War = new SnowStorm(Session.GetHabbo());
            if(!War.WarUsers.Contains(Session.GetHabbo()))
                War.WarUsers.Add(Session.GetHabbo());
            ServerMessage message = new ServerMessage(Outgoing.CreateWar);
            message.AppendInt32(-1);
            message.AppendString("SnowStorm level " + War.WarLevel);
            message.AppendInt32(0);
            message.AppendInt32(War.WarLevel);
            message.AppendInt32(2);
            message.AppendInt32(War.MaxUsers);
            message.AppendString(War.WarOwner.Username);
            message.AppendInt32(0x11);
            message.AppendInt32(War.WarUsers.Count);
            foreach(Habbo WarUser in War.WarUsers)
            {
                message.AppendInt32(WarUser.Id);
                message.AppendString(WarUser.Username);
                message.AppendString(WarUser.Figure);
                message.AppendString(WarUser.Gender.ToLower());
                message.AppendInt32(-1);
                message.AppendInt32(WarUser.SnowLevel);
                message.AppendInt32(WarUser.SnowPoints);
                message.AppendInt32(1);
            }
            message.AppendInt32(120);
            Session.SendMessage(message);
            //Session.GetHabbo().SnowUserId = War.WarUsers.Count;
            War.SendToStorm(Session.GetClientMessageHandler().AddToNewGame(War), false, Session.GetHabbo().Id);
            if (War.WarOwner == Session.GetHabbo())
            {
                if (action2 == null)
                {
                    action2 = delegate(object obj)
                    {
                        while(War.Countdown != 0)
                        {
                            War.Countdown--;
                            Thread.Sleep(1000);
                        }
                        try
                        {
                            this.StartGame(War);
                        }catch
                        {
                            foreach(Habbo h in War.WarUsers)
                            {
                                if (h != null && h.GetClient() != null)
                                    h.GetClient().SendNotification("Spiel wurde abgebrochen. Der Ersteller ist offline gegangen!");
                            }
                        }
                    };
                }
                Action<object> action = action2;
                new Task(action, "break").Start();
            }
            ServerMessage packet = new ServerMessage(Outgoing.StartCounter);
            packet.AppendInt32(War.Countdown);
            Session.SendMessage(packet);
        }
        internal void StartGame(SnowStorm War)
        {
            War.AssignTeams();
            War.WarStarted = 1;
            ServerMessage SetStep1 = new ServerMessage(Outgoing.SetStep1);
            SetStep1.AppendInt32(War.WarId);
            SetStep1.AppendString("SnowStorm level " + War.WarLevel);
            SetStep1.AppendInt32(0);
            SetStep1.AppendInt32(War.WarLevel);
            SetStep1.AppendInt32(2);
            SetStep1.AppendInt32(War.MaxUsers);
            SetStep1.AppendString(War.WarOwner.Username);
            SetStep1.AppendInt32(15);
            SetStep1.AppendInt32(War.WarUsers.Count);
            foreach (Habbo habbo in War.WarUsers)
            {
                SetStep1.AppendInt32(habbo.Id);
                SetStep1.AppendString(habbo.Username);
                SetStep1.AppendString(habbo.Figure);
                SetStep1.AppendString((habbo.Gender.ToUpper() == "M") ? "M" : "f");
                SetStep1.AppendInt32(habbo.SnowTeam);
                SetStep1.AppendInt32(habbo.SnowLevel);
                SetStep1.AppendInt32(habbo.SnowPoints);
                SetStep1.AppendInt32(Essential.GetGame().GetStormWars().LevelScore[habbo.SnowLevel]);
            }
            SetStep1.AppendInt32(0);
            SetStep1.AppendInt32(120);
            War.SendToStorm(SetStep1, false, 0);
            ServerMessage EnterArena = new ServerMessage(Outgoing.Game2EnterArenaMessageEvent);
            EnterArena.AppendInt32(0);
            EnterArena.AppendInt32(War.WarLevel);
            EnterArena.AppendInt32(2);
            EnterArena.AppendInt32(War.WarUsers.Count);
            foreach(Habbo habbo in War.WarUsers)
            {
                EnterArena.AppendInt32(habbo.Id);
                EnterArena.AppendString(habbo.Username);
                EnterArena.AppendString(habbo.Figure);
                EnterArena.AppendString(habbo.Gender.ToLower());
                EnterArena.AppendInt32(habbo.SnowTeam);
            }
            EnterArena.AppendInt32(50);
            EnterArena.AppendInt32(50);
            EnterArena.AppendString(War.Model.SerializeHeightMap());
            EnterArena.AppendInt32(War.Model.SnowItems.Count);
            foreach (SnowItems items in War.Model.SnowItems)
            {
                items.SerializeItem(EnterArena);
            }
            War.SendToStorm(EnterArena, false, 0);
            foreach (Habbo habbo in War.WarUsers)
            {
                ServerMessage ArenaEntered = new ServerMessage(Outgoing.Game2ArenaEnteredMessageEvent);
                ArenaEntered.AppendInt32(habbo.Id);
                ArenaEntered.AppendString(habbo.Username);
                ArenaEntered.AppendString(habbo.Figure);
                ArenaEntered.AppendString(habbo.Gender.ToLower());
                ArenaEntered.AppendInt32(habbo.SnowTeam);
                War.SendToStorm(ArenaEntered, false, 0);
            }
            ServerMessage StageLoad = new ServerMessage(Outgoing.Game2StageLoadMessageEvent);
            StageLoad.AppendInt32(0);
            War.SendToStorm(StageLoad, false, 0);
            Action<object> action = delegate(object obj)
            {
                Thread.Sleep(0x1388);
                ServerMessage StageStillLoading = new ServerMessage(Outgoing.Game2StageStillLoadingMessageEvent);
                StageStillLoading.AppendInt32(0);
                StageStillLoading.AppendInt32(0);
                War.SendToStorm(StageStillLoading, false, 0);
                this.LoadArena(War);
            };
            new Task(action, "pingsession").Start();
        }
        internal void LoadArena(SnowStorm War)
        {
            ServerMessage StageStarting = new ServerMessage(Outgoing.Game2StageStartingMessageEvent);
            StageStarting.AppendInt32(0);
            StageStarting.AppendString("snowwar_arena_0");
            StageStarting.AppendInt32(5);
            this.CheckArenaStatic(StageStarting, War);
            int xyz = 0;
            foreach (Habbo habbo in War.WarUsers)
            {
                //habbo.SnowUserId = xyz;
                StageStarting.AppendInt32(5);
                //StageStarting.AppendInt32(0);
                StageStarting.AppendInt32(habbo.SnowUserId);
                StageStarting.AppendInt32(habbo.SnowX);
                StageStarting.AppendInt32(habbo.SnowY);
                StageStarting.AppendInt32((int)(habbo.SnowX / 0xc80));
                StageStarting.AppendInt32((int)(habbo.SnowY / 0xc80));
                StageStarting.AppendInt32(habbo.SnowRot);
                StageStarting.AppendInt32(5);
                StageStarting.AppendInt32(5);
                StageStarting.AppendInt32(0);
                StageStarting.AppendInt32(0);
                StageStarting.AppendInt32(0);
                StageStarting.AppendInt32((int)(habbo.SnowX / 0xc80));
                StageStarting.AppendInt32((int)(habbo.SnowY / 0xc80));
                StageStarting.AppendInt32(habbo.SnowX);
                StageStarting.AppendInt32(habbo.SnowY);
                StageStarting.AppendInt32(0);
                //StageStarting.AppendInt32(0);
                StageStarting.AppendInt32(habbo.SnowTeam);
                StageStarting.AppendInt32(habbo.Id);
                StageStarting.AppendString(habbo.Username);
                StageStarting.AppendString(habbo.Motto);
                StageStarting.AppendString(habbo.Figure);
                StageStarting.AppendString(habbo.Gender.ToLower());
                xyz++;
            }
            War.SendToStorm(StageStarting, false, 0);
            Thread.Sleep(5000);
            foreach (Habbo habbo in War.WarUsers)
            {
                ServerMessage PlayerExited = new ServerMessage(Outgoing.Game2PlayerExitedGameArenaMessageEvent);
                PlayerExited.AppendInt32(habbo.Id);
                PlayerExited.AppendInt32(20);
                habbo.GetClient().SendMessage(PlayerExited);
            }
            Thread thrd = new Thread(delegate()
            {
                int countdown = 120;
                while (countdown != 0)
                {
                    ServerMessage StageRunning = new ServerMessage(Outgoing.Game2StageRunningMessageEvent);
                    StageRunning.AppendInt32(countdown--);
                    War.SendToStorm(StageRunning);
                    ServerMessage Message5_0 = new ServerMessage(2352);//Game2FullGameStatusMessageEvent
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendInt32(countdown);
                    War.SendToStorm(Message5_0);
                    Thread.Sleep(1000);
                }
            });
            thrd.Start();
            War.SnowStormStart();
        }
        internal void CheckArenaStatic(ServerMessage Message, SnowStorm War)
        {
                Message.AppendInt32((int)(20 + War.WarUsers.Count));
                Message.AppendInt32(3);
                Message.AppendInt32(0);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xaf00);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0);
                Message.AppendInt32(3);
                Message.AppendInt32(1);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x1770a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(1);
                Message.AppendInt32(3);
                Message.AppendInt32(2);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(3);
                Message.AppendInt32(3);
                Message.AppendInt32(3);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0x11f80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(5);
                Message.AppendInt32(3);
                Message.AppendInt32(4);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x15180);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(6);
                Message.AppendInt32(3);
                Message.AppendInt32(5);
                Message.AppendInt32(0xc800);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(7);
                Message.AppendInt32(3);
                Message.AppendInt32(6);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x16a80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(10);
                Message.AppendInt32(3);
                Message.AppendInt32(7);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0xe100);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(14);
                Message.AppendInt32(3);
                Message.AppendInt32(8);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x12c0a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(15);
                Message.AppendInt32(3);
                Message.AppendInt32(9);
                Message.AppendInt32(0x15e0a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x10);
                Message.AppendInt32(3);
                Message.AppendInt32(10);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xd480);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x11);
                Message.AppendInt32(3);
                Message.AppendInt32(11);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0xbb80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x12);
                Message.AppendInt32(3);
                Message.AppendInt32(12);
                Message.AppendInt32(0x12c0a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(20);
                Message.AppendInt32(3);
                Message.AppendInt32(13);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x11f80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x15);
                Message.AppendInt32(3);
                Message.AppendInt32(14);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0xbb80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x18);
                Message.AppendInt32(3);
                Message.AppendInt32(15);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x1450a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x19);
                Message.AppendInt32(3);
                Message.AppendInt32(0x10);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0x15180);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x1b);
                Message.AppendInt32(3);
                Message.AppendInt32(0x11);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0xed80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x1d);
                Message.AppendInt32(3);
                Message.AppendInt32(0x12);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xfa00);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(30);
                Message.AppendInt32(3);
                Message.AppendInt32(0x13);
                Message.AppendInt32(0xfa00);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x23);
        }
    }
}
