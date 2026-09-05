using LocalAiChat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LocalAiChat.Models;

namespace LocalAiChat.Pages.Api;
public class ChatModel : PageModel
{
    private readonly OllamaChatService _chatService;

    public ChatModel(OllamaChatService chatService)
    {
        _chatService = chatService;
    }

    public void OnGet()
    {
        
    }
    public async Task<IActionResult> OnPostAsync(
        [FromBody] ChatRequest request)
    {
        var response = 
            await _chatService.AskAsync(request.Message);

            return new JsonResult(
                new
                {
                    response
                });
            
    }
    
}