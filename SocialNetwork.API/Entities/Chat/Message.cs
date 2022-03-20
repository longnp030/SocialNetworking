namespace SocialNetwork.API.Entities.Chat;

/// <summary>
/// Message from a user to a user or to a group chat
/// </summary>
public class Message : Entity
{
    /// <summary>
    /// User who sent message unique identifier
    /// </summary>
    public Guid FromId { get; set; }

    /// <summary>
    /// Can either be a user or a group chat
    /// </summary>
    public Guid ToId { get; set; }

    /// <summary>
    /// Message's parent if this is a reply message
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Text of message
    /// </summary>
    public String Text { get; set; }
}
