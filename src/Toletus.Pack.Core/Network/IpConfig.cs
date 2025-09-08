using System.Net;
using Toletus.Pack.Core.Network.Enums;

namespace Toletus.Pack.Core.Network;

public class IpConfig
{
    public IpMode? IpMode { get; set; }
    public IPAddress? FixedIp { get; set; }
    public IPAddress? SubnetMask { get; set; } 
}