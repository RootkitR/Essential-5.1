using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Games;
namespace Essential.Communication.Messages.Games
{
    internal sealed class GetGamePowerups : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {

            ServerMessage SerializePowerUps = new ServerMessage(15);
            SerializePowerUps.AppendInt32(Essential.GetGame().GetGamesManager().PowerupPackages.Count);
            foreach (PowerupPackage PowPackage in Essential.GetGame().GetGamesManager().PowerupPackages.Values)
            {
                SerializePowerUps.AppendString(PowPackage.PackageName);
                SerializePowerUps.AppendString(PowPackage.PowerupType);
                SerializePowerUps.AppendInt32(PowPackage.Amount);
                SerializePowerUps.AppendInt32(PowPackage.CostCredits);
            }
            Session.SendMessage(SerializePowerUps);

        }
    }
}