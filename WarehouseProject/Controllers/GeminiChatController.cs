using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Services;

namespace WarehouseProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiChatController : ControllerBase {
        private readonly IGeminiChatService _geminiChatService;

        public GeminiChatController(IGeminiChatService geminiChatService) {
            _geminiChatService = geminiChatService;
        }

        [HttpPost("start")]
        public async Task<ActionResult<ChatResponse>> StartConversation([FromBody] string initialPrompt) {
            if (string.IsNullOrEmpty(initialPrompt)) {
                return BadRequest("Prompt ban đầu không được để trống.");
            }

            try {
                var response = await _geminiChatService.StartConversationAsync(initialPrompt);
                return Ok(response);
            } catch (Exception ex) {
                return StatusCode(500, $"Lỗi khi bắt đầu cuộc trò chuyện: {ex.Message}");
            }
        }

        [HttpPost("continue")]
        public async Task<ActionResult<ChatResponse>> ContinueConversation([FromQuery] string conversationId, [FromBody] string userMessage) {
            if (string.IsNullOrEmpty(conversationId)) {
                return BadRequest("ConversationId không được để trống.");
            }

            if (string.IsNullOrEmpty(userMessage)) {
                return BadRequest("Tin nhắn của người dùng không được để trống.");
            }

            try {
                var response = await _geminiChatService.ContinueConversationAsync(conversationId, userMessage);
                return Ok(response);
            } catch (Exception ex) {
                return StatusCode(500, $"Lỗi khi tiếp tục cuộc trò chuyện: {ex.Message}");
            }
        }

    }
}