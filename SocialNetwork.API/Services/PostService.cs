using AutoMapper;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Entities.Comment;
using SocialNetwork.API.Entities.Post;
using SocialNetwork.API.Helpers;
using SocialNetwork.API.Models.Post;

namespace SocialNetwork.API.Services;

/// <summary>
/// Interface provides methods to PostService class
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Get a post by its id
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>A post with provided id</returns>
    Post GetById(Guid id);

    /// <summary>
    /// Get all comments of this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List of comments of this post</returns>
    IEnumerable<Comment> GetAllCommentsByPostId(Guid id);

    /// <summary>
    /// Get all likes for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List of likes for this post</returns>
    IEnumerable<PostLike> GetAllLikesByPostId(Guid id);

    /// <summary>
    /// Get all shares for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List all shares for this post</returns>
    IEnumerable<PostShare> GetAllSharesByPostId(Guid id);

    /// <summary>
    /// Create new post
    /// </summary>
    /// <param name="model">Required fields to create new post form</param>
    void Create(CreatePostRequest model);

    /// <summary>
    /// Edit a post
    /// </summary>
    /// <param name="model">Required fields to edit a post form</param>
    void Edit(Guid id, CreatePostRequest model);

    /// <summary>
    /// Delete a post by its id
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    void Delete(Guid id);
}

/// <summary>
/// Provides queries and helpers of Post table to DB
/// </summary>
public class PostService : IPostService
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
    public PostService(
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
    public Post GetById(Guid id)
    {
        var post = _context.Post.Find(id);
        return post;
    }

    public IEnumerable<Comment> GetAllCommentsByPostId(Guid id)
    {
        var comments = _context.Comment
            .Where(c => c.PostId == id).ToList();
        return comments;
    }

    public IEnumerable<PostLike> GetAllLikesByPostId(Guid id)
    {
        var likes = _context.PostLike
            .Where(l => l.PostId == id).ToList();
        return likes;
    }

    public IEnumerable<PostShare> GetAllSharesByPostId(Guid id)
    {
        var shares = _context.PostShare
            .Where(s => s.PostId == id).ToList();
        return shares;
    }

    public void Create(CreatePostRequest model)
    {
        var post = _mapper.Map<Post>(model);
        post.Id = Guid.NewGuid();
        post.Timestamp = DateTime.Now;
        _context.Post.Add(post);
        _context.SaveChanges();

        if (model.MediaPaths.Any())
        {
            foreach (var mediaPath in model.MediaPaths)
            {
                var postMedia = new PostMedia
                {
                    Id = Guid.NewGuid(),
                    PostId = post.Id,
                    Path = mediaPath
                };
                _context.PostMedia.Add(postMedia);
                _context.SaveChanges();
            }
        }
    }

    public void Edit(Guid id, CreatePostRequest model)
    {
        var post = _context.Post.Find(id);
        _mapper.Map(model, post);
        _context.Post.Update(post);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var post = _context.Post.Find(id);
        _context.Post.Remove(post);
        _context.SaveChanges();

        _context.PostMedia.RemoveRange(_context.PostMedia.Where(m => m.PostId == id).ToList());
        _context.SaveChanges();
    }

    #endregion Methods
}
