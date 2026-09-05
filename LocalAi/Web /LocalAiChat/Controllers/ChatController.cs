using Microsoft.AspNetCore.Mvc;
using LocalAiChat.Services;
using LocalAiChat.Models;

namespace LocalAiChat.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly OllamaChatService _chatService;

        public ChatController(OllamaChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { error = "message is required" });

            var response = await _chatService.AskAsync(request.Message);
            return Ok(new { response });
        }

        [HttpPost("stream")]
        public async Task Stream([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                Response.StatusCode = 400;
                return;
            }

            Response.StatusCode = 200;
            Response.ContentType = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";

            await foreach (var chunk in _chatService.StreamAsync(request.Message))
            {
                // send as SSE 'data' field (JSON encoded)
                var data = $"data: {System.Text.Json.JsonSerializer.Serialize(chunk)}\n\n";
                var bytes = System.Text.Encoding.UTF8.GetBytes(data);
                await Response.Body.WriteAsync(bytes, 0, bytes.Length);
                await Response.Body.FlushAsync();
            }

            // signal done
            var done = "event: done\ndata: [DONE]\n\n";
            var doneBytes = System.Text.Encoding.UTF8.GetBytes(done);
            await Response.Body.WriteAsync(doneBytes, 0, doneBytes.Length);
            await Response.Body.FlushAsync();
        }
    }
}
