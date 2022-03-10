namespace SocialNetwork.API.Models.User;

/// <summary>
/// Defines what will be send back to Backend after user requests log in
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class AuthenticateResponse
{
    /// <summary>
    /// User's unique identifier
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's email address
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String Email { get; set; }

    /// <summary>
    /// Jwt token
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    public String JwtToken { get; set; }
}