using System.ComponentModel.DataAnnotations;

namespace SAT242516081ITHELPDESK.Models
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ad { get; set; } // <-- HATA 4'ÜN ARADIĞI ÖZELLİK

        public virtual ICollection<Talep> Talepler { get; set; } = new List<Talep>();
    }
}