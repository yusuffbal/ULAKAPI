using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ULAKAPI.Hubs;

namespace ULAKAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly IMessageService _messageService;

        public MessageController(IHubContext<ChatHub> chatHubContext, IMessageService messageService)
        {
            _chatHubContext = chatHubContext;
            _messageService = messageService;
        }

        [HttpPost("send-message-to-user")]
        public async Task<IActionResult> SendMessageToUser([FromBody] SendMessageDto request)
        {
            var message = new Message
            {
                SenderId = request.SenderId,
                RecipientId = request.RecipientId,
                Content = request.Content,
            };


            await _chatHubContext.Clients.User(request.RecipientId.ToString()).SendAsync("ReceiveMessage", message);

            return Ok(new { Message = "Message sent successfully" });
        }

        [HttpPost("send-message-to-group")]
        public async Task<IActionResult> SendMessageToGroup([FromBody] SendMessageToGroupDto request)
        {
            var message = new Message
            {
                SenderId = request.SenderId,
                GroupId = request.GroupId,
                Content = request.Content
            };

            _messageService.SendMessage(message);

            await _chatHubContext.Clients.Group(request.GroupId.ToString()).SendAsync("ReceiveMessage", message);

            return Ok(new { Message = "Message sent to group successfully" });
        }


        [HttpGet("get-messages-by-user/{userId}")]
        public async Task<IActionResult> GetMessagesByUserIdAsync(int userId)
        {
            var messages = await _messageService.GetMessagesByUserIdAsync(userId);

            if (messages == null || messages.Count == 0)
            {
                return NotFound(new { Message = "No messages found for this user" });
            }

            return Ok(messages);
        }

        [HttpGet("get-messages-by-group/{groupId}")]
        public async Task<IActionResult> GetMessagesByGroupIdAsync(int groupId)
        {
            var messages = await _messageService.GetMessagesByGroupIdAsync(groupId);

            if (messages == null || messages.Count == 0)
            {
                return NotFound(new { Message = "No messages found for this group" });
            }

            return Ok(messages);
        }

        [HttpGet("GetChatList/{currentUserId}")]
        public async Task<IActionResult> GetChatList(int currentUserId)
        {
            var chatList = await _messageService.ListChatsAsync(currentUserId);

            if (chatList == null || chatList.Count == 0)
            {
                return NotFound(new { Message = "No messages found for this group" });
            }

            return Ok(chatList);
        }

        [HttpGet("chat/{chatId}/user/{userId}")]
        public async Task<ActionResult<List<Message>>> GetMessagesByChatAndUserId(int chatId, int userId)
        {
            try
            {
                // Get messages by chatId and userId
                var messages = await _messageService.GetMessagesByChatAndUserIdAsync(chatId, userId);

                if (messages == null || messages.Count == 0)
                {
                    return NotFound("No messages found.");
                }

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
