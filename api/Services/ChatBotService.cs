using System.Text;
using System.Text.Json;
using api.DTO;
using api.Models;
using Microsoft.Extensions.Options;

namespace api.Services
{
    public interface IChatBotService
    {
        Task<ChatBotResponse> AskChatBot(ChatBotRequest chatBotRequest, String jwtToken, String userId);
    }

    public class ChatBotService : IChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly ChatBotConfig _chatBotConfig;
        private readonly String _chatBotUrl;

        public ChatBotService(HttpClient httpClient, IOptions<ChatBotConfig> chatBotConfig)
        {
            _httpClient = httpClient;
            _chatBotConfig = chatBotConfig.Value;
            _chatBotUrl = $"http://{_chatBotConfig.Url}:{_chatBotConfig.Port}";
        }

        public async Task<ChatBotResponse> AskChatBot(ChatBotRequest chatBotRequest, String jwtToken, String userId)
        {
            var requestPayload = new
            {
                prompt = chatBotRequest.Prompt,
                token = jwtToken,
                userId = userId
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(requestPayload, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_chatBotUrl}/chatbot", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode} - {responseContent}");
            }

            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            var botResponse = JsonSerializer.Deserialize<ChatBotResponse>(responseContent, deserializationOptions)
                            ?? throw new Exception("Failed to deserialize the response.");

            return botResponse;
        }
    }
}