namespace SocialNetwork.API.Entities.User;

/// <summary>
/// User's profile, not credentials
/// <para>User - 1 : 1 - UserProfile</para>
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class UserProfile
{
    /// <summary>
    /// User's unique identifier
    /// <para>Reference to User.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User's username
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Name { get; set; }

    /// <summary>
    /// User's avatar url
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Avatar { get; set; }

    /// <summary>
    /// User's cover url
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>

    public String Cover { get; set; }

    /// <summary>
    /// User's date of birth
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String DateOfBirth { get; set; }

    /// <summary>
    /// User's current location
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String CurrentLocation { get; set; }

    /// <summary>
    /// User's self introduction
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String SelfIntroduction { get; set; }

    /// <summary>
    /// User's gender
    /// </summary>
    public bool Gender { get; set; }
}
