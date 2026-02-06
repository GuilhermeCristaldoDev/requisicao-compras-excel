var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI do seu serviço
builder.Services.AddScoped<ExcelGenerator.Api.Services.ExcelGeneratorService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors("frontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Servir front (wwwroot)
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// Health check simples
app.MapGet("/", () => "API Fazenda Amori rodando.");

app.Run();
