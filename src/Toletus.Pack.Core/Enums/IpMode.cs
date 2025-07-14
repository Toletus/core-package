using System.ComponentModel;

namespace Toletus.Pack.Core.Enums;

public enum IpMode
{
    [Description("Dynamic (DHCP)")]
    Dynamic,
    [Description("Fixed")]
    Fixed   
}