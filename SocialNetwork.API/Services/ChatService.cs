using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Chat;
using SocialNetwork.API.Entities.Notification;
using SocialNetwork.API.Helpers;
using SocialNetwork.API.Hubs;
using SocialNetwork.API.Models.Chat;

namespace SocialNetwork.API.Services;

public interface IChatService
{
    void CreateChat(Guid userId);

    void AddChatMember(Guid chatId, Guid userId);

    Guid GetOneToOneChatId(Guid fromId, Guid toId);

    void Send(CreateMessageRequest model);

    IEnumerable<Message> GetChatHistory(Guid chatId);

    void DeleteMessage(Guid id);
    void DeleteChat(Guid id);

    IEnumerable<Message> GetChatList(Guid userId);
    //TODO: Add methods
}

public class ChatService : IChatService
{
    #region Properties
    private DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub, IChatHub> _chatHub;
    private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;
    #endregion Properties

    #region Constructor
    public ChatService(
        DataContext context,
        IMapper mapper,
        IHubContext<ChatHub, IChatHub> chatHub,
        IHubContext<NotificationHub, INotificationHub> notificationHub)
    {
        _context = context;
        _mapper = mapper;
        _chatHub = chatHub;
        _notificationHub = notificationHub;
    }
    #endregion Constructor

    #region Methods
    public void CreateChat(Guid userId)
    {
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            Name = "",
            Timestamp = DateTime.Now
        };
        var chatCreator = new ChatMember
        {
            Id = Guid.NewGuid(),
            ChatId = chat.Id,
            UserId = userId,
            Role = 0,
            Timestamp = DateTime.Now
        };
        _context.Chat.Add(chat);
        _context.ChatMember.Add(chatCreator);
        _context.SaveChanges();
    }

    public void AddChatMember(Guid chatId, Guid userId)
    {
        var chatMember = new ChatMember
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            UserId = userId,
            Role = 1,
            Timestamp = DateTime.Now
        };
        _context.ChatMember.Add(chatMember);
        _context.SaveChanges();
    }

    public Guid GetOneToOneChatId(Guid fromId, Guid toId)
    {
        var chat = _context.OneToOneChat
            .Where(c => c.User1Id == fromId)
            .FirstOrDefault(c => c.User2Id == toId);

        if (chat == null)
        {
            chat = _context.OneToOneChat
                .Where(c => c.User1Id == toId)
                .FirstOrDefault(c => c.User2Id == fromId);

            if (chat == null)
            {
                chat = new OneToOneChat
                {
                    Id = Guid.NewGuid(),
                    Name = "",
                    User1Id = fromId,
                    User2Id = toId,
                    Timestamp = DateTime.Now
                };
                _context.OneToOneChat.Add(chat);
                _context.SaveChanges();

                return chat.Id;
            }
            else
            {
                return chat.Id;
            }
        }
        else
        {
            return chat.Id;
        }
    }

    public void Send(CreateMessageRequest model)
    {
        var message = _mapper.Map<Message>(model);
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

        var newMsgNotification = new Notification
        {
            FromId = message.UserId,
            ToId = message.ChatId,
            Verb = $"{_context.UserProfile.First(up => up.UserId == message.UserId).Name} has sent a message",
            EntityId = message.Id,
            Read = false,
            Timestamp = DateTime.Now
        };
        //_context.Notification.Add(likeNotification);

        _chatHub.Clients.Group(message.ChatId.ToString()).Send(message);
        _notificationHub.Clients.Group(message.ChatId.ToString()).Notify(newMsgNotification);

        _context.SaveChanges();
    }

    public IEnumerable<Message> GetChatHistory(Guid chatId)
    {
        var groupChatHistory = _context.Message
            .Where(m => m.ChatId == chatId)
            .ToList();
        return groupChatHistory;
    }

    public void DeleteMessage(Guid id)
    {
        var message = _context.Message.Find(id);
        _context.Message.Remove(message);

        _context.MessageMedia.RemoveRange(_context.MessageMedia.Where(m => m.MessageId == id).ToList());

        _chatHub.Clients.Group(message.ChatId.ToString()).Delete(message.Id);
        _context.SaveChanges();
    }
    
    public void DeleteChat(Guid id)
    {
        var chat = _context.Chat.Find(id);
        _context.Chat.Remove(chat);

        _context.SaveChanges();
    }

    public IEnumerable<Message> GetChatList(Guid userId)
    {
        var myOneToOneChatIds = _context.OneToOneChat
            .Where(c => (c.User1Id == userId) || (c.User2Id == userId))
            .Select(c => c.Id).ToList();
        var myGroupChatIds = _context.ChatMember
            .Where(cm => cm.UserId == userId)
            .Select(cm => cm.ChatId).ToList();
        var myChatIds = myOneToOneChatIds.Concat(myGroupChatIds).Take(10);

        var msgList = new List<Message>();
        foreach (var myChatId in myChatIds)
        {
            var latestMsg = _context.Message
                .Where(m => m.ChatId == myChatId)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefault();
            if (latestMsg != null)
            {
                msgList.Add(latestMsg);
            }
        }
        return msgList;
    }
    #endregion Methods
}
