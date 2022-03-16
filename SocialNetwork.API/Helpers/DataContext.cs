using Microsoft.EntityFrameworkCore;
using SocialNetwork.API.Entities.Comment;
using SocialNetwork.API.Entities.Notification;
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
    public DbSet<Follow> Follow { get; set; } 
    #endregion User

    #region Post
    public DbSet<Post> Post { get; set; }
    public DbSet<PostLike> PostLike { get; set; }
    public DbSet<PostShare> PostShare { get; set; }
    public DbSet<PostMedia> PostMedia { get; set; }
    public DbSet<PostSave> PostSave { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<CommentLike> CommentLike { get; set; }
    public DbSet<CommentMedia> CommentMedia { get; set; }
    #endregion Post

    #region Notification
    public DbSet<Notification> Notification { get; set; }
    #endregion Notification
}