using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Models.Post;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

/// <summary>
/// Provides API related to Post
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    #region Properties
    /// <summary>
    /// Provide queries of Post table to DB
    /// </summary>
    private readonly IPostService _postService;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="postService"></param>
    public PostsController(
        IPostService postService)
    {
        _postService = postService;
    }
    #endregion Constructor

    #region Methods

    /// <summary>
    /// Get a post by its id
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>A post with provided id</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var post = _postService.GetById(id);
        return Ok(post);
    }

    /// <summary>
    /// Get all likes for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List of likes this post</returns>
    [HttpGet("{id}/likes")]
    public IActionResult GetAllLikesByPostId(Guid id)
    {
        var likes = _postService.GetAllLikesByPostId(id);
        return Ok(likes);
    }

    /// <summary>
    /// Get all comments' Id for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List all comments' Id for this post</returns>
    [HttpGet("{id}/comments")]
    public IActionResult GetAllCommentsByPostId(Guid id)
    {
        var comments = _postService.GetAllCommentsByPostId(id);
        return Ok(comments);
    }

    /// <summary>
    /// Get number of comments for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List number of comments for this post</returns>
    [HttpGet("{id}/comments/count")]
    public IActionResult GetCommentCountByPostId(Guid id)
    {
        var numberOfComments = _postService.GetAllCommentsByPostId(id).Count();
        return Ok(numberOfComments);
    }

    /// <summary>
    /// Check if this auth user liked this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>True if liked otherwise false</returns>
    [HttpGet("{id}/likes/{userId}")]
    public IActionResult IsAuthUserLiked(Guid id, Guid userId)
    {
        var liked = _postService.IsAuthUserLiked(id, userId);
        return Ok(liked);
    }

    /// <summary>
    /// Auth user likes this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost("{id}/likes/{userId}/like")]
    public IActionResult Like(Guid id, Guid userId)
    {
        _postService.Like(id, userId);
        return Ok(new { Message = "Liked" });
    }

    /// <summary>
    /// Auth user unliked this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost("{id}/likes/{userId}/unlike")]
    public IActionResult Unlike(Guid id, Guid userId)
    {
        _postService.Unlike(id, userId);
        return Ok(new { Message = "Liked" });
    }

    /// <summary>
    /// Get all shares for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List all shares for this post</returns>
    [HttpGet("{id}/shares")]
    public IActionResult GetAllSharesByPostId(Guid id)
    {
        var shares = _postService.GetAllSharesByPostId(id);
        return Ok(shares);
    }

    /// <summary>
    /// Create new post
    /// </summary>
    /// <param name="model">Fields to create a post</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost]
    public IActionResult Create([FromBody] CreatePostRequest model)
    {
        _postService.Create(model);
        return Ok(new { Message = "Posted successfully" });
    }

    /// <summary>
    /// Edit a post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="model">Post's new information</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPatch("{id}/edit")]
    public IActionResult Edit(Guid id, [FromBody] CreatePostRequest model)
    {
        _postService.Edit(id, model);
        return Ok();
    }

    /// <summary>
    /// Delete a post by its id
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _postService.Delete(id);
        return Ok();
    }

    #endregion Methods
}
