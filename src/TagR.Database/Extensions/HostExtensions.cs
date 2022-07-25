using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TagR.Database.Extensions;

public static class HostExtensions
{
    public static void EnsureMigrated(this IHost host)
    {
        using var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var ctx = scope.ServiceProvider.GetRequiredService<TagRDbContext>();

        ctx.Database.Migrate();
    }
}