using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace LocalAiChat.Services
{
    public class OllamaChatService
    {
        private readonly IChatClient _chatClient;
        private readonly List<ChatMessage> _history = new();

        public OllamaChatService()
        {
            _chatClient = new OllamaApiClient(
                new Uri("http://localhost:11434"),
                "phi3:latest"
            );
        }

        public async Task<string> AskAsync(string prompt)
        {
            _history.Add(new ChatMessage(ChatRole.User, prompt));

            string response = string.Empty;

            await foreach (var item in _chatClient.GetStreamingResponseAsync(_history))
            {
                response += item.Text;
            }

            _history.Add(new ChatMessage(ChatRole.Assistant, response));
            return response;
        }

        public async IAsyncEnumerable<string> StreamAsync(string prompt)
        {
            _history.Add(new ChatMessage(ChatRole.User, prompt));

            await foreach (var item in _chatClient.GetStreamingResponseAsync(_history))
            {
                yield return item.Text;
            }

            // Optionally, accumulate final response into history
            // (consumer can assemble the pieces). For now we add a placeholder.
            // Note: reconstructing full response is left to caller if needed.
        }
    }
}