var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔥 CORS forçado (Railway-friendly)
app.Use(async (context, next) =>
{
    context.Response.Headers["Access-Control-Allow-Origin"] = "https://fazenda-amori.netlify.app";
    context.Response.Headers["Access-Control-Allow-Headers"] = "*";
    context.Response.Headers["Access-Control-Allow-Methods"] = "GET,POST,PUT,DELETE,OPTIONS";

    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 204;
        return;
    }

    await next();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
