using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Entities.Post;
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
    IEnumerable<Like> GetAllLikesByCommentId(Guid id);

    /// <summary>
    /// Get all shares for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List all shares for this comment</returns>
    IEnumerable<Share> GetAllSharesByCommentId(Guid id);

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
    private readonly IHubContext<CommentHub, ICommentHub> _hubContext;
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
        IHubContext<CommentHub, ICommentHub> hubContext)
    {
        _context = context;
        _mapper = mapper;
        _hubContext = hubContext;
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

    public async void Create(CreateCommentRequest model)
    {
        var comment = _mapper.Map<Comment>(model);
        comment.Id = Guid.NewGuid();
        _context.Comment.Add(comment);
        _context.SaveChanges();

        foreach (var mediaPath in model.MediaPaths)
        {
            var commentMedia = new Media
            {
                Id = Guid.NewGuid(),
                ParentId = comment.Id,
                Path = mediaPath
            };
            _context.Media.Add(commentMedia);
            _context.SaveChanges();
        }

        await _hubContext.Clients.Group(model.PostId.ToString()).AddComment(model);
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

        _context.Media.RemoveRange(_context.Media.Where(m => m.ParentId == id).ToList());
        _context.SaveChanges();
    }

    #endregion Methods
}
