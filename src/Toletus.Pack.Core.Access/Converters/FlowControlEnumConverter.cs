using Toletus.Pack.Core.Access.Enums;
using Toletus.Pack.Core.Access.Logic;
using Toletus.Pack.Core.Access.Logic.Enums;

namespace Toletus.Pack.Core.Access.Converters;

public static class FlowControlEnumConverter
{
    /// <summary>
    /// Converts entry and exit configurations to the corresponding flow type
    /// </summary>
    /// <param name="entryMode">Entry control mode</param>
    /// <param name="exitMode">Exit control mode</param>
    /// <returns>Flow type based on the configurations</returns>
    public static FlowControlEnum ConvertToFlow(DirectionPolicyEnum entryMode, DirectionPolicyEnum exitMode) =>
        (entryMode, exitMode) switch
        {
            (DirectionPolicyEnum.Blocked, DirectionPolicyEnum.Blocked) => FlowControlEnum.EntryAndExitBlocked,
            
            (DirectionPolicyEnum.Blocked, _) => FlowControlEnum.ExitWithEntryBlocked,
            (_, DirectionPolicyEnum.Blocked) => FlowControlEnum.EntryWithExitBlocked,
            
            (DirectionPolicyEnum.Controlled, DirectionPolicyEnum.Free) => FlowControlEnum.EntryWithExitFree,
            (DirectionPolicyEnum.Free, DirectionPolicyEnum.Controlled) => FlowControlEnum.ExitWithEntryFree,
            
            (DirectionPolicyEnum.Controlled, DirectionPolicyEnum.Controlled) => FlowControlEnum.EntryAndExitFree,
            (DirectionPolicyEnum.Free, DirectionPolicyEnum.Free) => FlowControlEnum.EntryAndExitFree,
            
            _ => FlowControlEnum.EntryExit
        };
}