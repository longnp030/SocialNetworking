namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Post's images, videos, ...
/// <para>Post - 1 : n - Media</para>
/// </summary>
public class Media
{
    /// <summary>
    /// Media's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Media's (post/comment)'s unique identifier
    /// <para>Reference to Post.Id</para>
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// Media path
    /// </summary>
    public String Path { get; set; }
}
