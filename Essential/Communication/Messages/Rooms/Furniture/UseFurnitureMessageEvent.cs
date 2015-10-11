using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Furniture
{
	internal sealed class UseFurnitureMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null)
				{
                    uint uint__ = Event.PopWiredUInt();
					RoomItem class2 = @class.method_28(uint__);
					if (class2 != null)
					{
						bool bool_ = false;
						if (@class.method_26(Session))
						{
							bool_ = true;
						}
                        class2.Interactor.OnTrigger(Session, class2, Event.PopWiredInt32(), bool_);
                        if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "SWITCHSTATE")
						{
                            Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
						}
						else
						{
                            if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDLIFEGUARDTOWER" && class2.GetBaseItem().Name == "bw_lgchair")
							{
                                Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
							}
							else
							{
                                if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDSURFBOARD" && class2.GetBaseItem().Name.Contains("bw_sboard"))
								{
                                    Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
								}
								else
								{
                                    if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDBEETLE" && class2.GetBaseItem().Name.Contains("bw_van"))
									{
                                        Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
									}
									else
									{
                                        if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDNEONFLOOR" && class2.GetBaseItem().Name.Contains("party_floor"))
										{
                                            Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
										}
										else
										{
                                            if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDDISCOBALL" && class2.GetBaseItem().Name.Contains("party_ball"))
											{
                                                Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
											}
											else
											{
                                                if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDJUKEBOX" && class2.GetBaseItem().Name.Contains("jukebox"))
                                                {
                                                    Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                                                }
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
