using AutoMapper;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Entities.Post;
using SocialNetwork.API.Helpers;

namespace SocialNetwork.API.Services;

/// <summary>
/// Interface provides methods to CommentService class
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Get a comment by its id
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>A comment with provided id</returns>
    Comment GetById(Guid id);

    /// <summary>
    /// Get all comments of this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List of comments of this comment</returns>
    IEnumerable<Comment> GetAllChildrenByCommentId(Guid id);

    /// <summary>
    /// Get all likes for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List of likes for this comment</returns>
    IEnumerable<Like> GetAllLikesByCommentId(Guid id);

    /// <summary>
    /// Get all shares for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List all shares for this comment</returns>
    IEnumerable<Share> GetAllSharesByCommentId(Guid id);
}

/// <summary>
/// Provides queries and helpers of Comment table to DB
/// </summary>
public class CommentService : ICommentService
{
    #region Properties
    private DataContext _context;
    private IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="context"></param>
    /// <param name="jwtUtils"></param>
    /// <param name="mapper"></param>
    public CommentService(
        DataContext context,
        IJwtUtils jwtUtils,
        IMapper mapper)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }
    #endregion Constructor

    #region Methods
    public Comment GetById(Guid id)
    {
        var comment = _context.Comment.Find(id);
        return comment;
    }

    public IEnumerable<Comment> GetAllChildrenByCommentId(Guid id)
    {
        var comments = _context.Comment
            .Where(c => c.ParentId == id).ToList();
        return comments;
    }

    public IEnumerable<Like> GetAllLikesByCommentId(Guid id)
    {
        var likes = _context.Like
            .Where(l => l.EntityId == id).ToList();
        return likes;
    }

    public IEnumerable<Share> GetAllSharesByCommentId(Guid id)
    {
        var shares = _context.Share
            .Where(s => s.EntityId == id).ToList();
        return shares;
    }

    #endregion Methods
}
