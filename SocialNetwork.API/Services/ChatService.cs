﻿using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Chat;
using SocialNetwork.API.Entities.Notification;
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

    //IEnumerable<Message> GetChatList(Guid userId);
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
    public void Send(CreateMessageRequest model)
    {
        var message = _mapper.Map<Message>(model);
        message.ParentId = Guid.NewGuid();
        message.Timestamp = DateTime.Now;
        //_context.Message.Add(message);

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
            FromId = message.FromId,
            ToId = message.ToId,
            Verb = $"{_context.UserProfile.First(up => up.UserId == message.FromId).Name} has sent a message",
            EntityId = message.Id,
            Read = false,
            Timestamp = DateTime.Now
        };
        //_context.Notification.Add(likeNotification);

        _chatHub.Clients.Group(message.ToId.ToString()).Send(message);
        _notificationHub.Clients.Group(message.ToId.ToString()).Notify(newMsgNotification);

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

    //public IEnumerable<Message> GetChatList(Guid userId)
    //{
    //    var chats = _context.Message
    //                .Where(m => m.FromId == userId)
    //                .Where(m => m.ToId == userId);
    //    var chats2 = from c in chats
    //                 group c by c.ToId
    //                 into c1
    //                 select new { Id = c1.Key, Latest = c1.Max(m => m.Timestamp) };
    //    var res = from c1 in chats
    //              from c2 in chats2.Select(c => c.Id == c1.Id)
    //              on c1.Id equals c2.Id && c1.Timestamp equals c2.Latest
    //              select c1;
    //}
    #endregion Methods
}
