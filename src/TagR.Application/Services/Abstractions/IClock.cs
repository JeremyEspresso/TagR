namespace TagR.Application.Services.Abstractions;

public interface IClock
{
    DateTime UtcNow { get; }
}

