using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MySqlX.XDevAPI.Common;
using Ubiety.Dns.Core;
using UrlShortener.Database;
using UrlShortener.Models;

namespace UrlShortener.Routes
{
    public static class AddressEndPoint
    {
        private static async Task<string> ShortenUrlAsync(DbConnection db)
        {

            string shortned = string.Empty;
            do
            {
                var letters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
        "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
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
            } while (await db.Addresses.FindAsync(shortned) is not null);
            return shortned;
        }
        public static void MapRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/{key}", async (string key, DbConnection db) =>
                {
                    Address? address = await db.Addresses.FindAsync(key);
                    return address is null ? Results.NotFound() : Results.Redirect(address.Url);
                });

            app.MapGet("/addresses", async (DbConnection db) => await db.Addresses.ToListAsync());

            app.MapGet("/addresses/{key}", async (string key, DbConnection db) => await db.Addresses.FindAsync(key));

            app.MapPost("/create", async (string url, DbConnection db) =>
            {

                if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? result))
                    return Results.BadRequest($"URL inválida.");

                Address? address = await db.Addresses.FirstOrDefaultAsync(x => x.Url == url);
                if (address is not null) return Results.Conflict(address);

                address = new Address
                {
                    Url = url,
                    Shortned = await ShortenUrlAsync(db),
                    Expiration = DateTime.Now.AddDays(3)
                };
                await db.Addresses.AddAsync(address);
                await db.SaveChangesAsync();

                return Results.Created($"/addresses/{address.Shortned}", address);
            });
        }
    }
}