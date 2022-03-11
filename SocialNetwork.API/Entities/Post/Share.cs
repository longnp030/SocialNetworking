namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who shared which entity
/// <para>User - n : n : Entity -> Share</para>
/// </summary>
public class Share
{
    /// <summary>
    /// Entity's unique identifier
    /// <para>Reference to Entity.Id</para>
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Who shared the entity
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid FromId { get; set; }

    /// <summary>
    /// Who (User) / Which (Group) received the shared entity
    /// <para>Reference to User.Id / Group.Id</para>
    /// </summary>
    public Guid ToId { get; set; }

    /// <summary>
    /// Timestamp when entity is shared
    /// </summary>
    public DateTime Timestamp { get; set; }
}
