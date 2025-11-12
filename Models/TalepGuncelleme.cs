using Microsoft.AspNetCore.Identity;
using SAT242516081ITHELPDESK.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SAT242516081ITHELPDESK.Models
{
    public class TalepGuncelleme
    {
        public int Id { get; set; }
        [Required]
        public string Aciklama { get; set; }
        public DateTime GuncellemeTarihi { get; set; } = DateTime.Now;

        public int TalepId { get; set; }
        public string GuncelleyenKullaniciId { get; set; }
        [ForeignKey("TalepId")]
        public virtual Talep Talep { get; set; }

        [ForeignKey("GuncelleyenKullaniciId")]
        public virtual ApplicationUser GuncelleyenKullanici { get; set; }
    }
}
