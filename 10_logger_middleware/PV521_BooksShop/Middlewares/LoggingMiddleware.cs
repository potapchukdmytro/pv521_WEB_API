namespace PV521_BooksShop.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Request

            var request = context.Request;
            string message = $"IsHttps: {request.IsHttps}\n" +
                $"Method: {request.Method}\n" +
                $"Path: {request.Path}\n" +
                $"Protocol: {request.Protocol}\n" +
                $"Host: {request.Host}\n" +
                $"Scheme: {request.Scheme}\n" +
                $"Query: {request.QueryString}\n";
            _logger.LogInformation(message);

            string url = $"{request.Scheme}://{request.Host}{request.Path}";
            _logger.LogInformation(url);

            await _next(context);

            // Response

            var response = context.Response;
            message = $"StatusCode: {response.StatusCode}\n";
            _logger.LogInformation(message);

            var headers = response.Headers.ContentType;
            _logger.LogInformation(headers);
        }
    }
}
