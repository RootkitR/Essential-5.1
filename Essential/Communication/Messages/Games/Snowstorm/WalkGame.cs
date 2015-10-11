using Essential.HabboHotel.GameClients;
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
    class WalkGame : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int X = Event.PopWiredInt32() / 0xc80;
            int Y = Event.PopWiredInt32() / 0xc80;
            new Thread(delegate()
            {
                try
                {
                    int i = 3;
                    int num2 = 5;
                    int num3 = 0;
                    while ((i != X) || (num2 != Y))
                    {
                        if (i != X)
                        {
                            if (i < X)
                            {
                                i++;
                            }
                            else
                            {
                                i--;
                            }
                        }
                        if (num2 != Y)
                        {
                            if (num2 < Y)
                            {
                                num2++;
                            }
                            else
                            {
                                num2--;
                            }
                        }
                        ServerMessage message = new ServerMessage(Outgoing.Game2GameStatusMessageEvent);
                        message.AppendInt32(-11);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(1);
                        message.AppendInt32(5);
                        message.AppendInt32(0);
                        message.AppendInt32(i * 0xc80);
                        message.AppendInt32(num2 * 0xc80);
                        message.AppendInt32(i);
                        message.AppendInt32(num2);
                        message.AppendInt32(3);
                        message.AppendInt32(5);
                        message.AppendInt32(5);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(X);
                        message.AppendInt32(Y);
                        message.AppendInt32(X * 0xc80);
                        message.AppendInt32(Y * 0xc80);
                        message.AppendInt32(0);
                        message.AppendInt32(1);
                        message.AppendUInt(Session.GetHabbo().Id);
                        message.AppendString(Session.GetHabbo().Username);
                        message.AppendString(Session.GetHabbo().Motto);
                        message.AppendString(Session.GetHabbo().Figure);
                        message.AppendString(Session.GetHabbo().Gender.ToLower());
                        message.AppendInt32(-11);
                        message.AppendInt32(0);
                        message.AppendInt32(num3 * 7);
                        num3++;
                        message.AppendInt32(0);
                        message.AppendInt32(1);
                        message.AppendInt32(1);
                        message.AppendInt32(12);
                        message.AppendInt32(0);
                        message.AppendInt32(10);
                        message.AppendInt32(10);
                        message.AppendInt32(10);
                        message.AppendInt32(10);
                        message.AppendInt32(10);
                        Session.SendMessage(message);
                        Thread.Sleep(500);
                    }
                }
                catch
                {
                }
            }).Start();
        }
    }
}
