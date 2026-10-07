using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=movies.db"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();

    if (!db.Movies.Any())
    {
        var actor1 = new Actor
        {
            Name = "Christian Bale",
            Bio = "Known for his dramatic physical transformations."
        };

        var actor2 = new Actor
        {
            Name = "Heath Ledger",
            Bio = "Legendary for his role as the Joker."
        };

        var actor3 = new Actor
        {
            Name = "Leonardo DiCaprio",
            Bio = "Academy Award winner known for dramatic films."
        };

        var movie1 = new Movie
        {
            Title = "The Dark Knight",
            ReleaseYear = 2008,
            Actors = new List<Actor>
            {
                actor1,
                actor2
            }
        };

        var movie2 = new Movie
        {
            Title = "Inception",
            ReleaseYear = 2010,
            Actors = new List<Actor>
            {
                actor1,
                actor3
            }
        };

        db.Movies.AddRange(movie1, movie2);
        db.SaveChanges();
    }
}

app.UseCors("AllowReact");

app.UseAuthorization();

app.MapControllers();

app.Run();
