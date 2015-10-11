using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Catalog
{
    internal sealed class GetGiftWrappingConfigurationEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            ServerMessage Message = new ServerMessage(Outgoing.ShopData2); // Rootkit
            Message.AppendBoolean(true);
            Message.AppendInt32(1);
            Message.AppendInt32(4);
            int num;
            for (num = 187; num < 191; num++)
            {
                Message.AppendInt32(num);
            }
            Message.AppendInt32(7);
            Message.AppendInt32(0);
            Message.AppendInt32(1);
            Message.AppendInt32(2);
            Message.AppendInt32(3);
            Message.AppendInt32(4);
            Message.AppendInt32(5);
            Message.AppendInt32(6);
            Message.AppendInt32(11);
            Message.AppendInt32(0);
            Message.AppendInt32(1);
            Message.AppendInt32(2);
            Message.AppendInt32(3);
            Message.AppendInt32(4);
            Message.AppendInt32(5);
            Message.AppendInt32(6);
            Message.AppendInt32(7);
            Message.AppendInt32(8);
            Message.AppendInt32(9);
            Message.AppendInt32(10);
            Message.AppendInt32(4);
            for (num = 187; num < 191; num++)
            {
                Message.AppendInt32(num);
            }
            Session.SendMessage(Message);
            ServerMessage OfferConfiguration = new ServerMessage(Outgoing.Offer); // Rootkit
            OfferConfiguration.AppendInt32(100);
            OfferConfiguration.AppendInt32(6);
            OfferConfiguration.AppendInt32(1);
            OfferConfiguration.AppendInt32(1);
            OfferConfiguration.AppendInt32(2);
            OfferConfiguration.AppendInt32(40);
            OfferConfiguration.AppendInt32(99);
            Session.SendMessage(OfferConfiguration);
        }
    }
}