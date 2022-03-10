namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who shared which post
/// <para>User - n : n : Post -> PostShare</para>
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class PostShare
{
    /// <summary>
    /// Post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Who shared the post
    /// <para>Reference to User.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid FromId { get; set; }

    /// <summary>
    /// Who (User) / Which (Group) received the shared post
    /// <para>Reference to User.Id / Group.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid ToId { get; set; }

    /// <summary>
    /// Timestamp when post is shared
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public DateTime Timestamp { get; set; }
}
