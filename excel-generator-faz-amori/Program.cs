var builder = WebApplication.CreateBuilder(args);

// --- CORREÇÃO DO CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://fazenda-amori.netlify.app") // URL do seu front SEM a barra no final
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
// ------------------------

builder.Services.AddControllers();
// ... resto dos services ...

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// --- ATIVAR O CORS (TEM QUE SER ANTES DE TUDO) ---
app.UseCors("AllowAll");
// -------------------------------------------------

app.UseAuthorization();
app.MapControllers();

app.Run();