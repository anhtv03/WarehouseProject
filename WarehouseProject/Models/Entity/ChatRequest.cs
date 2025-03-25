namespace WarehouseProject.Models.Entity {
    public class ChatRequest {
        public string Prompt { get; set; }
    }

    public class ContinueChatRequest {
        public string ConversationId { get; set; }
        public string UserMessage { get; set; }
    }
}
