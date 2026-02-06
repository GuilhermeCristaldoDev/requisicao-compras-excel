var builder = WebApplication.CreateBuilder(args);

// --- CORREÇÃO DO CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Aceita qualquer origem (Netlify, Localhost, etc)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Permite cookies/auth se precisar
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