using System.Text;
using System.Text.Json;
using api.DTO;
using api.Models;
using Microsoft.Extensions.Options;

namespace api.Services
{
    public interface IChatBotService
    {
        Task<ChatBotResponse> AskChatBot(ChatBotRequest chatBotRequest);
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

        public async Task<ChatBotResponse> AskChatBot(ChatBotRequest chatBotRequest)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // This ensures the first letter is lowercase
            };

            String jsonPostBodyRequest = JsonSerializer.Serialize(chatBotRequest, options);
            StringContent content = new StringContent(jsonPostBodyRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_chatBotUrl + "/chatbot", content);
            String responseContent = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                responseContent = await response.Content.ReadAsStringAsync();
                // Console.WriteLine("Raw response: " + responseContent);  // Log the raw response content
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {response.StatusCode} - {errorContent}");
            }

            try
            {
                // Deserialize the response content with added options
                options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    AllowTrailingCommas = true,  // Allow trailing commas
                    ReadCommentHandling = JsonCommentHandling.Skip
                };

                // Deserialize JSON content
                ChatBotResponse? botResponse = JsonSerializer.Deserialize<ChatBotResponse>(responseContent, options);

                // Check if deserialization was successful
                if (botResponse == null)
                {
                    throw new Exception("Failed to deserialize the response.");
                }

                String[] responseArray = botResponse.Response.Split("\n\n").Skip(1).ToArray();
                botResponse.Response = String.Join("\n\n", responseArray);

                // Console.WriteLine($"Deserialized response: {botResponse.Response}");
                return botResponse;
            }
            catch (JsonException)
            {
                // Console.WriteLine($"Deserialization failed: {ex.Message}");
                // Console.WriteLine($"Raw response content: {responseContent}");
                throw;
            }
        }
    }
}