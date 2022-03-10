using System.Text.Json.Serialization;

namespace SocialNetwork.API.Entities;

/// <summary>
/// User class
/// <para>Only handling user's credentials</para>
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class User
{
    /// <summary>
    /// User's unique identity code
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's email address
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User's hashed password
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    [JsonIgnore]
    public string PasswordHash { get; set; }

    /// <summary>
    /// User's registration timestamp
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public DateTime Timestamp { get; set; }
}
