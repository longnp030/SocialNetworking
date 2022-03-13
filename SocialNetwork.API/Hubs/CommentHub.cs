using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Models.Post;

namespace SocialNetwork.API.Hubs;


public interface ICommentHub
{
    Task AddComment(CreateCommentRequest model);
}


public class CommentHub : Hub<ICommentHub>
{
    private readonly ILogger _logger;

    public CommentHub(ILogger<CommentHub> logger)
    {
        _logger = logger;
    }

    public async Task JoinPostGroup(Guid userId, Guid postId)
    {
        _logger.LogInformation($"Client {userId} is viewing {postId}");
        await Groups.AddToGroupAsync(userId.ToString(), postId.ToString());
    }

    public async Task LeavePostGroup(Guid userId, Guid postId)
    {
        _logger.LogInformation($"Client {userId} has left {postId}");
        await Groups.RemoveFromGroupAsync(userId.ToString(), postId.ToString());
    }


    public async Task AddComment(CreateCommentRequest model)
    {
        await JoinPostGroup(model.AuthorId, model.PostId);
        await Clients.Group(model.PostId.ToString()).AddComment(model);
    }
}
