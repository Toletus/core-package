using System.ComponentModel;

namespace Toletus.Pack.Core.Network.Enums;

public enum IpMode
{
    [Description("Dynamic (DHCP)")]
    Dynamic,
    [Description("Fixed")]
    Fixed   
}