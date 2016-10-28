using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Client;
using Client = Client.Client;

namespace Server
{
    class Packet
    {
    }

    static class PacketHandler
    {
        private static string ClientName;
        public static void Handle(byte[] packet, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            Console.WriteLine("[Server]: Received packet length {0} | type {1}", packetLength, packetType);

            Message msg;
            switch (packetType)
            {
                // receiving List<string>
                case 0:
                    msg = new Message(packet);
                    List<string> data = msg.ListData;
                    foreach (string s in data)
                    {
                        Console.WriteLine("[Server received] list string {0}", s);
                    }
                    SaveData(data);
                    break;
                // receiving string
                case 2000:
                    msg = new Message(packet);
                    string received = msg.Text;
                    Console.WriteLine("[Server received] string: {0}", received);
                    ClientName = msg.Text;
                    break;
                // receiving Object
                case 100:
                    msg = new Message(packet);
                    TestClass test = (TestClass) msg.obj;
                    Console.WriteLine("[Server received] TestClass string: {0}, TestClass int: {1}",test.aString,test.anInt);
                    break;
            }
        }

        private static void SaveData(List<string> data )
        {
            //@TODO FILE IO
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

        public ushort ReadUShort(int offset)
        {
            return BitConverter.ToUInt16(_buffer, offset);
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

        public string ReadString(int offset, int count)
        {
            return Encoding.UTF8.GetString(_buffer, offset, count);
        }

        public void WriteList(List<string> value, int offset)
        {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, value);

            _buffer = mStream.ToArray();
        }

        public List<string> ReadList(int offset, int count)
        {
            var mStream = new MemoryStream();
            var binFormatter = new BinaryFormatter();

            mStream.Write(_buffer, 0, _buffer.Length);
            mStream.Position = 0;

            List<string> dataList = binFormatter.Deserialize(mStream) as List<string>;

            return dataList;
        }

        public void WriteObject(object obj, int offset)
        {
            if (obj != null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms,obj);

                    Array.Copy(ms.ToArray(), 0, _buffer, offset, 1024);
                    
                }
            }
        }

        public Object ReadObject()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            ms.Write(_buffer, 0, _buffer.Length);
            ms.Seek(2, SeekOrigin.Begin);
            Object obj = (Object) bf.Deserialize(ms);
            return obj;
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
        private Object _obj;
        
        public Message(string message) : base(length: (ushort)(4 + message.Length), type: 2000)
        {
            Text = message;
        }

        public Message(List<string> message) : base(length: (ushort)1024, type: 500)
        {
            ListData = message;
        }

        public Message(byte[] packet) : base(packet)
        {
            
        }

        public Message(Object obj) : base(length: (ushort)1024, type: 100)
        {
            this.obj = obj;
        }

        public string Text
        {
            get { return ReadString(4, Data.Length - 4); }
            set
            {
                _message = value;
                WriteString(value, 4);
            }
        }

        public List<string> ListData
        {
            get { return ReadList(4, 1024); }
            set
            {
                _data = value;
                WriteList(value, 4);
            }
        }

        public Object obj
        {
            get { return ReadObject(); }
            set
            {
                _obj = value;
                WriteObject(value, 4);
            }
        }
    }
}
