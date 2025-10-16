using Microsoft.AspNetCore.Mvc;

namespace TopicosP1Backend.Scripts
{
    public class TextActionResult : IActionResult
    {
        private readonly string _content;
        private readonly string _contentType;

        public TextActionResult(string content, string contentType = "text/plain")
        {
            _content = content;
            _contentType = contentType;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = _contentType;
            await response.WriteAsync(_content);
        }
    }
}
