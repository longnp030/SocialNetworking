using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.API.Models.User;

/// <summary>
/// Auth model equivalents to Log in form in Frontend
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class AuthenticateRequest
{
    /// <summary>
    /// User's email address
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// User's password
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    [Required]
    public string Password { get; set; }
}
