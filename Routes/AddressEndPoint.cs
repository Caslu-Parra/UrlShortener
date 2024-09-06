using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Models;

namespace UrlShortener.Routes
{
    public static class AddressEndPoint
    {
        private static string ShortenUrl()
        {
            string shortned = string.Empty;

            var letters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            var numers = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            var random = new Random();

            for (int i = 0; i < 2; i++)
            {
                string letter = letters[random.Next(letters.Count)];
                string number = numers[random.Next(numers.Count)];
                shortned += letter + number;
                letters.Remove(letter);
                numers.Remove(number);
            }
            return shortned;
        }
        public static void MapAddresesRoutes(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{key}", async (string key, DbConnection db) =>
            {
                Address? address = await db.Addresses.FindAsync(key);
                return address is null ? Results.NotFound("Endereço não encontrado") : Results.Redirect(address.Url, true);
            })
            .Produces<string>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status301MovedPermanently);

            app.MapGet("/", async (DbConnection db) => await db.Addresses.ToListAsync())
            .Produces<List<Address>>(StatusCodes.Status200OK);

            app.MapDelete("/delete/{key}", async (string key, DbConnection db) =>
            {
                Address? address = await db.Addresses.FindAsync(key);

                if (address is null) return Results.NotFound("Endereço não encontrado.");

                db.Addresses.Remove(address);
                await db.SaveChangesAsync();

                return Results.Accepted();
            })
            .Produces(StatusCodes.Status202Accepted)
            .Produces<string>(StatusCodes.Status404NotFound);

            app.MapPost("/create", async (string url, DbConnection db) =>
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) return Results.BadRequest("URL inválida.");

                string urlShortened = string.Empty;

                do urlShortened = ShortenUrl();
                while (await db.Addresses.AnyAsync(e => e.Shortned.Equals(urlShortened, StringComparison.InvariantCulture)));

                var address = new Address
                {
                    Url = url,
                    Shortned = urlShortened,
                    Expiration = DateTime.Now.AddDays(3)
                };
                await db.Addresses.AddAsync(address);
                await db.SaveChangesAsync();

                return Results.Created($"/addresses/{address.Shortned}", address);

            })
            .Produces<Address>(StatusCodes.Status201Created)
            .Produces<string>(StatusCodes.Status400BadRequest);
        }
    }
}