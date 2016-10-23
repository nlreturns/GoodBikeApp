using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Packet
    {
    }

    static class PacketHandler
    {
        public static void Handle(byte[] packet, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            Console.WriteLine("Received packet length {0} | type {1}", packetLength, packetType);

            Message msg;
            switch (packetType)
            {
                // how to handle RECEIVING a message.
                case 500:
                    msg = new Message(packet);
                    List<string> data = msg.ListData;
                    break;
                case 2000:
                    msg = new Message(packet);
                    string received = msg.Text;
                    break;
            }
        }
    }

    class PacketStructure
    {
        private byte[] _buffer;

        public PacketStructure(ushort length, ushort type)
        {
            _buffer = new byte[length];
            WriteUShort(length, 0);
            WriteUShort(type, 2);
        }

        public PacketStructure(byte[] packet)
        {
            _buffer = packet;
        }

        public void WriteUShort(ushort value, int offset)
        {
            byte[] tempBuf = new byte[2];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, 2);
        }

        public void WriteUInt(uint value, int offset)
        {
            byte[] tempBuf = new byte[4];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, 4);
        }

        public void WriteString(string value, int offset)
        {
            byte[] tempBuf = new byte[value.Length];
            tempBuf = Encoding.UTF8.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, value.Length);
        }

        public void WriteList(List<string> value, int offset)
        {
            byte[] tempBuf = value.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s)).ToArray();
            int length = value.Sum(item => Encoding.Default.GetByteCount(item.ToString()));
            Buffer.BlockCopy(tempBuf,0,_buffer,offset,length);
        }
        
        public void RewriteHeader(ushort length, ushort type)
        {
            WriteUShort(length, 0);
            WriteUShort(type, 2);
        }

        public byte[] Data
        {
            get { return _buffer; }
        }
    }

    class Message : PacketStructure
    {
        private string _message;
        private List<string> _data;
        
        public Message(string message) : base(length: (ushort)(4 + message.Length), type: 2000)
        {
            Text = message;
        }

        public Message(List<string> message) : base(length: (ushort)10, type: 500)
        {
            WriteList(message,0);
            ListData = message;
        }

        public Message(byte[] packet) : base(packet)
        {
            
        }

        public string Text
        {
            /* return ReadString(4, Data.Length - 4)
             *  ^ Server read
             */
            get { return _message; }
            set
            {
                _message = value;
                WriteString(value, 4);
            }
        }

        public List<string> ListData
        {
            get { return _data; }
            set
            {
                _data = value;
                WriteList(value, 4);
            }
        }
    }
}
