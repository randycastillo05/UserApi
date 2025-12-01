using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 2. Agregar controladores
builder.Services.AddControllers();

// 3. Configurar Swagger para documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();