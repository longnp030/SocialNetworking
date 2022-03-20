using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Models.Chat;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    #region Properties
    private readonly IChatService _chatService;
    #endregion Properties

    #region Constructor
    public ChatController(
        IChatService chatService
        )
    {
        _chatService = chatService;
    }
    #endregion Constructor

    #region Methods
    [HttpPost("messages")]
    public IActionResult Send([FromBody] CreateMessageRequest model)
    {
        _chatService.Send(model);
        return Ok(new { Message = "Message sent." });
    }

    [HttpGet("{fromId}/and/{toId}")]
    public IActionResult GetChatHistory(Guid fromId, Guid toId)
    {
        var chatHistory = _chatService.GetChatHistory(fromId, toId);
        return Ok(chatHistory);
    }

    [HttpGet("{chatId}")]
    public IActionResult GetGroupChatHistory(Guid chatId)
    {
        var groupChatHistory = _chatService.GetGroupChatHistory(chatId);
        return Ok(groupChatHistory);
    }

    [HttpDelete("messages/{id}")]
    public IActionResult DeleteMessage(Guid id)
    {
        _chatService.DeleteMessage(id);
        return Ok(new { Message = "Deleted" });
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteGroupChat(Guid id)
    {
        _chatService.DeleteGroupChat(id);
        return Ok(new { Message = "Deleted" });
    }
    #endregion Methods
}
