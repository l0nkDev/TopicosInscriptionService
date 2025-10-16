using Microsoft.EntityFrameworkCore;
using TopicosP1Backend.Scripts;
using TopicosP1Backend.Cache;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(opt =>
    opt.UseNpgsql("Username=postgres; Password=\\^MGbat%=5deeuN; Host=34.55.58.2; Database=topicos;"));
//    opt.UseNpgsql("Username=postgres; Password=postgres; Host=localhost; Database=topicos;"));
//    opt.UseInMemoryDatabase("main"));
//    opt.UseSqlite("Data Source=main.db"));
builder.Services.AddDbContext<CacheContext>(opt =>
//    opt.UseSqlite("Data Source=cache.db"));
    opt.UseInMemoryDatabase("cache"));
builder.Services.AddSingleton<APIQueue>();
builder.Services.AddSingleton<WorkerManager>();
builder.Services.AddHostedService(_ => _.GetRequiredService<WorkerManager>());
var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.CreateDbIfNotExists();
app.CreateCacheIfNotExists();


app.Run();
