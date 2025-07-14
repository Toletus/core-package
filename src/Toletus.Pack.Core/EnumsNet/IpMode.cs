using System.ComponentModel;

namespace Toletus.Pack.Core.EnumsNet;

public enum IpMode
{
    [Description("Dynamic (DHCP)")]
    Dynamic,
    [Description("Fixed")]
    Fixed   
}