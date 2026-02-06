var builder = WebApplication.CreateBuilder(args);

// 1. ADICIONAR O SERVIÇO DE CORS
// Defina uma política com um nome específico (ex: "AllowSpecificOrigin")
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://fazenda-amori.netlify.app") // SEM a barra no final!
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- REMOVA AQUELE SEU BLOCO 'app.Use(...)' INTEIRO DAQUI ---

app.UseSwagger();
app.UseSwaggerUI();

// 2. ATIVAR O MIDDLEWARE DE CORS
// A ORDEM É CRUCIAL: Tem que ser ANTES de Authorization e MapControllers
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.MapControllers();

app.Run();