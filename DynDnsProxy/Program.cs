using DynDnsProxy;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddHttpLogging(options => options.LoggingFields =
        HttpLoggingFields.RequestProperties
        | HttpLoggingFields.RequestQuery
        | HttpLoggingFields.ResponseBody);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddOptions<DynDnsConfiguration>()
    .Bind(builder.Configuration.GetSection("DynDns"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<DynDnsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpLogging();
app.UseHttpsRedirection();

app.MapGet("/dyndns/update",
    ([FromServices] DynDnsService dynDnsService, string ip4, string ip6, string? ip6LanPrefix, string domain)
        => dynDnsService.Update(ip4, ip6, ip6LanPrefix, domain));

app.Run();
