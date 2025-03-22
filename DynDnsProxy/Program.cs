using DynDnsProxy;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddHttpLogging(options => options.LoggingFields =
        HttpLoggingFields.RequestQuery
        | HttpLoggingFields.ResponseBody
        | HttpLoggingFields.ResponseStatusCode);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DynDnsConfiguration>(builder.Configuration.GetSection("DynDns"));
builder.Services.AddSingleton<DynDnsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/dyndns/update",
    (string ip4, string ip6, string? ip6LanPrefix, string domain) =>
    {
        using var scope = app.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<DynDnsService>()
            .Update(ip4, ip6, ip6LanPrefix, domain);
    });

app.Run();
