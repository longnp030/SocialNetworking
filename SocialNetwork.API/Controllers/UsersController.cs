using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Models.User;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

/// <summary>
/// Provides API related to User
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

    /// <summary>
    /// Get user profile
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>User profile</returns>
    [HttpGet("{id}/profile")]
    public IActionResult GetProfile(Guid id)
    {
        var profile = _userService.GetProfile(id);
        return Ok(profile);
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    /// <param name="id">User's unque identifier</param>
    /// <param name="model">Update profile model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPatch("{id}/profile")]
    public IActionResult UpdateProfile(Guid id, [FromBody] UpdateProfileRequest model)
    {
        _userService.UpdateProfile(id, model);
        return Ok(new { message = "Profile updated successfully!" });
    }

    /// <summary>
    /// Update user settings
    /// </summary>
    /// <param name="id">User's unque identifier</param>
    /// <param name="model">Update setting model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPatch("{id}/setting")]
    public IActionResult UpdateSetting(Guid id, UpdateSettingRequest model)
    {
        _userService.UpdateSetting(id, model);
        return Ok(new { message = "Settings updated successfully!" });
    }

    /// <summary>
    /// Get all posts posted/shared by this user
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of all posts posted/shared by this user</returns>
    [HttpGet("{id}/posts")]
    public IActionResult GetAllPostsByUserId(Guid id)
    {
        var posts = _userService.GetAllPostsByUserId(id);
        return Ok(posts);
    }

    /// <summary>
    /// Get all posts saved by this user
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of all posts saved by this user</returns>
    [HttpGet("{id}/posts/saved")]
    public IActionResult GetAllSavedPostsByUserId(Guid id)
    {
        var savedPosts = _userService.GetAllSavedPostsByUserId(id);
        return Ok(savedPosts);
    }

    /// <summary>
    /// Populate user's newsfeed
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of posts to display in user's newsfeed</returns>
    [HttpGet("{id}/feed")]
    public IActionResult GetFeed(Guid id)
    {
        var feed = _userService.GetFeed(id);
        return Ok(feed);
    }

    
    #endregion Methods
}
