using Microsoft.AspNetCore.Diagnostics;
using ServiceExeption.Exceptions;


public static class WebPipeline
{
    public static WebApplication ConfigurePipeline(this WebApplication app, bool migrate = true)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseCustomSwagger();
        }
        app.UseGlobalErrorHandling();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.MapReverseProxy();
        app.Run();
        return app;
    }

    private static WebApplication UseGlobalErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler("/error");
        app.Map("/error", (HttpContext httpContext) =>
        {

            Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;


            return exception switch
            {
                ServiceException serviceException => Results.Problem(
                    statusCode: serviceException.StatusCode,
                    detail: serviceException.ErrorMessage),
                _ => Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: exception.Message)
            };

        });

        return app;
    }

    private static WebApplication UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/auth/v1/swagger.json", "Auth Service API");
            c.SwaggerEndpoint("/posts/v1/swagger.json", "Post Service API");
            c.SwaggerEndpoint("/profiles/v1/swagger.json", "Profile Service API");
            c.SwaggerEndpoint("/comments/v1/swagger.json", "Comment Service API");
            c.SwaggerEndpoint("/likes/v1/swagger.json", "Like Service API");
            c.RoutePrefix = string.Empty; // Чтобы Swagger UI был по корню (http://localhost:port/)
        });

        return app;
    }
}
