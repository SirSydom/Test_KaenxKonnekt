using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaenx.Konnect.Remote
{
    public class TunnelRequest : IRemoteMessage
    {
        public MessageCodes MessageCode { get; } = MessageCodes.TunnelRequest;
        public int SequenceNumber { get; set; } = -1;
        public int ChannelId { get; set; } = 0;
        public int ConnId { get; set; } = 0;
        public string Group { get; set; } = "";
        public TunnelTypes Type { get; set; }
        public byte[] Data;

        public TunnelRequest() { }
        public TunnelRequest(byte[] data)
        {
            Data = data;
        }


        public void Parse(byte[] buffer)
        {
            SequenceNumber = buffer[1];
            Group = Encoding.UTF8.GetString(buffer.Skip(6).Take(buffer[5]).ToArray());
            ChannelId = buffer[2];
            ConnId = buffer[3];
            Type = (TunnelTypes)buffer[4];
            Data = buffer.Skip(6 + buffer[5]).ToArray();
        }


        public ArraySegment<byte> GetBytes()
        {
            byte[] group = Encoding.UTF8.GetBytes(Group);
            byte[] bytes = new byte[Data.Length + group.Length + 6];
            bytes[0] = Convert.ToByte(MessageCode);
            bytes[1] = Convert.ToByte(SequenceNumber);
            bytes[2] = Convert.ToByte(ChannelId);
            bytes[3] = Convert.ToByte(ConnId);
            bytes[4] = Convert.ToByte(Type);
            bytes[5] = Convert.ToByte(group.Length);
            group.CopyTo(bytes, 6); 
            Data.CopyTo(bytes, 6 + group.Length);


            //TODO ChannelId einpflegen!!

            return new ArraySegment<byte>(bytes);
        }
    }

    public enum TunnelTypes
    {
        Connect,
        Disconnect,
        State,
        Tunnel,
        Response
    }
}
