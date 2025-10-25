namespace Toletus.Pack.Core.Access.Enums;

/// <summary>
/// Physical direction of passage through a gate/barrier
/// Works for turnstiles, flap barriers, sliding gates, speed gates, etc.
/// </summary>
public enum PassagePhysicalDirectionEnum
{
    /// <summary>
    /// Passage from Side A to Side B
    /// Examples: 
    /// - Turnstile: clockwise rotation
    /// - Flap barrier: left sensor → right sensor
    /// - Gate: sensor 1 → sensor 2
    /// </summary>
    SideAtoB = 0,
    
    /// <summary>
    /// Passage from Side B to Side A
    /// Examples:
    /// - Turnstile: counter-clockwise rotation  
    /// - Flap barrier: right sensor → left sensor
    /// - Gate: sensor 2 → sensor 1
    /// </summary>
    SideBtoA = 1
}