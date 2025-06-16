using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ServiceExeption.Exceptions;
using Persistance;

public static class WebPipeline
{
    public static WebApplication ConfigurePipeline(this WebApplication app, bool migrate = true)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseCustomSwagger();
        }
        if (migrate) app.ApplyMigration();
        app.UseGlobalErrorHandling();
        app.UseHttpsRedirection();
        app.MapControllers();
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
                    detail: exception?.Message)
            };

        });

        return app;
    }

    private static void ApplyMigration(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using PostDbContext dbContext = scope.ServiceProvider.GetRequiredService<PostDbContext>();
        // dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    private static WebApplication UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
            c.RoutePrefix = string.Empty; // Чтобы Swagger UI был по корню (http://localhost:port/)
        });

        return app;
    }
}
