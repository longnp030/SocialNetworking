namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who liked which post
/// <para>User - n : n : Post -> PostLike</para>
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class PostLike
{
    /// <summary>
    /// Post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// User's unique identifier
    /// <para>Reference to User.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Timestamp user liked the post
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public DateTime Timestamp { get; set; }
}
