using Microsoft.EntityFrameworkCore;
using templateMariaDb.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MariaDbConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("PermitirFrontend");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapGet("/", () => "Pedro Luiz - 2311550737");

#region ESTADOS
// CRUD de ESTADOS
app.MapGet("/estados", async (AppDbContext db) =>
    await db.Estados.ToListAsync());

app.MapPost("/estados", async (Estado estado, AppDbContext db) =>
{
    db.Estados.Add(estado);
    await db.SaveChangesAsync();
    return Results.Created($"/estados/{estado.Id}", estado);
});

app.MapPut("/estados/{id}", async (int id, Estado estadoAtualizado, AppDbContext db) =>
{
    var estado = await db.Estados.FindAsync(id);
    if (estado is null) return Results.NotFound();

    estado.Nome = estadoAtualizado.Nome;
    estado.Sigla = estadoAtualizado.Sigla;

    await db.SaveChangesAsync();
    return Results.Ok("Estado atualizado.");
});

app.MapDelete("/estados/{id}", async (int id, AppDbContext db) =>
{
    var estado = await db.Estados
        .Include(e => e.Cidades)
        .FirstOrDefaultAsync(e => e.Id == id);
        
    if (estado is null) return Results.NotFound("Estado não encontrado.");

    if (estado.Cidades.Count > 0)
    {
        return Results.Problem(
            detail: $"Não é possível excluir o estado '{estado.Nome}' porque existem {estado.Cidades.Count} cidades associadas a ele.",
            statusCode: 400,
            title: "Exclusão não permitida"
        );
    }

    db.Estados.Remove(estado);
    await db.SaveChangesAsync();
    return Results.Ok("Estado deletado.");
});
#endregion

#region CIDADES
// CRUD de CIDADES
app.MapGet("/cidades", async (AppDbContext db) =>
    await db.Cidades.Include(c => c.Estado)
                    .Select(c => new 
                    { 
                        c.Id, 
                        c.Nome, 
                        EstadoId = c.Estado.Id, 
                        EstadoSigla = c.Estado.Sigla 
                    })
                    .OrderBy(c => c.Id)
                    .ToListAsync());

app.MapPost("/cidades", async (Cidade cidade, AppDbContext db) =>
{
    db.Cidades.Add(cidade);
    await db.SaveChangesAsync();
    return Results.Created($"/cidades/{cidade.Id}", cidade);
});

app.MapPut("/cidades/{id}", async (int id, Cidade cidadeAtualizada, AppDbContext db) =>
{
    var cidade = await db.Cidades.FindAsync(id);
    if (cidade is null) return Results.NotFound();

    cidade.Nome = cidadeAtualizada.Nome;
    cidade.EstadoId = cidadeAtualizada.EstadoId;

    await db.SaveChangesAsync();
    return Results.Ok("Cidade atualizada.");
});

app.MapDelete("/cidades/{id}", async (int id, AppDbContext db) =>
{
    var cidade = await db.Cidades
        .Include(c => c.LinhasOnibus)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (cidade is null) return Results.NotFound("Cidade não encontrada.");

    if (cidade.LinhasOnibus.Count > 0)
    {
        return Results.Problem(
            detail: $"Não é possível excluir a cidade '{cidade.Nome}' porque existem {cidade.LinhasOnibus.Count} linhas de ônibus associadas a ela.",
            statusCode: 400,
            title: "Exclusão não permitida"
        );
    }

    db.Cidades.Remove(cidade);
    await db.SaveChangesAsync();
    return Results.Ok("Cidade deletada.");
});
#endregion

#region LINHAS DE ÔNIBUS
// CRUD de LINHAS DE ÔNIBUS
app.MapGet("/linhasonibus", async (AppDbContext db) =>
    await db.LinhasOnibus.Include(l => l.Cidade)
                         .ThenInclude(c => c.Estado)
                         .Select(l => new
                         { 
                             l.Id, 
                             l.Nome, 
                             CidadeId = l.Cidade.Id, 
                             CidadeNome = l.Cidade.Nome,
                             EstadoSigla = l.Cidade.Estado.Sigla
                         })
                         .OrderBy(l => l.Id)
                         .ToListAsync());

app.MapPost("/linhasonibus", async (LinhaOnibus linhaOnibus, AppDbContext db) =>    
{
    db.LinhasOnibus.Add(linhaOnibus);
    await db.SaveChangesAsync();
    return Results.Created($"/linhasonibus/{linhaOnibus.Id}", linhaOnibus);
});

app.MapPut("/linhasonibus/{id}", async (int id, LinhaOnibus linhaOnibusAtualizada, AppDbContext db) =>
{
    var linhaOnibus = await db.LinhasOnibus.FindAsync(id);
    if (linhaOnibus is null) return Results.NotFound();

    linhaOnibus.Nome = linhaOnibusAtualizada.Nome;
    linhaOnibus.CidadeId = linhaOnibusAtualizada.CidadeId;

    await db.SaveChangesAsync();
    return Results.Ok("Linha de ônibus atualizada.");
});

app.MapDelete("/linhasonibus/{id}", async (int id, AppDbContext db) =>
{
    var linhaOnibus = await db.LinhasOnibus
        .Include(l => l.Onibus)
        .FirstOrDefaultAsync(l => l.Id == id);

    if (linhaOnibus is null) return Results.NotFound("Linha de ônibus não encontrada.");

    if (linhaOnibus.Onibus.Count > 0)
    {
        return Results.Problem(
            detail: $"Não é possível excluir a linha de ônibus '{linhaOnibus.Nome}' porque existem {linhaOnibus.Onibus.Count} ônibus associados a ela.",
            statusCode: 400,
            title: "Exclusão não permitida"
        );
    }

    db.LinhasOnibus.Remove(linhaOnibus);
    await db.SaveChangesAsync();
    return Results.Ok("Linha de ônibus deletada.");
});
#endregion

#region ÔNIBUS
// CRUD de ÔNIBUS
app.MapGet("/onibus", async (AppDbContext db) =>
    await db.Onibus.Include(o => o.LinhaOnibus)
                    .ThenInclude(l => l.Cidade)
                   .Select(o => new 
                   { 
                       o.Id, 
                       o.Placa, 
                       LinhaOnibusId = o.LinhaOnibus.Id, 
                       LinhaOnibusNome = o.LinhaOnibus.Nome,
                       LinhaOnibusCidadeNome = o.LinhaOnibus.Cidade.Nome,
                   })
                   .OrderBy(o => o.Id)
                   .ToListAsync());

app.MapPost("/onibus", async (Onibus onibus, AppDbContext db) =>
{
    db.Onibus.Add(onibus);
    await db.SaveChangesAsync();
    return Results.Created($"/onibus/{onibus.Id}", onibus);
});

app.MapPut("/onibus/{id}", async (int id, Onibus onibusAtualizado, AppDbContext db) =>
{
    var onibus = await db.Onibus.FindAsync(id);
    if (onibus is null) return Results.NotFound();

    onibus.Placa = onibusAtualizado.Placa;
    onibus.LinhaOnibusId = onibusAtualizado.LinhaOnibusId;

    await db.SaveChangesAsync();
    return Results.Ok("Onibus atualizado.");
});

app.MapDelete("/onibus/{id}", async (int id, AppDbContext db) =>
{
    var onibus = await db.Onibus.FindAsync(id);
    if (onibus is null) return Results.NotFound("Ônibus não encontrado.");

    db.Onibus.Remove(onibus);
    await db.SaveChangesAsync();
    return Results.Ok("Onibus deletado.");
});
#endregion
app.Run();