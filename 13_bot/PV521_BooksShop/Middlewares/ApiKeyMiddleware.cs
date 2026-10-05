using PV521_BooksShop.BLL.Dtos;

namespace PV521_BooksShop.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var apiKey = context.Request.Headers["ApiKey"].FirstOrDefault();

            if(apiKey == null || apiKey != "1234567890")
            {
                context.Response.StatusCode = 401;
                string message = apiKey == null ? "Header 'ApiKey' is required" : "ApiKey invalid";
                var response = ServiceResponseDto.Error(message);

                await context.Response.WriteAsJsonAsync(response);

                return;
            }

            await _next(context);
        }
    }
}
