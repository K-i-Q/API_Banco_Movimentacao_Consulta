using FluentAssertions.Common;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Questao5.Application.MappingProfiles;
using Questao5.Application.Services;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;
using Questao5.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicione os servi�os ao cont�iner.
builder.Services.AddControllers();

builder.Services.AddSingleton<IDbConnection>(provider =>
{
    string connectionString = builder.Configuration.GetConnectionString("NomeDaConexao");
    return new SqliteConnection(connectionString);
});

builder.Services.AddSingleton<DatabaseConfig>(provider =>
{
    // L�gica para obter as configura��es do banco de dados
    // Pode ser necess�rio ajustar o c�digo de acordo com as suas necessidades
    string databaseName = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite");
    return new DatabaseConfig { Name = databaseName };
});

builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<IContaCorrenteService, ContaCorrenteService>();

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("NomeDaConexao"));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Outros registros de servi�o...

var app = builder.Build();

// Configurar o pipeline de solicita��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
});

// Outras configura��es da aplica��o...

#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetRequiredService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();
