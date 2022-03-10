namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Post class
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class Post
{
    /// <summary>
    /// Post's unique identifier
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Post's author's unique identifier
    /// <para>Reference to User.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid UserId{ get; set; }

    /// <summary>
    /// Group's unique identifier if post is posted in a group
    /// <para>Default</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// Post's privacy
    /// <para>0 - public / public group</para>
    /// <para>1 - private / only me</para>
    /// <para>2 - group scoped (private group)</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public int Privacy { get; set; }

    /// <summary>
    /// Post's text content
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Text { get; set; }

    /// <summary>
    /// Post's check in location
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Location { get; set; }

    /// <summary>
    /// Post's timestamp
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public DateTime Timestamp { get; set; }
}
