using Microsoft.EntityFrameworkCore;
using WebPDV.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AplicacaoDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Adiciona serviços essenciais ao container
builder.Services.AddControllers(); // Necessário para suportar controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS automaticamente

app.UseAuthorization(); // Habilita autorização se necessário

app.MapControllers(); // Mapeia os controllers automaticamente

app.Run();
