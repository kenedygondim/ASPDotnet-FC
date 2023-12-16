using ASPDotnetFC;
using ASPDotnetFC.Interface;
using ASPDotnetFC.Repository;
using ASPDotnetFC.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddDbContext<FootballContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen();

//injeção de dependências
builder.Services.AddTransient<Seed>();

//serviços do automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//permite ciclos
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<ICompetitionRepository, CompetitionRepository>();
builder.Services.AddScoped<IClubCompetitionsRepository, ClubCompetitionsRepository>();
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();

//injeta as depêndencias antes do programa iniciar
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
