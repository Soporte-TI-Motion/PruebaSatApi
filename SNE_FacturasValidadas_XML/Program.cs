using Microsoft.EntityFrameworkCore;
using SNE_FacturasValidadas_XML.Data;
using SNE_FacturasValidadas_XML.Repositories;
using SNE_FacturasValidadas_XML.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VuePolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Registrar HttpClient + SatService
builder.Services.AddHttpClient<ISatService, SatService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<IXMLService, XMLService>();

var app = builder.Build();

app.UseCors("VuePolicy");

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();