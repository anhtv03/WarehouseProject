using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models.Entity;
using WarehouseProject.Services;

namespace WarehouseProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiChatController : ControllerBase {
        private readonly IGeminiChatService _geminiService;

        public GeminiChatController(IGeminiChatService geminiService) {
            _geminiService = geminiService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request) {
            var response = await _geminiService.StartConversationAsync(request.Prompt);
            return Ok(response);
        }

        [HttpPost("continue")]
        public async Task<IActionResult> ContinueChat([FromBody] ContinueChatRequest request) {
            var response = await _geminiService.ContinueConversationAsync(
                request.ConversationId,
                request.UserMessage
            );
            return Ok(response);
        }

    }
}
