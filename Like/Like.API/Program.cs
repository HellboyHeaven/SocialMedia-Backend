using API;
using Persistance;
using Infrastructure;
using Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.InjectAPI(builder.Configuration);


builder.Services.InjectPersistence(builder.Configuration);
builder.Services.InjectInfrastrcutrue(builder.Configuration);
builder.Services.InjectApplication(builder.Configuration);

var app = builder.Build();
app.ConfigurePipeline(true);
