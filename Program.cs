var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços essenciais ao container
builder.Services.AddControllers(); // Necessário para suportar controllers

var app = builder.Build();

app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS automaticamente
app.UseAuthorization(); // Habilita autorização se necessário
app.MapControllers(); // Mapeia os controllers automaticamente

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
