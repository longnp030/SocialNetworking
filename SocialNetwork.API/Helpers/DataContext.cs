using Microsoft.EntityFrameworkCore;
using SocialNetwork.API.Entities.User;

namespace SocialNetwork.API.Helpers;

/// <summary>
/// Connects to DB services
/// <para>Provides entities calls to DB</para>
/// </summary>
public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server database
        options.UseMySql(Configuration.GetConnectionString("WebApiDatabase"), ServerVersion.AutoDetect(Configuration.GetConnectionString("WebApiDatabase")));
    }

    public DbSet<User> User { get; set; }
}