using RackDAT_API.Contracts;
using RackDAT_API.Models;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
        builder.Configuration["SupabaseUrl"],
        builder.Configuration["SupabaseKey"],
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/carreras", async (
    CreateCarrera request,
    Supabase.Client client) =>
{
    var carrera = new Carrera
    {
        carrera = request.carrera
    };

    var response = await client.From<Carrera>().Insert(carrera);

    var newCarrera = response.Models.First();

    return Results.Ok("Se ha agregado correctamente la carrera: " + carrera.carrera + " con ID: " + carrera.ID);

});

app.UseHttpsRedirection();

app.Run();
