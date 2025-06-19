namespace DeallsTechnicalTest.Middleware
{
    public class RequestIdMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = context.Request.Headers["X-Request-ID"].FirstOrDefault()
                            ?? Guid.NewGuid().ToString();

            context.Items["RequestId"] = requestId;

            context.Response.Headers["X-Request-ID"] = requestId;

            await _next(context);
        }
    }
}
