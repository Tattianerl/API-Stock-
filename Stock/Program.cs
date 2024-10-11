using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Stock.Data;
using Stock.Models;

var builder = WebApplication.CreateBuilder(args);

// String de conexão com o banco de dados SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=products.db";

// Configurando o contexto do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Adicionar serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock", Version = "v1" });
});

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
