using WarehouseProject.Models.DTOs;

namespace WarehouseProject.Services {
    public interface IGeminiChatService {
        Task<string> GenerateTextAsync(string prompt);
        Task<ChatResponse> StartConversationAsync(string initialPrompt);
        Task<ChatResponse> ContinueConversationAsync(string conversationId, string userMessage);
    }
}
