using Remora.Commands.Parsers;
using Remora.Results;
using TagR.Application.ResultErrors;
using TagR.Domain.Moderation;

namespace TagR.Bot.Commands.Parsers;

public class BlockedActionParser : AbstractTypeParser<BlockedAction>
{
    public override ValueTask<Result<BlockedAction>> TryParseAsync(string token, CancellationToken ct = default)
    {
        BlockedAction output = BlockedAction.None;

        foreach (var ch in token)
        {
            switch (ch)
            {
               case 'm':    
                    output |= BlockedAction.TagModify;
                    break;
               case 'i':    
                    output |= BlockedAction.TagInvoke;
                    break;
                default:
                    return new ValueTask<Result<BlockedAction>>(Result<BlockedAction>.FromError(new ParserError(ch.ToString())));
            }
        }

        return new ValueTask<Result<BlockedAction>>(output);
    }
}
