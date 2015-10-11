using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.SoundMachine;
namespace Essential.Communication.Messages.SoundMachine
{
	internal sealed class GetSongInfoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();

            ServerMessage Message = new ServerMessage(Outgoing.SongInfo); // Updated
			Message.AppendInt32(num);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = Event.PopWiredInt32();

                    
                    if (num2 > 0)
                    {
                        Message.AppendInt32(SongManager.GetSong(num2).Id);
                        Message.AppendString(SongManager.GetSong(num2).Name);
                        Message.AppendString(SongManager.GetSong(num2).Track);
                        Message.AppendInt32(SongManager.GetSong(num2).Length);
                        Message.AppendString(SongManager.GetSong(num2).Author);
                    }
                    else
                    {
                        // Ei lähetetä osittaista pakettia!!
                        return;
                    }
                }
            }
          
			Session.SendMessage(Message);
		}
	}
}
