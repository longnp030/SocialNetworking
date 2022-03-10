namespace SocialNetwork.API.Models.User;

/// <summary>
/// Update credentials model equivalents to update form in Frontend
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class UpdateCredentialsRequest
{
    /// <summary>
    /// User's email address
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Email { get; set; }

    /// <summary>
    /// User's new password
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Password { get; set; }
}