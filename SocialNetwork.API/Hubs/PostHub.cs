using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Comment;

namespace SocialNetwork.API.Hubs;

public interface IPostHub
{
    Task Reaction(Guid postId, Guid userId, bool like);

    Task Comment(Comment comment);

    Task ViewingPost(Guid postId);

    Task LeavingPost(Guid postId);
}

public class PostHub : Hub<IPostHub>
{
    private readonly ILogger _logger;

    public PostHub(ILogger<PostHub> logger)
    {
        _logger = logger;
    }

    public async Task Reaction(Guid postId, Guid userId, bool like)
    {
        await Clients.Group(postId.ToString()).Reaction(postId, userId, like);
    }

    public async Task Comment(Comment comment)
    {
        await Clients.Group(comment.PostId.ToString()).Comment(comment);
    }

    public async Task ViewingPost(Guid postId)
    {
        _logger.LogInformation($"Client {Context.ConnectionId} is viewing {postId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, postId.ToString());
    }

    public async Task LeavingPost(Guid postId)
    {
        _logger.LogInformation($"Client {Context.ConnectionId} has left {postId}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, postId.ToString());
    }
}
