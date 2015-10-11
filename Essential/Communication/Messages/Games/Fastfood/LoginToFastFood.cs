using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System.Data;
using Essential.Storage;

namespace Essential.Communication.Messages.Games
{
    internal sealed class LoginToFastFood : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string AuthTicket = Event.PopFixedString();
            string PrivateHost = Event.PopFixedString();

            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
			{
                // Check auth token

                dbClient.AddParamWithValue("token", AuthTicket);

                int UserId = dbClient.ReadInt32("SELECT user_id FROM basejump_auth_tokens WHERE token = @token LIMIT 1");

                if (dbClient.ReadInt32("SELECT COUNT(id) FROM users WHERE id = '" + UserId + "' LIMIT 1") == 1)
                {
                    Session.Basejump_UserId = UserId;

                   if (dbClient.ReadInt32("SELECT COUNT(id) FROM basejump_users_powerups WHERE user_id = '" + UserId + "' LIMIT 1") == 0)
                   {
                       dbClient.ExecuteQuery("INSERT INTO `basejump_users_powerups` (`user_id`) VALUES ('" + UserId + "')");
                   }

                }
                else
                {
                    // Token not found :(
                    return;
                }
            }



            ServerMessage LoggedInSuccessfully = new ServerMessage(Outgoing.LoginFastFood);
            Session.SendMessage(LoggedInSuccessfully);
            

            Console.WriteLine("Logged to BaseJump game with ticket: " + AuthTicket);

      /*      ServerMessage Localizations = new ServerMessage(13);
            Localizations.AppendInt32(1);
            Localizations.AppendString("devtest");
            Localizations.AppendString("devtest2");
            Session.SendMessage(Localizations);


            ServerMessage Credits = new ServerMessage(16);
            Credits.AppendInt32(1000);
            Session.SendMessage(Credits);
            */
         

        }
    }
}
