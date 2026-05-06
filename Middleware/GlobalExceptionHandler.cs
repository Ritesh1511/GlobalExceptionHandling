namespace GlobalExceptionHandling.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
                await _next(context);
                _logger.LogInformation("Response Status Code: {StatusCode}",context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    Message = "Something went wrong",
                    Error = ex.Message
                };

                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
