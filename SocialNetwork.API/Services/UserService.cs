using AutoMapper;
using SocialNetwork.API.Authorization;
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
        return getUser(id);
    }

    public void Register(RegisterRequest model)
    {
        // validate
        if (_context.User.Any(x => x.Email == model.Email))
            throw new AppException("Email '" + model.Email + "' is already taken");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // save user
        _context.User.Add(user);
        _context.SaveChanges();
    }

    public void UpdateCredentials(Guid id, UpdateCredentialsRequest model)
    {
        var user = getUser(id);

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
        var user = getUser(id);
        _context.User.Remove(user);
        _context.SaveChanges();
    }
    #endregion Methods

    #region Helper methods

    private User getUser(Guid id)
    {
        var user = _context.User.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
    #endregion Helper methods
}
