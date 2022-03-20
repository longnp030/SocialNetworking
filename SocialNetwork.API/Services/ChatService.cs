using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Chat;
using SocialNetwork.API.Helpers;
using SocialNetwork.API.Hubs;
using SocialNetwork.API.Models.Chat;

namespace SocialNetwork.API.Services;

public interface IChatService
{
    void Send(CreateMessageRequest model);

    IEnumerable<Message> GetChatHistory(Guid fromId, Guid toId);

    IEnumerable<Message> GetGroupChatHistory(Guid chatId);

    void DeleteMessage(Guid id);
    void DeleteGroupChat(Guid id);
    //TODO: Add methods
}

public class ChatService : IChatService
{
    #region Properties
    private DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub, IChatHub> _chatHub;
    #endregion Properties

    #region Constructor
    public ChatService(
        DataContext context,
        IMapper mapper,
        IHubContext<ChatHub, IChatHub> chatHub)
    {
        _context = context;
        _mapper = mapper;
        _chatHub = chatHub;
    }
    #endregion Constructor

    #region Methods
    public void Send(CreateMessageRequest model)
    {
        var message = _mapper.Map<Message>(model);
        message.ParentId = Guid.NewGuid();
        message.Timestamp = DateTime.Now;
        _context.Message.Add(message);

        if (model.MediaPaths.Any())
        {
            foreach (var mediaPath in model.MediaPaths)
            {
                var messageMedia = new MessageMedia
                {
                    Id = Guid.NewGuid(),
                    MessageId = message.Id,
                    Path = mediaPath
                };
                _context.MessageMedia.Add(messageMedia);

            }
        }

        _chatHub.Clients.Group(message.ToId.ToString()).Send(message);

        _context.SaveChanges();
    }

    public IEnumerable<Message> GetChatHistory(Guid fromId, Guid toId)
    {
        var aToB = _context.Message
            .Where(m => m.FromId == fromId)
            .Where(m => m.ToId == toId);
        var bToA = _context.Message
            .Where(m => m.FromId == toId)
            .Where(m => m.ToId == fromId);
        return aToB.Concat(bToA).OrderBy(m => m.Timestamp).Reverse().ToList();
    }

    public IEnumerable<Message> GetGroupChatHistory(Guid chatId)
    {
        var groupChatHistory = _context.Message
            .Where(m => m.ToId == chatId)
            .ToList();
        return groupChatHistory;
    }

    public void DeleteMessage(Guid id)
    {
        var message = _context.Message.Find(id);
        _context.Message.Remove(message);

        _context.MessageMedia.RemoveRange(_context.MessageMedia.Where(m => m.MessageId == id).ToList());

        _chatHub.Clients.Group(message.ToId.ToString()).Delete(message.Id);
        _context.SaveChanges();
    }
    
    public void DeleteGroupChat(Guid id)
    {
        var chat = _context.GroupChat.Find(id);
        _context.GroupChat.Remove(chat);

        _context.SaveChanges();
    }
    #endregion Methods
}
