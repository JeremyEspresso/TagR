using TagR.Application.ResultErrors;
using TagR.Bot.Commands.Parsers;
using TagR.Domain.Moderation;

namespace TagR.UnitTests.Parsers;

public class BlockedActionParserTests
{
    private readonly BlockedActionParser _sut;
    
    public BlockedActionParserTests()
    {
        _sut = new BlockedActionParser();
    }

    [Fact] 
    public async Task TryParseAsync_ValidTokenTwoActions_ShouldReturnSuccessResult()
    {
        const string token = "mi";
        BlockedAction expected = BlockedAction.TagModify | BlockedAction.TagInvoke;

        var result = await _sut.TryParseAsync(token);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Entity);
    }
    
    [Fact] 
    public async Task TryParseAsync_ValidTokenOneAction_ShouldReturnSuccessResult()
    {
        const string token = "i";
        BlockedAction expected = BlockedAction.TagInvoke;

        var result = await _sut.TryParseAsync(token);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Entity);
    }
    
    [Fact] 
    public async Task TryParseAsync_Invalid_Token_ShouldReturnParserError()
    {
        const string token = "k";
        var expected = $"Unknown token `{token}`";

        var result = await _sut.TryParseAsync(token);
        
        Assert.False(result.IsSuccess);
        
        Assert.IsType<ParserError>(result.Error);
        Assert.Equal(expected, result!.Error!.Message);
    }
}