using Toletus.Pack.Core.Access.Enums;

namespace Toletus.Pack.Core.Access.Converters;

public static class FlowControlConverter
{
    /// <summary>
    /// Converts entry and exit configurations to the corresponding flow type
    /// </summary>
    /// <param name="entryMode">Entry control mode</param>
    /// <param name="exitMode">Exit control mode</param>
    /// <returns>Flow type based on the configurations</returns>
    public static FlowControlEnum ConvertToFlow(DirectionControlEnum entryMode, DirectionControlEnum exitMode) =>
        (entryMode, exitMode) switch
        {
            (DirectionControlEnum.Blocked, DirectionControlEnum.Blocked) => FlowControlEnum.EntryAndExitBlocked,
            
            (DirectionControlEnum.Blocked, _) => FlowControlEnum.ExitWithEntryBlocked,
            (_, DirectionControlEnum.Blocked) => FlowControlEnum.EntryWithExitBlocked,
            
            (DirectionControlEnum.Controlled, DirectionControlEnum.Free) => FlowControlEnum.EntryWithExitFree,
            (DirectionControlEnum.Free, DirectionControlEnum.Controlled) => FlowControlEnum.ExitWithEntryFree,
            
            (DirectionControlEnum.Controlled, DirectionControlEnum.Controlled) => FlowControlEnum.EntryAndExitFree,
            (DirectionControlEnum.Free, DirectionControlEnum.Free) => FlowControlEnum.EntryAndExitFree,
            
            _ => FlowControlEnum.EntryExit
        };
}