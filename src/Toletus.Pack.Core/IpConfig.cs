using System.Net;
using Toletus.Pack.Core.Enums;

namespace Toletus.Pack.Core;

public class IpConfig
{
    public IpMode? IpMode { get; set; }
    public IPAddress? FixedIp { get; set; }
    public IPAddress? SubnetMask { get; set; } 
}