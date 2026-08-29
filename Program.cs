using Microsoft.Extensions.AI;
using OllamaSharp;

// Chat client
IChatClient chatClient =
    new OllamaApiClient(new Uri("http://localhost:11434"), "phi3:latest");

// Start the conversation with context for the AI model
List<ChatMessage> chatHistory = new();

while (true)
{
    //Main 
    Console.WriteLine();
    Console.WriteLine("\n==== Local-AI ===");
    Console.WriteLine("Local Support Agent");
    Console.WriteLine();
    Console.Write("Your Prompt: ");
    var userPrompt = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userPrompt))
    {
        break;
    }

    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    Console.Write("AI Response: ");
    var response = "";

    await foreach (ChatResponseUpdate item in chatClient.GetStreamingResponseAsync(chatHistory))
    {
        Console.Write(item.Text);
        response += item.Text;
    }

    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
    Console.WriteLine();
}