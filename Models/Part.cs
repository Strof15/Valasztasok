using System.ComponentModel.DataAnnotations;

namespace Valasztasok.Models
{
    public class Part
    {
        [Key]
        public string RovidNev { get; set; }
        public string TeljesNev { get; set; }
        public ICollection<Jelolt> Jeloltek { get; set; }
    }
}
