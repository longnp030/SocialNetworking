namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Post's images, videos, ...
/// <para>Post - 1 : n - PostMedia</para>
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class PostMedia
{
    /// <summary>
    /// Media's unique identifier
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Media's post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Media path
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Media { get; set; }
}
