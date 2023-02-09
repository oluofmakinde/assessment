using Assessment.Caching;
using Assessment.Context;
using Assessment.Interfaces;
using Assessment.Middleware;
using Assessment.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContactsContext>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();


builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
