using DynDnsProxy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var dynDnsConfiguration = app.Configuration.GetSection("DynDns").Get<DynDnsConfiguration>();
if (dynDnsConfiguration == null)
    throw new ApplicationException("Config is missing.");

app.MapGet("/dyndns/update",
    (string ip4, string ip6, string ip6LanPrefix, string domain) => dynDnsConfiguration.UpdateUrl);

app.Run();
