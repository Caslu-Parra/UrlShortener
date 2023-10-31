using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Models
{
    [Table("addresses")]
    // [PrimaryKey(nameof(Url), nameof(Shortned))]
    [Index(nameof(Url), IsUnique = true)]
    public class Address
    {
        [Required(ErrorMessage = "Url address is required")]
        public string Url { get; set; }
        [Key]
        [MaxLength(5)]
        public string Shortned { get; set; }
        [Required]
        public DateTime Expiration { get; set; }
    }
}