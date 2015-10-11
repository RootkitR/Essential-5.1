using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Catalogs;
namespace Essential.Communication.Messages.Recycler
{
	internal sealed class GetRecyclerPrizesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            ServerMessage Response = new ServerMessage(Outgoing.ShopData1);
            Response.AppendBoolean(true);
            Response.AppendInt32(1);
            Response.AppendInt32(0);
            Response.AppendInt32(0);
            Response.AppendInt32(1);
            Response.AppendInt32(0x2710);
            Response.AppendInt32(0x30);
            Response.AppendInt32(7);
            Session.SendMessage(Response);

            Dictionary<uint, List<EcotronReward>> list = Essential.GetGame().GetCatalog().GetEcotronRewards();
            ServerMessage Message = new ServerMessage(Outgoing.RecyclePrizes); // Updated
            Message.AppendInt32(list.Count);
            foreach (var reward2 in list)
            {

                Message.AppendInt32(reward2.Key);

                if (reward2.Key == 1)
                    Message.AppendInt32(0);
                if (reward2.Key == 2)
                    Message.AppendInt32(4);
                if (reward2.Key == 3)
                    Message.AppendInt32(40);
                if (reward2.Key == 4)
                    Message.AppendInt32(200);
                if (reward2.Key == 5)
                    Message.AppendInt32(2000);
                Message.AppendInt32(reward2.Value.Count);
                foreach (EcotronReward reward in reward2.Value)
                {
                    Message.AppendString(reward.GetBaseItem().Name);
                    Message.AppendInt32(1);
                    Message.AppendString(reward.GetBaseItem().Type.ToString().ToLower());
                    Message.AppendInt32(reward.GetBaseItem().Sprite);
                }
            }
            Session.SendMessage(Message);
		}
	}
}
