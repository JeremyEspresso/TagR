using TagR.Application.Services.Abstractions;

namespace TagR.Application.Services;
public class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}

