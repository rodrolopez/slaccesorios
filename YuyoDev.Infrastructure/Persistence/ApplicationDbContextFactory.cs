using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using YuyoDev.Infrastructure.Persistence;

namespace YuyoDev.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // ¡OJO! Poné acá el mismo puerto que te funcionó en Docker (el 5439)
        optionsBuilder.UseNpgsql("Host=localhost;Port=5439;Database=yuyodev_boilerplate;Username=yuyo_user;Password=yuyo_pass");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}