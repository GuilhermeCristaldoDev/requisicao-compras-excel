using ExcelGenerator.Api.Services; // Ajuste para o seu namespace real

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAÇÃO DO CORS (O Coração da Solução)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://fazenda-amori.netlify.app")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. INJEÇÃO DOS SEUS SERVIÇOS (Essencial para o Controller não dar erro)
builder.Services.AddScoped<ExcelGeneratorService>();
builder.Services.AddScoped<PdfGeneratorService>();

var app = builder.Build();

// 3. PIPELINE DE EXECUÇÃO (A ordem aqui é sagrada!)
app.UseSwagger();
app.UseSwaggerUI();

// O CORS tem que vir antes da Autorização e dos Controllers
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();