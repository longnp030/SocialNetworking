namespace SocialNetwork.API.Entities.User;

/// <summary>
/// Universal setting for user
/// <para>User - 1 : 1 - UserSetting</para>
/// <para>Boolean value for all bool attributes:</para>
/// <para>0 - show; 1 - hide</para>
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class UserSetting
{
    /// <summary>
    /// User's unique identifier
    /// <para>Reference to User.Id</para>
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User's user interface display language
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Language { get; set; }

    /// <summary>
    /// Show user's online status
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public bool ShowOnlineStatus { get; set; }

    /// <summary>
    /// Show who user followed
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public bool ShowFollowers { get; set; }

    /// <summary>
    /// Show who followed user
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public bool ShowFollowees { get; set; }

    /// <summary>
    /// Show user's location
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public bool ShowLocation { get; set; }

    /// <summary>
    /// Show user's dob
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public bool ShowDateOfBirth { get; set; }

    /// <summary>
    /// Show user's gender
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public bool ShowGender { get; set; }

    /// <summary>
    /// Future user's posts privacy
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public bool PostPrivacy { get; set; }
}
