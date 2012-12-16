using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace SendMagicPacket
{
	class Program
	{
		static void Main(string[] args)
		{
			if(args.Length<1){
				Console.WriteLine("Usage: SendMagicPacket XX-XX-XX-XX-XX-XX");
				return;
			}
			var magicPacket=new List<byte>();
			magicPacket.AddRange(new byte[]{0xFF,0xFF,0xFF,0xFF,0xFF,0xFF});
			try{
				var macAddress=PhysicalAddress.Parse(args[0].ToUpper()).GetAddressBytes();
				for(int i=0;i<16;i++) magicPacket.AddRange(macAddress);
				Console.WriteLine("Send magic packet to "+string.Join("-",macAddress.Select((b)=>b.ToString("X2"))));
			}catch(Exception){
				Console.WriteLine("Invalid address format");
				return;
			}
			var socket=new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
			socket.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.Broadcast,true);
			socket.SendTo(magicPacket.ToArray(),new IPEndPoint(IPAddress.Broadcast,7));
			socket.Close();
		}
	}
}
