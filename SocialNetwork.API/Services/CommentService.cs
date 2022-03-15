using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Comment;
using SocialNetwork.API.Helpers;
using SocialNetwork.API.Hubs;
using SocialNetwork.API.Models.Post;

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
    IEnumerable<CommentLike> GetAllLikesByCommentId(Guid id);

    /// <summary>
    /// Check if this auth user liked this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>True if liked otherwise false</returns>
    bool IsAuthUserLiked(Guid id, Guid userId);

    /// <summary>
    /// Auth user likes this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    void Like(Guid id, Guid userId);

    /// <summary>
    /// Auth user unliked this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    void Unlike(Guid id, Guid userId);

    /// <summary>
    /// Get comment's media
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List of media attached to this comment</returns>
    IEnumerable<String> GetMedia(Guid id);

    /// <summary>
    /// Create new comment
    /// </summary>
    /// <param name="model">Required fields to create new comment form</param>
    void Create(CreateCommentRequest model);

    /// <summary>
    /// Edit a comment
    /// </summary>
    /// <param name="model">Required fields to edit a comment form</param>
    void Edit(Guid id, CreateCommentRequest model);

    /// <summary>
    /// Delete a comment by its id
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    void Delete(Guid id);
}

/// <summary>
/// Provides queries and helpers of Comment table to DB
/// </summary>
public class CommentService : ICommentService
{
    #region Properties
    private DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<PostHub, IPostHub> _postHubContext;
    private readonly IHubContext<CommentHub, ICommentHub> _commentHubContext;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public CommentService(
        DataContext context,
        IMapper mapper,
        IHubContext<PostHub, IPostHub> postHubContext,
        IHubContext<CommentHub, ICommentHub> commentHubContext)
    {
        _context = context;
        _mapper = mapper;
        _postHubContext = postHubContext;
        _commentHubContext = commentHubContext;
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

    public IEnumerable<CommentLike> GetAllLikesByCommentId(Guid id)
    {
        var likes = _context.CommentLike
            .Where(l => l.CommentId == id).ToList();
        return likes;
    }

    public bool IsAuthUserLiked(Guid id, Guid userId)
    {
        return _context.CommentLike
            .Where(l => l.CommentId == id)
            .Any(l => l.UserId == userId);
    }

    public void Like(Guid id, Guid userId)
    {
        var like = new CommentLike
        {
            CommentId = id,
            UserId = userId,
            Timestamp = DateTime.Now,
        };
        _context.CommentLike.Add(like);
        _context.SaveChanges();

        _commentHubContext.Clients.All.Reaction(id, userId, true);
    }

    public void Unlike(Guid id, Guid userId)
    {
        var like = _context.CommentLike
            .Where(l => l.CommentId == id)
            .Single(l => l.UserId == userId);
        _context.CommentLike.Remove(like);
        _context.SaveChanges();

        _commentHubContext.Clients.All.Reaction(id, userId, false);
    }

    public IEnumerable<String> GetMedia(Guid id)
    {
        var mediaPaths = _context.CommentMedia
            .Where(pm => pm.CommentId == id)
            .Select(pm => pm.Path);
        return mediaPaths;
    }

    public void Create(CreateCommentRequest model)
    {
        var comment = _mapper.Map<Comment>(model);
        comment.Id = Guid.NewGuid();
        comment.Timestamp = DateTime.Now;
        _context.Comment.Add(comment);
        _context.SaveChanges();

        if (model.MediaPaths.Any())
        {
            foreach (var mediaPath in model.MediaPaths)
            {
                var commentMedia = new CommentMedia
                {
                    Id = Guid.NewGuid(),
                    CommentId = comment.Id,
                    Path = mediaPath
                };
                _context.CommentMedia.Add(commentMedia);
                _context.SaveChanges();
            }
        }

        _postHubContext.Clients.Group(model.PostId.ToString()).Comment(comment);
    }

    public void Edit(Guid id, CreateCommentRequest model)
    {
        var comment = _context.Comment.Find(id);
        _mapper.Map(model, comment);
        _context.Comment.Update(comment);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var comment = _context.Comment.Find(id);
        _context.Comment.Remove(comment);
        _context.SaveChanges();

        _context.CommentMedia.RemoveRange(_context.CommentMedia.Where(m => m.CommentId == id).ToList());
        _context.SaveChanges();
    }

    #endregion Methods
}
