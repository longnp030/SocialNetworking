namespace SocialNetwork.API.Entities;

/// <summary>
/// Base entity
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class Entity
{
    /// <summary>
    /// Entity's unique identifier
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Entity created timestamp
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public DateTime Timestamp { get; set; }
}
