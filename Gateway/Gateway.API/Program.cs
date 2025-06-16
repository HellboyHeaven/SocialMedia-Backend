using API;
var builder = WebApplication.CreateBuilder(args);
builder.Services.InjectAPI(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // URL вашего фронта
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();
app.UseCors("AllowFrontend");
app.ConfigurePipeline(false);
