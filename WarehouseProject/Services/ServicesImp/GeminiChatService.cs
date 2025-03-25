using Microsoft.CodeAnalysis;
using WarehouseProject.Models.DTOs;
using Google.Cloud.AIPlatform.V1;

namespace WarehouseProject.Services.ServicesImp {
    public class GeminiChatService : IGeminiChatService {

        private readonly PredictionServiceClient _client;
        private readonly string _projectId;
        private readonly string _location;

        public GeminiChatService(string projectId, string location) {
            _projectId = projectId;
            _location = location;
            _client = PredictionServiceClient.Create();
        }
        public async Task<string> GenerateTextAsync(string prompt) {
            try {
                var request = new GenerateContentRequest {
                    Model = $"projects/{_projectId}/locations/{_location}/models/gemini-pro",
                    Contents = { new Content
                {
                    Role = "user",
                    Parts = { new Part { Text = prompt } }
                }}
                };

                var response = await _client.GenerateContentAsync(request);
                return response.Candidates[0].Content.Parts[0].Text;
            } catch (Exception ex) {
                return $"Lỗi: {ex.Message}";
            }
        }
        public async Task<ChatResponse> StartConversationAsync(string initialPrompt) {
            var conversationId = Guid.NewGuid().ToString();
            var aiResponse = await GenerateTextAsync(initialPrompt);

            return new ChatResponse {
                ConversationId = conversationId,
                Response = aiResponse
            };
        }

        public async Task<ChatResponse> ContinueConversationAsync(string conversationId, string userMessage) {
            var aiResponse = await GenerateTextAsync(userMessage);

            return new ChatResponse {
                ConversationId = conversationId,
                Response = aiResponse
            };
        }

    }
}
