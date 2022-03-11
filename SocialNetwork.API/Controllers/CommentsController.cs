using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

/// <summary>
/// Provides API related to Comment
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    #region Properties
    /// <summary>
    /// Provide queries of Comment table to DB
    /// </summary>
    private readonly ICommentService _commentService;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="commentService"></param>
    public CommentsController(
        ICommentService commentService)
    {
        _commentService = commentService;
    }
    #endregion Constructor

    #region Methods

    /// <summary>
    /// Get a comment by its id
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>A comment with provided id</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var comment = _commentService.GetById(id);
        return Ok(comment);
    }

    /// <summary>
    /// Get all likes for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List of likes this comment</returns>
    [HttpGet("{id}/likes")]
    public IActionResult GetAllLikesByCommentId(Guid id)
    {
        var likes = _commentService.GetAllLikesByCommentId(id);
        return Ok(likes);
    }

    /// <summary>
    /// Get all comments for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List all comments for this comment</returns>
    [HttpGet("{id}/comments")]
    public IActionResult GetAllChildrenByCommentId(Guid id)
    {
        var comments = _commentService.GetAllChildrenByCommentId(id);
        return Ok(comments);
    }

    /// <summary>
    /// Get all shares for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List all shares for this comment</returns>
    [HttpGet("{id}/shares")]
    public IActionResult GetAllSharesByCommentId(Guid id)
    {
        var shares = _commentService.GetAllSharesByCommentId(id);
        return Ok(shares);
    }

    #endregion Methods
}

