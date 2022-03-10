namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Comment class
/// <para>Post - 1 : n - Comment</para>
/// <para>Comment - 1 : n - Comment</para>
/// </summary>
public class Comment : Entity
{
    /// <summary>
    /// Comment's author's unique identifier
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// If root comment -> Parent = Post
    /// <para>Reference to Post.Id</para>
    /// If node / leaf comment -> Parent = Comment
    /// <para>Reference to Comment.Id</para>
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// Comment's content
    /// </summary>
    public String Text { get; set; }
}
