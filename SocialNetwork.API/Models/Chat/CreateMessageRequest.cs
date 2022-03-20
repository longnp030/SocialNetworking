namespace SocialNetwork.API.Models.Chat;

public class CreateMessageRequest
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

    public IEnumerable<String> MediaPaths { get; set; }
}
