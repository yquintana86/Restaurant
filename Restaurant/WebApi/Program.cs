using Application;
using Application.Abstractions.Data;
using Infrastructure;
using Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;
using Presentation;
using Serilog;
using WebApi.Infrastructure;
using Presentation.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddControllers();// This is how you can put the Controllers in other Assembly but this one
                                  // .AddApplicationPart(typeof(Presentation.RestaurantDependencyInjection).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inject Services by layers
builder.Services.AddApplication()
                        .AddInfrastructure()
                            .AddPresentation();



builder.Services.AddDbContext<IEFCoreDbContext, EFCoreDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDatabase"));
});

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    redisOptions.Configuration = builder.Configuration.GetConnectionString("RedisCache");
});

builder.Services.AddExceptionHandler<GlobalExceptionHandlingMiddleWare>();
builder.Services.AddProblemDetails();

//Configure Serilog 
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Add Serilog to the PipeLine
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseExceptionHandler();

//EndPoints Without Carter because of conflict with Microsoft.EntityFrameworkCore.Design 8.x -> Microsoft.CodeAnalysis.Common
app.AddShiftEndPoints();
app.AddWaitersEndPoints();
app.AddRoomEndPoints();

app.Run();

