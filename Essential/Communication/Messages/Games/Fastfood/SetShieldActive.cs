using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Games;
using Essential.Storage;

namespace Essential.Communication.Messages.Games
{
    internal sealed class SetShieldActive : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int ShieldId = Event.PopWiredInt32();
            if (ShieldId > 4)
            {
                return;
            }
            Essential.GetGame().GetGamesManager().ActivateShield(Session.Basejump_LobbyId, ShieldId, Session.Basejump_UserId);
        }
    }
}