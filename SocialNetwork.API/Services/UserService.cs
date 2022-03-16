﻿using AutoMapper;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Entities.Post;
using SocialNetwork.API.Entities.User;
using SocialNetwork.API.Helpers;
using SocialNetwork.API.Models.User;

namespace SocialNetwork.API.Services;

/// <summary>
/// Interface provides methods to UserService class
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Log user in
    /// </summary>
    /// <param name="model">Auth model</param>
    /// <returns>User's credentials and jwt token</returns>
    AuthenticateResponse Authenticate(AuthenticateRequest model);

    /// <summary>
    /// Get all users in DB
    /// </summary>
    /// <returns>All users in DB</returns>
    IEnumerable<User> GetAll();

    /// <summary>
    /// Get a user by his/her id
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>One user</returns>
    User GetById(Guid id);

    /// <summary>
    /// Register a user into system
    /// </summary>
    /// <param name="model">Register model</param>
    void Register(RegisterRequest model);

    /// <summary>
    /// Update a user's credentials information
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <param name="model">Update credentials model</param>
    void UpdateCredentials(Guid id, UpdateCredentialsRequest model);

    /// <summary>
    /// Delete a user from the system
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    void Delete(Guid id);

    /// <summary>
    /// Get user profile
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>User profile</returns>
    UserProfile GetProfile(Guid id);

    /// <summary>
    /// Update user profile
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <param name="model">Update Profile model</param>
    void UpdateProfile(Guid id, UpdateProfileRequest model);

    /// <summary>
    /// Update user setting
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <param name="model">Update Setting model</param>
    void UpdateSetting(Guid id, UpdateSettingRequest model);

    /// <summary>
    /// Get all posts by user whose unique identifier equals to id
    /// </summary>
    /// <param id="">User's unique identifier</param>
    /// <returns>All posts by this user</returns>
    IEnumerable<Post> GetAllPostsByUserId(Guid id);

    /// <summary>
    /// Get all posts saved by this user
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of posts saved by this user</returns>
    IEnumerable<Post> GetAllSavedPostsByUserId(Guid id);

    /// <summary>
    /// Populate user's newsfeed
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of posts to display in user's newsfeed</returns>
    IEnumerable<Guid> GetFeed(Guid id);

    /// <summary>
    /// fromId follows toId
    /// </summary>
    /// <param name="fromId">Id of who follows</param>
    /// <param name="toId">Id of who being followed</param>
    void Follow(Guid fromId, Guid toId);

    /// <summary>
    /// fromId unfollows toId
    /// </summary>
    /// <param name="fromId">Id of who unfollows</param>
    /// <param name="toId">Id of who being unfollowed</param>
    void Unfollow(Guid fromId, Guid toId);
}

/// <summary>
/// Provides queries and helpers of User table to DB
/// </summary>
public class UserService : IUserService
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
    public UserService(
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
    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.User.SingleOrDefault(x => x.Email == model.Email);

        // validate
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Email or password is incorrect");

        // authentication successful
        var response = _mapper.Map<AuthenticateResponse>(user);
        response.JwtToken = _jwtUtils.GenerateToken(user);
        return response;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.User;
    }

    public User GetById(Guid id)
    {
        var user = _context.User.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    public void Register(RegisterRequest model)
    {
        // validate
        if (_context.User.Any(x => x.Email == model.Email))
            throw new AppException("Email '" + model.Email + "' is already taken");

        // map model to new user object
        var user = _mapper.Map<User>(model);
        user.Id = Guid.NewGuid();
        user.Timestamp = DateTime.Now;

        // hash password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // save user
        _context.User.Add(user);
        _context.SaveChanges();

        var userProfile = new UserProfile
        {
            UserId = user.Id
        };
        _context.UserProfile.Add(userProfile);
        _context.SaveChanges();
    }

    public void UpdateCredentials(Guid id, UpdateCredentialsRequest model)
    {
        var user = GetById(id);

        // validate
        if (model.Email != user.Email && _context.User.Any(x => x.Email == model.Email))
            throw new AppException("Email '" + model.Email + "' is already taken");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // copy model to user and save
        _mapper.Map(model, user);
        _context.User.Update(user);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var user = GetById(id);
        _context.User.Remove(user);
        _context.SaveChanges();
    }

    public UserProfile GetProfile(Guid id)
    {
        return _context.UserProfile.SingleOrDefault(x => x.UserId == id);
    }
    
    public void UpdateProfile(Guid id, UpdateProfileRequest model)
    {
        var userProfile = _context.UserProfile.SingleOrDefault(up => up.UserId == id);
        _mapper.Map(model, userProfile);
        _context.Update(userProfile);
        _context.SaveChanges();
    }

    public void UpdateSetting(Guid id, UpdateSettingRequest model)
    {
        var userSetting = _context.UserSetting.SingleOrDefault(up => up.UserId == id);
        _mapper.Map(model, userSetting);
        _context.Update(userSetting);
        _context.SaveChanges();
    }

    public IEnumerable<Post> GetAllPostsByUserId(Guid id)
    {
        var ownPosts = _context.Post                  // get all posts
            .Where(p => p.AuthorId == id)             // by this user
            .Where(p => p.GroupId == Guid.Empty)      // but not in any group
            .ToList();   

        var sharedPostsIds = _context.PostShare    // get all shared posts
            .Where(s => s.FromId == id)          // shared by this user
            .Where(s => s.ToId == id)            // to his/her own (not shared to others)
            .Select(s => s.PostId);              // get col PostId

        var sharedPosts = _context.Post                     // get all posts
            //.Where(p => p.UserId != id)                     // whose author is not this user
            //.Where(p => p.GroupId == Guid.Empty)            // not in any group
            //.Where(p => p.Privacy == 0)                     // public
            .Where(p => sharedPostsIds.Contains(p.Id))      // which ids are in the id list "sharedPostsIds" we got above
            .ToList();

        return ownPosts.Concat(sharedPosts);
    }

    public IEnumerable<Post> GetAllSavedPostsByUserId(Guid id)
    {
        var savedPostsIds = _context.PostSave    // Get all saved posts
            .Where(s => s.UserId == id)         // saved by this user
            .Select(s => s.PostId);           // get col PostId

        var savedPosts = _context.Post                      // Get all posts
            .Where(p => savedPostsIds.Contains(p.Id))    // appear in saved list
            .ToList();

        return savedPosts;
    }

    public IEnumerable<Guid> GetFeed(Guid id)
    {
        return _context.Post                          // get all posts
            .Where(p => p.AuthorId == id)             // by this user
            .Select(p => p.Id)
            .ToList();
    }

    public void Follow(Guid fromId, Guid toId)
    {
        var follow = new Follow
        {
            FromId = fromId,
            ToId = toId,
            Timestamp = DateTime.Now
        };
        _context.Follow.Add(follow);
        _context.SaveChanges(); 
    }

    public void Unfollow(Guid fromId, Guid toId)
    {
        var follow = _context.Follow
            .Where(f => f.FromId == fromId)
            .SingleOrDefault(f => f.ToId == toId);
        _context.Follow.Remove(follow);
        _context.SaveChanges();
    }

    #endregion Methods
}
