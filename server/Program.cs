using Npgsql;

NpgsqlDataSource db = NpgsqlDataSource.Create("Host=localhost;Database=dhsolutionsdb");
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/api/kontaktlista", GetKontakter);
app.MapPost("/api/nyKontakt", PostKontakt);
app.Run();

static async Task<List<Kontakt>> GetKontakter(NpgsqlDataSource db)
{
    List<Kontakt> kontakter = new List<Kontakt>();

    try
    {
        await using var cmd = db.CreateCommand(("SELECT * FROM kontakter"));
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            kontakter.Add(new Kontakt(
                    reader.GetInt32(2),
                    reader.GetString(0),
                    reader.GetString(1)
                )
            );
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("errror hämtning av kontakter:  [e.Message}");
        throw;
    }
    return kontakter;
}

static async Task<IResult> PostKontakt(NpgsqlDataSource db, KontaktDTO nyKontakt)
{
    using var cmd = db.CreateCommand("INSERT INTO telefonlista(namn, nummewr) VALUES ($1, $2)");
    cmd.Parameters.AddWithValue(nyKontakt.Namn);
    cmd.Parameters.AddWithValue(nyKontakt.Nummer);
    try
    {
        cmd.ExecuteNonQuery();
        return TypedResults.Created();
    }
    catch (Exception e)
    {
        Console.WriteLine($"error att läga till kontakt: {e.Message}");
        return TypedResults.BadRequest();
        throw;
    }
}

public record KontaktDTO(string Namn, string Nummer);


public record Kontakt(int Id, string Namn, string Nummer);
