using Microsoft.EntityFrameworkCore;
using SocialNetwork.API.Entities.Post;
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

    #region User
    public DbSet<User> User { get; set; }
    public DbSet<UserProfile> UserProfile { get; set; }
    public DbSet<UserSetting> UserSetting { get; set; }
    #endregion User

    #region Post
    public DbSet<Post> Post { get; set; }
    public DbSet<Share> Share { get; set; }
    public DbSet<Save> Save { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Like> Like { get; set; }
    public DbSet<Media> Media { get; set; }
    #endregion Post
}