using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Games;
namespace Essential.Communication.Messages.Games
{
    internal sealed class GetLocaleStrings : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {

                  ServerMessage Localizations = new ServerMessage(13);
                  Localizations.AppendInt32(Essential.GetGame().GetGamesManager().GetLocales("fi").Count);
                  foreach (GameLocale Locale in Essential.GetGame().GetGamesManager().GetLocales("fi"))
                  {
                      Localizations.AppendString(Locale.LocaleKey);
                      Localizations.AppendString(Locale.LocaleValue);
                  }
                  Session.SendMessage(Localizations);

                  if (ServerConfiguration.BasejumpMaintenance)
                  {
                      ServerMessage MaintenanceMode = new ServerMessage(1);
                      MaintenanceMode.AppendBoolean(false);
                      MaintenanceMode.AppendBoolean(ServerConfiguration.BasejumpMaintenance);
                      Session.SendMessage(MaintenanceMode);
                  }
        }
    }
}