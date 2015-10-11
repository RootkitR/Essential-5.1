using Essential;
using Essential.Util;
using HabboEncryption;
using System;
using System.Text;
namespace Essential.Messages
{
    public class ClientMessage
    {
        private byte[] Body;
        private int MessageId;
        private int Pointer;
        private bool isMobile;
        private string[] MobileBody;
        internal ClientMessage(uint messageID, byte[] body, bool isMobileMessage = false, string Packet = "")
        {
            this.Init(messageID, body);
            this.isMobile = isMobileMessage;
            this.MobileBody = Packet.Split((char)1);
            if(isMobile)
                this.Pointer = 2;
        }

        internal void AdvancePointer(int i)
        {
            this.Pointer += i * 4;
        }

        private void CheckForExploits(string packetdata)
        {
        }
        internal void Init(uint messageID, byte[] body)
        {
            if (body == null)
            {
                body = new byte[0];
            }
            this.MessageId = (int)messageID;
            this.Body = body;
            this.Pointer = 0;
        }

        internal byte[] PlainReadBytes(int Bytes)
        {
            if (Bytes > this.RemainingLength)
            {
                Bytes = this.RemainingLength;
            }
            byte[] buffer = new byte[Bytes];
            int index = 0;
            for (int i = this.Pointer; index < Bytes; i++)
            {
                buffer[index] = this.Body[i];
                index++;
            }
            return buffer;
        }

        internal int PopFixedInt32()
        {
            int result = 0;
            if (!isMobile)
                int.TryParse(this.PopFixedString(Encoding.ASCII), out result);
            else
                int.TryParse(MobileBody[Pointer++], out result);
            return result;
        }

        internal string PopFixedString()
        {
            if (!isMobile)
                return this.PopFixedString(Encoding.Default);
            else
                return MobileBody[Pointer++];
        }

        internal string PopFixedString(Encoding encoding)
        {
            return encoding.GetString(this.ReadFixedValue());
        }

        internal Boolean PopWiredBoolean()
        {
            if (!isMobile)
            {
                if (this.RemainingLength > 0 && Body[Pointer++] == Convert.ToChar(1))
                {
                    return true;
                }
            }
            else
            {
                return MobileBody[Pointer++] == "true";
            }
            return false;
        }

        internal int PopWiredInt32()
        {
            if (isMobile)
                return int.Parse(MobileBody[Pointer++]);
            if (this.RemainingLength < 1)
            {
                return 0;
            }
            int num = HabboEncoding.DecodeInt32(this.PlainReadBytes(4));
            this.Pointer += 4;
            return num;
        }

        internal uint PopWiredUInt()
        {
            if (isMobile)
                return uint.Parse(MobileBody[Pointer++]);
            return uint.Parse(this.PopWiredInt32().ToString());
        }

        internal byte[] ReadBytes(int Bytes)
        {
            if (Bytes > this.RemainingLength)
            {
                Bytes = this.RemainingLength;
            }
            byte[] buffer = new byte[Bytes];
            for (int i = 0; i < Bytes; i++)
            {
                int num2;
                this.Pointer = (num2 = this.Pointer) + 1;
                buffer[i] = this.Body[num2];
            }
            return buffer;
        }

        internal byte[] ReadFixedValue()
        {
            int bytes = HabboEncoding.DecodeInt16(this.ReadBytes(2));
            return this.ReadBytes(bytes);
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "[", this.Header, "] BODY: ", Encoding.Default.GetString(this.Body).Replace(Convert.ToChar(0).ToString(), "[0]") });
        }
        public string GetBody()
        {
            return Encoding.Default.GetString(this.Body);
        }
        public uint Id
        {
            get
            {
                return (uint)this.MessageId;
            }
        }
        public int Length
        {
            get
            {
                return this.Body.Length;
            }
        }
        public int RemainingLength
        {
            get
            {
                return this.Body.Length - this.Pointer;
            }
        }
        public string Header
        {
            get
            {
                return Encoding.Default.GetString(Base64Encoding.Encodeuint((uint)this.MessageId, 2));
            }
        }
        
    }
}

