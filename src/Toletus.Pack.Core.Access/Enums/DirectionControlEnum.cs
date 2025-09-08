namespace Toletus.Pack.Core.Access.Enums;

public enum DirectionControlEnum
{
    Blocked,    // travado (não gira)
    Free,       // livre (gira sem autorização)
    Controlled  // controlado (exige autorização -> pulso/relay)
}