namespace PV521_BooksShop.Middlewares
{
    public class StopRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<StopRequestMiddleware> _logger;

        public StopRequestMiddleware(RequestDelegate next, ILogger<StopRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Request

            _logger.LogInformation("Request stoped");
            return;
            await _next(context);

            // Response
        }
    }
}
