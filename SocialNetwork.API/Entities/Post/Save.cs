namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who saved which post
/// <para>User - n : n - Entity -> Save</para>
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class Save : Entity
{
    /// <summary>
    /// Saved entity's unique identifier
    /// <para>Reference to Post.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// User who saved post's unique identifier
    /// <para>Reference to User.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid UserId { get; set; }
}
