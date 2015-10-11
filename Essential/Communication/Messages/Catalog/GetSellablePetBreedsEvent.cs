using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Pets;
using System.Collections.Generic;
namespace Essential.Communication.Messages.Catalog
{
	internal sealed class GetSellablePetBreedsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
             ServerMessage Message = new ServerMessage(Outgoing.PetRace); //Rootkit
             string PetType = Event.PopFixedString();
             Message.AppendStringWithBreak(PetType);
             int petid = int.Parse(PetType.Substring(PetType.IndexOf('t'), PetType.Length - PetType.IndexOf('t')).Replace("t",""));
            if (PetRace.RaceGotRaces(petid))
            {
                List<PetRace> Races = PetRace.GetRacesForRaceId(petid);
                Message.AppendInt32(Races.Count);
                foreach (PetRace r in Races)
                {
                    Message.AppendInt32(petid);
                    Message.AppendInt32(r.Color1);
                    Message.AppendInt32(r.Color2);
                    Message.AppendBoolean(r.Has1Color);
                    Message.AppendBoolean(r.Has2Color); 
                }
            }
            else
            {
                Message.AppendInt32(0);
            }
				Session.SendMessage(Message);
			}
    
	}
}
