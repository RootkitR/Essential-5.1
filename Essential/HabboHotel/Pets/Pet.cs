using System;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Pets
{
	internal sealed class Pet
	{
		public uint PetId;
		public uint OwnerId;
		public int VirtualId;

		public uint Type;
		public string Name;
		public string Race;
		public string Color;

		public int Expirience;
		public int Energy;
		public int Nutrition;

		public uint RoomId;
		public int X;
		public int Y;
		public double Z;

		public int Respect;

		public double CreationStamp;
		public bool PlacedInRoom;

        internal readonly int[] experienceLevels = new int[] { 0, 100, 200, 400, 600, 900, 1300, 1800, 2400, 3200, 4300, 5700, 7600, 10100, 13300, 17500, 23000, 30200, 39600, 51900 };

		internal DatabaseUpdateState DBState;
		public Room Room
		{
			get
			{
				if (!IsInRoom)
				{
					return null;
				}
					return Essential.GetGame().GetRoomManager().GetRoom(RoomId);
			}
		}
		public bool IsInRoom
		{
			get
			{
				return (RoomId > 0);
			}
		}
		public int Level
		{
			get
			{
				for (int level = 0; level < this.experienceLevels.Length; level++)
				{
                    if (this.Expirience < this.experienceLevels[level])
                        return level;
				}

                return MaxLevel;
			}
		}
		public int MaxLevel
		{
			get
			{
				return 20;
			}
		}
		public int ExpirienceGoal
		{
			get
			{
				int level;
				if (this.Level != this.MaxLevel)
				{
					level = this.experienceLevels[this.Level];
				}
				else
				{
                    level = 51900;
				}
				return level;
			}
		}
		public int MaxEnergy
		{
			get
			{
				return 100 + (this.Level * 20);
			}
		}
		public int MaxNutrition
		{
			get
			{
				return 100;
			}
		}
		public int Age
		{
			get
			{
				return (int)Math.Floor((Essential.GetUnixTimestamp() - this.CreationStamp) / 86400.0);
			}
		}
		public string Look
		{
			get
			{
                return Type + " " + Race + " " + Color.ToLower();
			}
		}
		public string UNDEFINED
		{
			get
			{
				return OldEncoding.encodeVL64((int)this.Type) + OldEncoding.encodeVL64(Convert.ToInt32(this.Race)) + this.Color;
			}
		}
		public string OwnerName
		{
			get
			{
				return Essential.GetGame().GetClientManager().GetNameById(OwnerId);
			}
		}
		public Pet(uint PetId, uint OwnerId, uint RoomId, string Name, uint Type, string Race, string Color, int Expirience, int Energy, int Nutrition, int Respect, double CreationStamp, int X, int Y, double Z)
		{
			this.PetId = PetId;
			this.OwnerId = OwnerId;
			this.RoomId = RoomId;
			this.Name = Name;
			this.Type = Type;
			this.Race = Race;
			this.Color = Color;
			this.Expirience = Expirience;
			this.Energy = Energy;
			this.Nutrition = Nutrition;
			this.Respect = Respect;
			this.CreationStamp = CreationStamp;
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.PlacedInRoom = false;
			this.DBState = DatabaseUpdateState.Updated;
		}
		public void OnRespect()
		{
			this.Respect++;
			if (this.DBState != DatabaseUpdateState.NeedsInsert)
			{
				this.DBState = DatabaseUpdateState.NeedsUpdate;
			}
			if (this.Expirience <= 51900)
			{
				this.AddExpirience(10, 0);
			}
            ServerMessage Message = new ServerMessage(Outgoing.RespectPet); // NEW
            Message.AppendInt32(this.VirtualId);
            Message.AppendInt32(this.Respect);
            this.Room.SendMessage(Message, null);
            this.Room.SendMessage(this.Room.ComposeInfoMessage(this.Name +" wurde gekrault!", this.VirtualId),null);
           /* ServerMessage Message2 = new ServerMessage(Outgoing.PetRespectNotification); // NEW
            Message2.AppendUInt(this.OwnerId);
            Message2.AppendInt32(1);
            SerializeInventory(Message2);
                this.Room.SendMessage(Message2, null);*/
			/*ServerMessage Message = new ServerMessage(606);
			Message.AppendInt32(this.Respect);
			Message.AppendUInt(this.OwnerId);
			Message.AppendUInt(this.PetId);
			Message.AppendStringWithBreak(this.Name);
			Message.AppendBoolean(false);
			Message.AppendInt32(10);
			Message.AppendBoolean(false);
			Message.AppendInt32(-2);
			Message.AppendBoolean(true);
			Message.AppendStringWithBreak("281");
            */
			//this.Room.SendMessage(Message, null);
		}
		public void AddExpirience(int Amount, int int_9)
		{
			this.PetEnergy(-int_9);

            bool LevelUp = false;

			if (this.Expirience < 51900)
			{
                if (this.Expirience + Amount >= this.ExpirienceGoal && this.Level != this.MaxLevel)
                {
                    LevelUp = true;
                }

                if (this.Expirience + Amount > 51900)
                {
                    this.Expirience = 51900;
                }
                else
                {
                    this.Expirience += Amount;
                }

				if (this.DBState != DatabaseUpdateState.NeedsInsert)
				{
					this.DBState = DatabaseUpdateState.NeedsUpdate;
				}
				if (this.Room != null)
				{
                    ServerMessage message2 = new ServerMessage(Outgoing.AddExperience); // Updated
					message2.AppendUInt(this.PetId);
					message2.AppendInt32(this.VirtualId);
					message2.AppendInt32(Amount);
					this.Room.SendMessage(message2, null);
				}

                if (this.Room != null && LevelUp)
                {
                    this.Energy = this.MaxEnergy;
                    ServerMessage message = new ServerMessage(Outgoing.Talk); // Updated
                    message.AppendInt32(this.VirtualId);
                    message.AppendStringWithBreak("*Ist jetzt im Level " + (this.Level) + "*");
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendInt32(-1);
                    this.Room.SendMessage(message, null);
                    //this.Room.SendMessage(this.SerializePetCommands(), null);
                }
			}
		}
        public void PetEnergy(int Add)
        {
            this.Energy += Add;
            if (this.Energy < 0)
            {
                this.Energy = 0;
            }

            if (this.DBState != DatabaseUpdateState.NeedsInsert)
            {
                this.DBState = DatabaseUpdateState.NeedsUpdate;
            }
        }
		public void SerializeInventory(ServerMessage Message)
		{
			Message.AppendUInt(PetId);
			Message.AppendStringWithBreak(Name);
            Message.AppendUInt(Type); // typeId
            Message.AppendInt32(int.Parse(Race)); // paletteId
            Message.AppendStringWithBreak(Color); // color
            Message.AppendInt32(0); // unknown
            Message.AppendInt32(0); // somthing-count
            Message.AppendInt32(Level); // level
		}
		public ServerMessage SerializeInfo()
		{
            ServerMessage Nfo = new ServerMessage(Outgoing.PetInformation); // Updated
			Nfo.AppendUInt(PetId);
			Nfo.AppendStringWithBreak(Name);
			Nfo.AppendInt32(Level);
			Nfo.AppendInt32(MaxLevel);
			Nfo.AppendInt32(Expirience);
			Nfo.AppendInt32(ExpirienceGoal);
			Nfo.AppendInt32(Energy);
			Nfo.AppendInt32(MaxEnergy);
			Nfo.AppendInt32(Nutrition);
			Nfo.AppendInt32(MaxNutrition);
			Nfo.AppendInt32(Respect);
			Nfo.AppendUInt(OwnerId);
			Nfo.AppendInt32(Age);
			Nfo.AppendStringWithBreak(OwnerName);
            Nfo.AppendUInt(1);
            Nfo.AppendBoolean(false); // Have saddle
            Nfo.AppendBoolean(false); // Horse related
            Nfo.AppendInt32(0);
            Nfo.AppendInt32(0);
            Nfo.AppendInt32(0);
            Nfo.AppendInt32(0);
            Nfo.AppendInt32(0);
            Nfo.AppendInt32(0);
            Nfo.AppendInt32(0);
            Nfo.AppendInt32(0);
            Nfo.AppendString("");
            Nfo.AppendBoolean(false);
            Nfo.AppendInt32(-1);
            Nfo.AppendInt32(-1);
            Nfo.AppendInt32(-1);
            Nfo.AppendBoolean(false);
			return Nfo;
		}

        public ServerMessage SerializePetCommands()
        {
            ServerMessage Message = new ServerMessage(Outgoing.PetCommands); // Updated
            Message.AppendUInt(this.PetId);
            Message.AppendInt32(22); //Pet commands count
            //Pet Commands
            Message.AppendInt32(0); //Free
            Message.AppendInt32(1); //Sit
            Message.AppendInt32(14); //Drink
            Message.AppendInt32(43); //Eat
            Message.AppendInt32(2); //Down
            Message.AppendInt32(3); //Here
            Message.AppendInt32(4); //Beg
            Message.AppendInt32(17); //Play football
            Message.AppendInt32(5); //Play dead
            Message.AppendInt32(6); //Stay
            Message.AppendInt32(7); //Follow
            Message.AppendInt32(8); //Stand
            Message.AppendInt32(9); //Jump
            Message.AppendInt32(10); //Speak
            Message.AppendInt32(11); //Play
            Message.AppendInt32(12); //Silent
            Message.AppendInt32(13); //Nest
            Message.AppendInt32(15); //Follow left
            Message.AppendInt32(16); //Follow right
            Message.AppendInt32(24); //Move forwar
            Message.AppendInt32(25); //Turn left
            Message.AppendInt32(26); //Turn right
            //Pet commands acces
            Message.AppendInt32(22); //Pet commands count
            Message.AppendInt32(0); //Free
            Message.AppendInt32(1); //Sit
            Message.AppendInt32(14); //Drink
            Message.AppendInt32(43); //Eat
            Message.AppendInt32(this.CanUseDownPetCommand ? 2 : 0); //Down
            Message.AppendInt32(this.CanUseHerePetCommand ? 3 : 0); //Here
            Message.AppendInt32(this.CanUseBegPetCommand ? 4 : 0); //Beg
            Message.AppendInt32(this.CanUsePlayFootballPetCommand ? 17 : 0); //Play football
            Message.AppendInt32(this.CanUsePlayDeadPetCommand ? 5 : 0); //Play dead
            Message.AppendInt32(this.CanUseStayPetCommand ? 6 : 0); //Stay
            Message.AppendInt32(this.CanUseFollowPetCommand ? 7 : 0); //Follow
            Message.AppendInt32(this.CanUseStandPetCommand ? 8 : 0); //Stand
            Message.AppendInt32(this.CanUseJumpPetCommand ? 9 : 0); //Jump
            Message.AppendInt32(this.CanUseSpeakPetCommand ? 10 : 0); //Speak
            Message.AppendInt32(this.CanUsePlayPetCommand ? 11 : 0); //Play
            Message.AppendInt32(this.CanUseSilentPetCommand ? 12 : 0); //Silent
            Message.AppendInt32(this.CanUseNestPetCommand ? 13 : 0); //Nest
            Message.AppendInt32(this.CanUseFollowLeftPetCommand ? 15 : 0); //Follow left
            Message.AppendInt32(this.CanUseFollowRightPetCommand ? 16 : 0); //Follow right
            Message.AppendInt32(this.CanUseMoveForwarPetCommand ? 24 : 0); //Move forwar
            Message.AppendInt32(this.CanUseTurnLeftPetCommand ? 25 : 0); //Turn left
            Message.AppendInt32(this.CanUseTurnRightPetCommand ? 26 : 0); //Turn right

            return Message;
        }

        public bool CanUseDownPetCommand
        {
            get
            {
                return this.Level >= 2;
            }
        }

        public bool CanUseHerePetCommand
        {
            get
            {
                return this.Level >= 3;
            }
        }

        public bool CanUseBegPetCommand
        {
            get
            {
                return this.Level >= 4;
            }
        }

        public bool CanUsePlayFootballPetCommand
        {
            get
            {
                return this.Level >= 4;
            }
        }

        public bool CanUsePlayDeadPetCommand
        {
            get
            {
                return this.Level >= 5;
            }
        }

        public bool CanUseStayPetCommand
        {
            get
            {
                return this.Level >= 6;
            }
        }

        public bool CanUseFollowPetCommand
        {
            get
            {
                return this.Level >= 7;
            }
        }

        public bool CanUseStandPetCommand
        {
            get
            {
                return this.Level >= 8;
            }
        }

        public bool CanUseJumpPetCommand
        {
            get
            {
                return this.Level >= 9;
            }
        }

        public bool CanUseSpeakPetCommand
        {
            get
            {
                return this.Level >= 10;
            }
        }

        public bool CanUsePlayPetCommand
        {
            get
            {
                return this.Level >= 11;
            }
        }

        public bool CanUseSilentPetCommand
        {
            get
            {
                return this.Level >= 12;
            }
        }

        public bool CanUseNestPetCommand
        {
            get
            {
                return this.Level >= 13;
            }
        }

        public bool CanUseFollowLeftPetCommand
        {
            get
            {
                return this.Level >= 14;
            }
        }

        public bool CanUseFollowRightPetCommand
        {
            get
            {
                return this.Level >= 14;
            }
        }

        public bool CanUseMoveForwarPetCommand
        {
            get
            {
                return this.Level >= 15;
            }
        }

        public bool CanUseTurnLeftPetCommand
        {
            get
            {
                return this.Level >= 15;
            }
        }

        public bool CanUseTurnRightPetCommand
        {
            get
            {
                return this.Level >= 15;
            }
        }
	}
}
