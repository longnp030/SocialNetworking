namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who saved which post
/// <para>User - n : n - Post 0 -> PostSave</para>
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class PostSave
{
    /// <summary>
    /// Saved post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// User who saved post's unique identifier
    /// <para>Reference to User.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid UserId { get; set; }
}
