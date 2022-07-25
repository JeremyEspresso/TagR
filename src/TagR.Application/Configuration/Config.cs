namespace TagR.Application.Configuration;

public class Config : IConfig
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Config() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DiscordConfiguration Discord { get; init; }

    public DatabaseConfiguration Database { get; init; }

    public static IConfig Get() =>
        new Config()
        {
            Discord = new DiscordConfiguration
            {
                Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN")!,
                CommandPrefix = Environment.GetEnvironmentVariable("DISCORD_COMMAND_PREFIX")!,
            },
            Database = new DatabaseConfiguration
            {
                ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")!,
            }
        };
}
