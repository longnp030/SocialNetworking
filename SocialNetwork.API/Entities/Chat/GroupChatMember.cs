namespace SocialNetwork.API.Entities.Chat;

/// <summary>
/// Membership of a group chat
/// </summary>
public class GroupChatMember : Entity
{
    /// <summary>
    /// GroupChat's unique identifier
    /// </summary>
    public Guid GroupChatId { get; set; }

    /// <summary>
    /// User's unique identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Role of a user in the group chat
    /// </summary>
    public int Role { get; set; }
}
