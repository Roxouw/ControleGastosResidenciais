using FluentValidation;
using FluentValidation.AspNetCore;
using GastosResidenciais.API.Middleware;
using GastosResidenciais.API.Validators;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Application.Services;
using GastosResidenciais.Infrastructure.Data;
using GastosResidenciais.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── 1. Banco de Dados — SQLite com EF Core ────────────────────────────────────
// A connection string aponta para um arquivo .db local.
// O arquivo é criado automaticamente se não existir, garantindo persistência
// mesmo após reiniciar a aplicação.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=gastos_residenciais.db"
    ));

// ── 2. Repositórios — Infrastructure ─────────────────────────────────────────
// Registrados como Scoped: uma instância por requisição HTTP,
// compartilhada entre o repositório e outros serviços da mesma requisição.
builder.Services.AddScoped<IPessoaRepository,    PessoaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

// ── 3. Serviços — Application (lógica de negócio) ─────────────────────────────
builder.Services.AddScoped<IPessoaService,    PessoaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();

// ── 4. Validação — FluentValidation ──────────────────────────────────────────
// Registra todos os validators definidos no assembly da API.
// A validação ocorre automaticamente antes de entrar no action do controller.
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePessoaValidator>();

// ── 5. Controllers + configurações de serialização JSON ──────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serializa enums como strings no JSON (ex.: "Despesa" em vez de 0)
        // Facilita a leitura e depuração no front-end
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// ── 6. Swagger / OpenAPI ──────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title   = "Gastos Residenciais API",
        Version = "v1",
        Description = "API para controle de gastos residenciais. " +
                      "Gerencie pessoas, categorias e transações financeiras."
    });

    // Inclui os comentários XML do código na documentação do Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// ── 7. CORS — permite requisições do front-end React ─────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEnd", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",  // Vite dev server (padrão)
                "http://localhost:3000"   // CRA / outros bundlers
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ─────────────────────────────────────────────────────────────────────────────

var app = builder.Build();

// ── Aplicar Migrations automaticamente ao iniciar ────────────────────────────
// Em produção seria mais seguro rodar migrations via CLI ou pipeline de deploy.
// Para este projeto, aplicar automaticamente simplifica a execução local.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ── Pipeline de middlewares ───────────────────────────────────────────────────

// Tratamento global de exceções — deve ser o PRIMEIRO middleware
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gastos Residenciais API v1");
        c.RoutePrefix = "swagger"; // Disponível em /swagger
    });
}

app.UseHttpsRedirection();
app.UseCors("FrontEnd");
app.UseAuthorization();
app.MapControllers();

app.Run();
