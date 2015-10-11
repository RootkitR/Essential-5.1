using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pets;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
using Essential.HabboHotel.Pathfinding;
using Essential.HabboHotel.Items;
using System.Collections.Generic;
namespace Essential.HabboHotel.RoomBots
{
	internal sealed class PetBot : BotAI
	{
        private int SpeechTimer;
        private int ActionTimer;
        private FollowType FollowType;
        private RoomUser FollowUser;
		public PetBot(int int_4)
		{
            this.SpeechTimer = new Random((int_4 ^ 2) + DateTime.Now.Millisecond).Next(25, 60);
            this.ActionTimer = new Random((int_4 ^ 2) + DateTime.Now.Millisecond).Next(10, 60);
            this.FollowType = FollowType.None;
            this.FollowUser = null;
		}
        private void RemovePetStatus()
        {
            RoomUser Pet = base.GetRoomUser();

            // Remove Status
            Pet.Statusses.Remove("sit");
            Pet.Statusses.Remove("lay");
            Pet.Statusses.Remove("snf");
            Pet.Statusses.Remove("eat");
            Pet.Statusses.Remove("ded");
            Pet.Statusses.Remove("jmp");
        }
		private int method_4()
		{
			RoomUser @class = base.GetRoomUser();
			int result = 5;
			if (@class.PetData.Level >= 1)
			{
				result = Essential.smethod_5(1, 8);
			}
			else
			{
				if (@class.PetData.Level >= 5)
				{
					result = Essential.smethod_5(1, 7);
				}
				else
				{
					if (@class.PetData.Level >= 10)
					{
						result = Essential.smethod_5(1, 6);
					}
				}
			}
			return result;
		}
		private void method_5(int int_4, int int_5, bool bool_0)
		{
			RoomUser @class = base.GetRoomUser();
			if (bool_0)
			{
                int int_6 = Essential.smethod_5(0, base.GetRoom().RoomModel.int_4);
                int int_7 = Essential.smethod_5(0, base.GetRoom().RoomModel.int_5);
				@class.MoveTo(int_6, int_7);
			}
			else
			{
                if (int_4 < base.GetRoom().RoomModel.int_4 && int_5 < base.GetRoom().RoomModel.int_5 && int_4 >= 0 && int_5 >= 0)
				{
					@class.MoveTo(int_4, int_5);
				}
			}
		}
		public override void OnSelfEnterRoom()
		{
			if (base.GetRoomUser().PetData.X > 0 && base.GetRoomUser().PetData.Y > 0)
			{
				base.GetRoomUser().X = base.GetRoomUser().PetData.X;
				base.GetRoomUser().Y = base.GetRoomUser().PetData.Y;
			}
			this.method_5(0, 0, true);
		}
		public override void OnSelfLeaveRoom(bool bool_0)
		{
            if (base.GetRoomBot().RoomUser_0 != null)
			{
                RoomUser RoomUser_ = base.GetRoomBot().RoomUser_0;
                if (RoomUser_.class34_1 != null && RoomUser_ == base.GetRoomBot().RoomUser_0)
				{
                    base.GetRoomBot().RoomUser_0 = null;
					RoomUser_.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
					RoomUser_.class34_1 = null;
					RoomUser_.RoomUser_0 = null;
				}
			}
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				if (base.GetRoomUser().PetData.DBState == DatabaseUpdateState.NeedsInsert)
				{
					@class.AddParamWithValue("petname" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Name);
					@class.AddParamWithValue("petcolor" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Color);
					@class.AddParamWithValue("petrace" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Race);
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO `user_pets` VALUES ('",
						base.GetRoomUser().PetData.PetId,
						"', '",
						base.GetRoomUser().PetData.OwnerId,
						"', '0', @petname",
						base.GetRoomUser().PetData.PetId,
						", @petrace",
						base.GetRoomUser().PetData.PetId,
						", @petcolor",
						base.GetRoomUser().PetData.PetId,
						", '",
						base.GetRoomUser().PetData.Type,
						"', '",
						base.GetRoomUser().PetData.Expirience,
						"', '",
						base.GetRoomUser().PetData.Energy,
						"', '",
						base.GetRoomUser().PetData.Nutrition,
						"', '",
						base.GetRoomUser().PetData.Respect,
						"', '",
						base.GetRoomUser().PetData.CreationStamp,
						"', '",
						base.GetRoomUser().PetData.X,
						"', '",
						base.GetRoomUser().PetData.Y,
						"', '",
						base.GetRoomUser().PetData.Z,
						"');"
					}));
				}
				else
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE user_pets SET room_id = '0', expirience = '",
						base.GetRoomUser().PetData.Expirience,
						"', energy = '",
						base.GetRoomUser().PetData.Energy,
						"', nutrition = '",
						base.GetRoomUser().PetData.Nutrition,
						"', respect = '",
						base.GetRoomUser().PetData.Respect,
						"' WHERE Id = '",
						base.GetRoomUser().PetData.PetId,
						"' LIMIT 1; "
					}));
				}
				base.GetRoomUser().PetData.DBState = DatabaseUpdateState.Updated;
			}
		}
		public override void OnUserEnterRoom(RoomUser RoomUser_0)
		{
		}
		public override void OnUserLeaveRoom(GameClient Session)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
				string string_ = Session.GetHabbo().Username;
                RoomUser @class = base.GetRoom().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (base.GetRoomBot().RoomUser_0 != null && @class != null && @class.class34_1 != null && @class == base.GetRoomBot().RoomUser_0)
				{
                    base.GetRoomBot().RoomUser_0 = null;
				}
				try
				{
                    if (string_.ToLower() == base.GetRoomUser().PetData.OwnerName.ToLower() && string_.ToLower() != base.GetRoom().Owner.ToLower())
					{
                        base.GetRoom().method_6(base.GetRoomUser().PetData.VirtualId, false);
						Session.GetHabbo().GetInventoryComponent().AddPet(base.GetRoomUser().PetData);
					}
				}
				catch
				{
				}
			}
		}
		public override void OnUserSay(RoomUser RoomUser, string string_0)
		{
			RoomUser Pet = base.GetRoomUser();
            //TODO: Pet Commands :D
			if (Pet.RoomBot.RoomUser_0 == null)
			{
				if (string_0.ToLower().Equals(Pet.PetData.Name.ToLower()))
				{
                    Pet.method_9(Rotation.GetRotation(Pet.X, Pet.Y, RoomUser.X, RoomUser.Y));
				}
				else
				{
					if (string_0.ToLower().StartsWith(Pet.PetData.Name.ToLower() + " ") && RoomUser.GetClient().GetHabbo().Username.ToLower() == base.GetRoomUser().PetData.OwnerName.ToLower())
					{
						string key = string_0.Substring(Pet.PetData.Name.ToLower().Length + 1);
						if ((Pet.PetData.Energy >= 10 && this.method_4() < 6) || Pet.PetData.Level >= 15)
						{
							Pet.Statusses.Clear();
							if (!Essential.GetGame().GetRoleManager().dictionary_5.ContainsKey(key))
							{
								string[] array = new string[]
								{
									EssentialEnvironment.GetExternalText("pet_response_confused1"),
									EssentialEnvironment.GetExternalText("pet_response_confused2"),
									EssentialEnvironment.GetExternalText("pet_response_confused3"),
									EssentialEnvironment.GetExternalText("pet_response_confused4"),
									EssentialEnvironment.GetExternalText("pet_response_confused5"),
									EssentialEnvironment.GetExternalText("pet_response_confused6"),
									EssentialEnvironment.GetExternalText("pet_response_confused7")
								};
								Random random = new Random();
								Pet.HandleSpeech(null, array[random.Next(0, array.Length - 1)], false);
							}
							else
							{
                                switch (Essential.GetGame().GetRoleManager().dictionary_5[key])
                                {
                                    case 0:
                                        // Remove Status
                                        RemovePetStatus();

                                        this.SpeechTimer = 0;
                                        this.ActionTimer = 0;
                                        this.FollowType = FollowType.None;

                                        // Add Status
                                        Pet.Statusses.Add("sit", TextHandling.GetString(Pet.double_0));
                                        break;
                                    case 1:
                                        this.ActionTimer = 25;
                                        this.FollowType = FollowType.None;

                                        // Remove Status
                                        RemovePetStatus();

                                        Pet.PetData.AddExpirience(10, -10); // Give XP

                                        // Add Status
                                        Pet.Statusses.Add("sit", TextHandling.GetString(Pet.double_0));
                                        break;
                                    case 2:
                                        this.ActionTimer = 30;
                                        this.FollowType = FollowType.None;

                                        // Remove Status
                                        RemovePetStatus();

                                        Pet.PetData.AddExpirience(10, -10); // Give XP

                                        // Add Status
                                        Pet.Statusses.Add("lay", TextHandling.GetString(Pet.double_0));
                                        break;
                                    case 3:
                                        this.ActionTimer = 30;
                                        this.FollowType = FollowType.None;

                                        RemovePetStatus();

                                        int NewX = RoomUser.X;
                                        int NewY = RoomUser.Y;

                                        #region Rotation
                                        if (RoomUser.BodyRotation == 4)
                                        {
                                            NewY = RoomUser.Y + 1;
                                        }
                                        else if (RoomUser.BodyRotation == 0)
                                        {
                                            NewY = RoomUser.Y - 1;
                                        }
                                        else if (RoomUser.BodyRotation == 6)
                                        {
                                            NewX = RoomUser.X - 1;
                                        }
                                        else if (RoomUser.BodyRotation == 2)
                                        {
                                            NewX = RoomUser.X + 1;
                                        }
                                        else if (RoomUser.BodyRotation == 3)
                                        {
                                            NewX = RoomUser.X + 1;
                                            NewY = RoomUser.Y + 1;
                                        }
                                        else if (RoomUser.BodyRotation == 1)
                                        {
                                            NewX = RoomUser.X + 1;
                                            NewY = RoomUser.Y - 1;
                                        }
                                        else if (RoomUser.BodyRotation == 7)
                                        {
                                            NewX = RoomUser.X - 1;
                                            NewY = RoomUser.Y - 1;
                                        }
                                        else if (RoomUser.BodyRotation == 5)
                                        {
                                            NewX = RoomUser.X - 1;
                                            NewY = RoomUser.Y + 1;
                                        }
                                        #endregion

                                        Pet.PetData.AddExpirience(11, -10); // Give XP

                                        Pet.MoveTo(NewX, NewY);
                                        break;
                                    case 4:
                                        this.ActionTimer = 10;
                                        this.FollowType = FollowType.None;

                                        //Remove Status
                                        RemovePetStatus();

                                        Pet.PetData.AddExpirience(15, -13); // Give XP

                                        Pet.Statusses.Add("beg", TextHandling.GetString(Pet.double_0));
                                        break;
                                    case 5:
                                        this.ActionTimer = 30;
                                        this.FollowType = FollowType.None;

                                        // Remove Status
                                        RemovePetStatus();

                                        // Add Status 
                                        Pet.Statusses.Add("ded", TextHandling.GetString(Pet.double_0));

                                        Pet.PetData.AddExpirience(20, -18); // Give XP
                                        break;
                                    case 6:
                                        this.ActionTimer = 120;
                                        this.FollowType = FollowType.None;

                                        // Remove Status
                                        RemovePetStatus();

                                        // Add Status 
                                        Pet.Statusses.Add("sit", TextHandling.GetString(Pet.double_0));

                                        Pet.PetData.AddExpirience(20, -18); // Give XP

                                        Pet.MoveTo(Pet.X, Pet.Y);
                                        break;
                                    case 7:
                                        this.ActionTimer = 120;
                                        this.FollowType = FollowType.Normal;
                                        this.FollowUser = RoomUser;

                                        Pet.PetData.AddExpirience(30, -30); // Give XP

                                        this.PetFollowUser(RoomUser, Pet, FollowType.Normal);
                                        break;
                                    case 8:
                                        //Stand
                                        break;
                                    case 9:
                                        this.ActionTimer = 5;
                                        this.FollowType = FollowType.None;

                                        // Remove Status
                                        RemovePetStatus();

                                        // Add Status 
                                        Pet.Statusses.Add("jmp", TextHandling.GetString(Pet.double_0));

                                        Pet.PetData.AddExpirience(35, -30); // Give XP
                                        break;
                                    case 10:
                                        // Remove Status
                                        RemovePetStatus();

                                        Pet.PetData.AddExpirience(35, -30); // Give XP

                                        this.SpeechTimer = 2;
                                        this.FollowType = FollowType.None;
                                        break;
                                    case 11:
                                        //Play
                                        break;
                                    case 12:
                                        // Remove Status
                                        RemovePetStatus();

                                        Pet.PetData.AddExpirience(35, -30); // Give XP

                                        this.SpeechTimer = 120;
                                        this.FollowType = FollowType.None;
                                        break;
                                    case 13:
                                        //Nest Pesä

                                        this.ActionTimer = 30;
                                        List<RoomItem> Nests = new List<RoomItem>();
                                        foreach (RoomItem PetItem in this.GetRoomUser().GetRoom().Hashtable_0.Values)
            {
                if (PetItem.GetBaseItem().InteractionType.ToLower() == "pet_nest")
                {
                    Nests.Add(PetItem);
                }
                                            }

                                        if (Nests.Count == 0)
                                        {
                                            return;
                                        }
                                        int PetNX = Nests[new Random().Next(0, Nests.Count - 1)].GetX;
                                        int PetNY = Nests[new Random().Next(0, Nests.Count - 1)].Int32_1;
                                        Pet.MoveTo(PetNX, PetNY);
                                        
                                        Nests.Clear();
                                        Nests = null;

                                         Pet.PetData.AddExpirience(15, -10); // Give XP

                                        break;
                                    case 14:
                                        //Drink

                                          this.ActionTimer = 30;
                                        List<RoomItem> Drinks = new List<RoomItem>();
                                        foreach (RoomItem PetItem in this.GetRoomUser().GetRoom().Hashtable_0.Values)
            {
                if (PetItem.GetBaseItem().InteractionType.ToLower() == "pet_drink")
                {
                    Drinks.Add(PetItem);
                }
                                            }

                                        if (Drinks.Count == 0)
                                        {
                                            return;
                                        }
                                        int PetDX = Drinks[new Random().Next(0, Drinks.Count - 1)].GetX;
                                        int PetDY = Drinks[new Random().Next(0, Drinks.Count - 1)].Int32_1;
                                        Pet.MoveTo(PetDX, PetDY);

                                        Drinks.Clear();
                                        Drinks = null;

                                         Pet.PetData.AddExpirience(25, -10); // Give XP

                                        break;
                                    case 15:
                                        this.ActionTimer = 120;
                                        this.FollowType = FollowType.Left;
                                        this.FollowUser = RoomUser;

                                        Pet.PetData.AddExpirience(35, -30); // Give XP

                                        this.PetFollowUser(RoomUser, Pet, FollowType.Left);
                                        break;
                                    case 16:
                                        this.FollowType = FollowType.Right;
                                        this.FollowUser = RoomUser;

                                        Pet.PetData.AddExpirience(35, -30); // Give XP

                                        this.PetFollowUser(RoomUser, Pet, FollowType.Right);
                                        break;
                                    case 17:
                                        //Play football
                                        break;
                                    case 24:
                                        //Move forwar
                                        break;
                                    case 25:
                                        //Turn left
                                        break;
                                    case 26:
                                        //Turn right
                                        break;
                                    case 43:
                                        //Eat
                                        break;
                                }
							}
						}
						else
						{
							string[] array2 = new string[]
							{
								EssentialEnvironment.GetExternalText("pet_response_sleeping1"),
								EssentialEnvironment.GetExternalText("pet_response_sleeping2"),
								EssentialEnvironment.GetExternalText("pet_response_sleeping3"),
								EssentialEnvironment.GetExternalText("pet_response_sleeping4"),
								EssentialEnvironment.GetExternalText("pet_response_sleeping5")
							};
							string[] array3 = new string[]
							{
								EssentialEnvironment.GetExternalText("pet_response_refusal1"),
								EssentialEnvironment.GetExternalText("pet_response_refusal2"),
								EssentialEnvironment.GetExternalText("pet_response_refusal3"),
								EssentialEnvironment.GetExternalText("pet_response_refusal4"),
								EssentialEnvironment.GetExternalText("pet_response_refusal5")
							};
							Pet.int_10 = Pet.int_12;
							Pet.int_11 = Pet.int_13;
							Pet.Statusses.Clear();
							if (Pet.PetData.Energy < 10)
							{
								Random random2 = new Random();
								Pet.HandleSpeech(null, array2[random2.Next(0, array2.Length - 1)], false);
								if (Pet.PetData.Type != 13u)
								{
									Pet.Statusses.Add("lay", Pet.double_0.ToString());
								}
								else
								{
									Pet.Statusses.Add("lay", (Pet.double_0 - 1.0).ToString());
								}
                                this.SpeechTimer = 25;
                                this.ActionTimer = 20;
								base.GetRoomUser().PetData.PetEnergy(25);
							}
							else
							{
								Random random2 = new Random();
								Pet.HandleSpeech(null, array3[random2.Next(0, array3.Length - 1)], false);
							}
						}
						Pet.UpdateNeeded = true;
					}
				}
			}
		}
		public override void OnUserShout(RoomUser RoomUser_0, string string_0)
		{
		}
		public override void OnTimerTick()
		{
            if (this.SpeechTimer <= 0)
			{
				RoomUser @class = base.GetRoomUser();
				string[] array = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_dog1"),
					EssentialEnvironment.GetExternalText("pet_chatter_dog2"),
					EssentialEnvironment.GetExternalText("pet_chatter_dog3"),
					EssentialEnvironment.GetExternalText("pet_chatter_dog4"),
					EssentialEnvironment.GetExternalText("pet_chatter_dog5")
				};
				string[] array2 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_cat1"),
					EssentialEnvironment.GetExternalText("pet_chatter_cat2"),
					EssentialEnvironment.GetExternalText("pet_chatter_cat3"),
					EssentialEnvironment.GetExternalText("pet_chatter_cat4"),
					EssentialEnvironment.GetExternalText("pet_chatter_cat5")
				};
				string[] array3 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_croc1"),
					EssentialEnvironment.GetExternalText("pet_chatter_croc2"),
					EssentialEnvironment.GetExternalText("pet_chatter_croc3"),
					EssentialEnvironment.GetExternalText("pet_chatter_croc4"),
					EssentialEnvironment.GetExternalText("pet_chatter_croc5")
				};
				string[] array4 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_dog1"),
					EssentialEnvironment.GetExternalText("pet_chatter_dog2"),
					EssentialEnvironment.GetExternalText("pet_chatter_dog3"),
					EssentialEnvironment.GetExternalText("pet_chatter_dog4"),
					EssentialEnvironment.GetExternalText("pet_chatter_dog5")
				};
				string[] array5 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_bear1"),
					EssentialEnvironment.GetExternalText("pet_chatter_bear2"),
					EssentialEnvironment.GetExternalText("pet_chatter_bear3"),
					EssentialEnvironment.GetExternalText("pet_chatter_bear4"),
					EssentialEnvironment.GetExternalText("pet_chatter_bear5")
				};
				string[] array6 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_pig1"),
					EssentialEnvironment.GetExternalText("pet_chatter_pig2"),
					EssentialEnvironment.GetExternalText("pet_chatter_pig3"),
					EssentialEnvironment.GetExternalText("pet_chatter_pig4"),
					EssentialEnvironment.GetExternalText("pet_chatter_pig5")
				};
				string[] array7 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_lion1"),
					EssentialEnvironment.GetExternalText("pet_chatter_lion2"),
					EssentialEnvironment.GetExternalText("pet_chatter_lion3"),
					EssentialEnvironment.GetExternalText("pet_chatter_lion4"),
					EssentialEnvironment.GetExternalText("pet_chatter_lion5")
				};
				string[] array8 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_rhino1"),
					EssentialEnvironment.GetExternalText("pet_chatter_rhino2"),
					EssentialEnvironment.GetExternalText("pet_chatter_rhino3"),
					EssentialEnvironment.GetExternalText("pet_chatter_rhino4"),
					EssentialEnvironment.GetExternalText("pet_chatter_rhino5")
				};
				string[] array9 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_spider1"),
					EssentialEnvironment.GetExternalText("pet_chatter_spider2"),
					EssentialEnvironment.GetExternalText("pet_chatter_spider3"),
					EssentialEnvironment.GetExternalText("pet_chatter_spider4"),
					EssentialEnvironment.GetExternalText("pet_chatter_spider5")
				};
				string[] array10 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_turtle1"),
					EssentialEnvironment.GetExternalText("pet_chatter_turtle2"),
					EssentialEnvironment.GetExternalText("pet_chatter_turtle3"),
					EssentialEnvironment.GetExternalText("pet_chatter_turtle4"),
					EssentialEnvironment.GetExternalText("pet_chatter_turtle5")
				};
				string[] array11 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_chic1"),
					EssentialEnvironment.GetExternalText("pet_chatter_chic2"),
					EssentialEnvironment.GetExternalText("pet_chatter_chic3"),
					EssentialEnvironment.GetExternalText("pet_chatter_chic4"),
					EssentialEnvironment.GetExternalText("pet_chatter_chic5")
				};
				string[] array12 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_frog1"),
					EssentialEnvironment.GetExternalText("pet_chatter_frog2"),
					EssentialEnvironment.GetExternalText("pet_chatter_frog3"),
					EssentialEnvironment.GetExternalText("pet_chatter_frog4"),
					EssentialEnvironment.GetExternalText("pet_chatter_frog5")
				};
				string[] array13 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_dragon1"),
					EssentialEnvironment.GetExternalText("pet_chatter_dragon2"),
					EssentialEnvironment.GetExternalText("pet_chatter_dragon3"),
					EssentialEnvironment.GetExternalText("pet_chatter_dragon4"),
					EssentialEnvironment.GetExternalText("pet_chatter_dragon5")
				};
				string[] array14 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_horse1"),
					EssentialEnvironment.GetExternalText("pet_chatter_horse2"),
					EssentialEnvironment.GetExternalText("pet_chatter_horse3"),
					EssentialEnvironment.GetExternalText("pet_chatter_horse4"),
					EssentialEnvironment.GetExternalText("pet_chatter_horse5")
				};
				string[] array15 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_monkey1"),
					EssentialEnvironment.GetExternalText("pet_chatter_monkey2"),
					EssentialEnvironment.GetExternalText("pet_chatter_monkey3"),
					EssentialEnvironment.GetExternalText("pet_chatter_monkey4"),
					EssentialEnvironment.GetExternalText("pet_chatter_monkey5")
				};
				string[] array16 = new string[]
				{
					EssentialEnvironment.GetExternalText("pet_chatter_generic1"),
					EssentialEnvironment.GetExternalText("pet_chatter_generic2"),
					EssentialEnvironment.GetExternalText("pet_chatter_generic3"),
					EssentialEnvironment.GetExternalText("pet_chatter_generic4"),
					EssentialEnvironment.GetExternalText("pet_chatter_generic5")
				};
				string[] array17 = new string[]
				{
					"sit",
					"lay",
					"snf",
					"ded",
					"jmp",
					"snf",
					"sit",
					"snf"
				};
				string[] array18 = new string[]
				{
					"sit",
					"lay"
				};
				string[] array19 = new string[]
				{
					"wng",
					"grn",
					"flm",
					"std",
					"swg",
					"sit",
					"lay",
					"snf",
					"plf",
					"jmp",
					"flm",
					"crk",
					"rlx",
					"flm"
				};
				if (@class != null)
				{
					Random random = new Random();
					int num = Essential.smethod_5(1, 4);
					if (num == 2)
					{
						@class.Statusses.Clear();
						if (base.GetRoomUser().RoomBot.RoomUser_0 == null)
						{
							if (@class.PetData.Type == 13u)
							{
								@class.Statusses.Add(array18[random.Next(0, array18.Length - 1)], @class.double_0.ToString());
							}
							else
							{
								if (@class.PetData.Type != 12u)
								{
									@class.Statusses.Add(array17[random.Next(0, array17.Length - 1)], @class.double_0.ToString());
								}
								else
								{
									@class.Statusses.Add(array19[random.Next(0, array19.Length - 1)], @class.double_0.ToString());
								}
							}
						}
					}
					switch (@class.PetData.Type)
					{
					case 0u:
						@class.HandleSpeech(null, array[random.Next(0, array.Length - 1)], false);
						break;
					case 1u:
						@class.HandleSpeech(null, array2[random.Next(0, array2.Length - 1)], false);
						break;
					case 2u:
						@class.HandleSpeech(null, array3[random.Next(0, array3.Length - 1)], false);
						break;
					case 3u:
						@class.HandleSpeech(null, array4[random.Next(0, array4.Length - 1)], false);
						break;
					case 4u:
						@class.HandleSpeech(null, array5[random.Next(0, array5.Length - 1)], false);
						break;
					case 5u:
						@class.HandleSpeech(null, array6[random.Next(0, array6.Length - 1)], false);
						break;
					case 6u:
						@class.HandleSpeech(null, array7[random.Next(0, array7.Length - 1)], false);
						break;
					case 7u:
						@class.HandleSpeech(null, array8[random.Next(0, array8.Length - 1)], false);
						break;
					case 8u:
						@class.HandleSpeech(null, array9[random.Next(0, array9.Length - 1)], false);
						break;
					case 9u:
						@class.HandleSpeech(null, array10[random.Next(0, array10.Length - 1)], false);
						break;
					case 10u:
						@class.HandleSpeech(null, array11[random.Next(0, array11.Length - 1)], false);
						break;
					case 11u:
						@class.HandleSpeech(null, array12[random.Next(0, array12.Length - 1)], false);
						break;
					case 12u:
						@class.HandleSpeech(null, array13[random.Next(0, array13.Length - 1)], false);
						break;
					case 13u:
						@class.HandleSpeech(null, array14[random.Next(0, array14.Length - 1)], false);
						break;
					case 14u:
						@class.HandleSpeech(null, array15[random.Next(0, array15.Length - 1)], false);
						break;
					default:
						@class.HandleSpeech(null, array16[random.Next(0, array16.Length - 1)], false);
						break;
					}
				}
                this.SpeechTimer = Essential.smethod_5(30, 120);
			}
			else
			{
                this.SpeechTimer--;
			}
            if (this.ActionTimer <= 0)
			{
				base.GetRoomUser().PetData.PetEnergy(10);
				if (base.GetRoomUser().RoomBot.RoomUser_0 == null)
				{
					this.method_5(0, 0, true);
				}
                this.ActionTimer = 30;
                this.FollowType = FollowType.None;
                this.FollowUser = null;
			}
			else
			{
                this.ActionTimer--;

                if (this.FollowType != FollowType.None && this.FollowUser != null)
                {
                    this.PetFollowUser(this.FollowUser, base.GetRoomUser(), this.FollowType);
                }
			}
		}

        public void PetFollowUser(RoomUser RoomUser, RoomUser Pet, FollowType FollowType)
        {
            RemovePetStatus();

            int NewX = RoomUser.X;
            int NewY = RoomUser.Y;

            #region Rotation
            if (RoomUser.BodyRotation == 4)
            {
                if (FollowType == FollowType.Normal)
                {
                    NewY = RoomUser.Y - 1;
                }
                else if (FollowType == FollowType.Left)
                {
                    NewX = RoomUser.X + 1;
                }
                if (FollowType == FollowType.Right)
                {
                    NewX = RoomUser.X - 1;
                }
            }
            else if (RoomUser.BodyRotation == 0)
            {
                if (FollowType == FollowType.Normal)
                {
                    NewY = RoomUser.Y + 1;
                }
                else if (FollowType == FollowType.Left)
                {
                    NewX = RoomUser.X - 1;
                }
                if (FollowType == FollowType.Right)
                {
                    NewX = RoomUser.X + 1;
                }
            }
            else if (RoomUser.BodyRotation == 6)
            {
                if (FollowType == FollowType.Normal)
                {
                    NewX = RoomUser.X + 1;
                }
                else if (FollowType == FollowType.Left)
                {
                    NewY = RoomUser.Y + 1;
                }
                if (FollowType == FollowType.Right)
                {
                    NewY = RoomUser.Y - 1;
                }
            }
            else if (RoomUser.BodyRotation == 2)
            {
                if (FollowType == FollowType.Normal)
                {
                    NewX = RoomUser.X - 1;
                }
                else if (FollowType == FollowType.Left)
                {
                    NewY = RoomUser.Y - 1;
                }
                if (FollowType == FollowType.Right)
                {
                    NewY = RoomUser.Y + 1;
                }
            }
            else if (RoomUser.BodyRotation == 3)
            {
                if (FollowType == FollowType.Normal)
                {
                    NewX = RoomUser.X - 1;
                    NewY = RoomUser.Y - 1;
                }
                else if (FollowType == FollowType.Left)
                {
                    NewY = RoomUser.Y - 1;
                }
                if (FollowType == FollowType.Right)
                {
                    NewX = RoomUser.X - 1;
                }
            }
            else if (RoomUser.BodyRotation == 1)
            {
                if (FollowType == FollowType.Normal)
                {
                    NewX = RoomUser.X - 1;
                    NewY = RoomUser.Y + 1;
                }
                else if (FollowType == FollowType.Left)
                {
                    NewX = RoomUser.X - 1;
                }
                if (FollowType == FollowType.Right)
                {
                    NewY = RoomUser.Y + 1;
                }
            }
            else if (RoomUser.BodyRotation == 7)
            {
                if (FollowType == FollowType.Normal)
                {
                    NewX = RoomUser.X + 1;
                    NewY = RoomUser.Y + 1;
                }
                else if (FollowType == FollowType.Left)
                {
                    NewY = RoomUser.Y + 1;
                }
                if (FollowType == FollowType.Right)
                {
                    NewX = RoomUser.X + 1;
                }
            }
            else if (RoomUser.BodyRotation == 5)
            {
                if (FollowType == FollowType.Normal)
                {
                    NewX = RoomUser.X + 1;
                    NewY = RoomUser.Y - 1;
                }
                else if (FollowType == FollowType.Left)
                {
                    NewX = RoomUser.X + 1;
                }
                if (FollowType == FollowType.Right)
                {
                    NewY = RoomUser.Y - 1;
                }
            }
            #endregion

            if (Pet.X != NewX && Pet.Y != NewY)
            {
                Pet.MoveTo(NewX, NewY);
            }
        }
	}
}
