using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TagR.Database
{
    public class TagRDbContextFactory : IDesignTimeDbContextFactory<TagRDbContext>
    {
        public TagRDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TagRDbContext>();
            optionsBuilder.UseNpgsql();
            return new TagRDbContext(optionsBuilder.Options);
        }
    }
}
