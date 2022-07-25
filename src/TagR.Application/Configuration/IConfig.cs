namespace TagR.Application.Configuration;

public interface IConfig
{
    DiscordConfiguration Discord { get; }

    DatabaseConfiguration Database { get; }
}

