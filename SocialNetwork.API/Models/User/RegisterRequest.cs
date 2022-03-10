using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.API.Models.User;

/// <summary>
/// Register model equivalents to register form in Frontend
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// User's email address
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    [Required]
    public String Email { get; set; }

    /// <summary>
    /// User's password
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    [Required]
    public String Password { get; set; }
}
