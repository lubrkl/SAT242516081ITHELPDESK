using System.ComponentModel.DataAnnotations;

namespace SAT242516081ITHELPDESK.Models
{
    public class Durum
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ad { get; set; }

        public virtual ICollection<Talep> Talepler { get; set; } = new List<Talep>();
    }
}