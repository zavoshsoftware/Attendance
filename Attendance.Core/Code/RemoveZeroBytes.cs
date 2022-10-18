using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
    class RemoveZeroBytes
    {
    public byte[] Decode(byte[] packet)
    {
        var i = packet.Length - 1;
        while (packet[i] == 0)
        {
            --i;
        }
        var temp = new byte[i + 1];
        Array.Copy(packet, temp, i + 1); 
        return temp;
    }
} 