using ChatApp.Data;
using ChatApp.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// CORS for Angular dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")   // allow Angular dev server
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// SQL Server
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Order matters
app.UseRouting();
app.UseCors("AngularClient");   // must be before MapControllers/MapHub
app.UseAuthentication();
app.UseAuthentication();
app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();
