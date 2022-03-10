using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Models.User;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

/// <summary>
/// Provides API related to User
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    #region Properties
    /// <summary>
    /// Provide queries of User table to DB
    /// </summary>
    private readonly IUserService _userService;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    /// <param name="userService"></param>
    public UsersController(
        IUserService userService)
    {
        _userService = userService;
    }
    #endregion Constructor

    #region Methods
    /// <summary>
    /// Log user in
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    /// <param name="model">Auth model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data:
    /// <para>User's credentials and Jwt token if success</para>
    /// </returns>
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);
        return Ok(response);
    }

    /// <summary>
    /// Register new user account
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    /// <param name="model">Register model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: None
    /// </returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest model)
    {
        _userService.Register(model);
        return Ok(new { message = "Registration successful" });
    }

    /// <summary>
    /// Get all users
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: List of all users in DB
    /// </returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    /// <summary>
    /// Get a user by id
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: One user
    /// </returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }

    /// <summary>
    /// Update a user's credentials
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <param name="model">Credentials update model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: None
    /// </returns>
    [HttpPut("{id}/credentials")]
    public IActionResult UpdateCredentials(Guid id, UpdateCredentialsRequest model)
    {
        _userService.UpdateCredentials(id, model);
        return Ok(new { message = "User updated successfully" });
    }

    /// <summary>
    /// Delete a user by his/her id
    /// <para>Author: longnp</para>
    /// <para>Created: 10/03/2022</para>
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: None
    /// </returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted successfully" });
    }
    #endregion Methods
}
