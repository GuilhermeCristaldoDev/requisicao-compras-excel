using ExcelGenerator.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 CORS: libera SOMENTE o Netlify
builder.Services.AddCors(options =>
{
    options.AddPolicy("NetlifyFront", policy =>
    {
        policy
            .WithOrigins("https://fazenda-amori.netlify.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 🔧 DI
builder.Services.AddScoped<ExcelGeneratorService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("NetlifyFront");

app.UseStaticFiles();
app.MapControllers();

app.Run();
