namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Category
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class Category
{
    /// <summary>
    /// Category's unique identifier
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Category's name
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Name { get; set; }
}
