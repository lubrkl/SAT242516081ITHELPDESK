using Microsoft.AspNetCore.Identity;
using SAT242516081ITHELPDESK.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SAT242516081ITHELPDESK.Models;


namespace SAT242516081ITHELPDESK.Models
{
    public class Talep
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Baslik { get; set; }

        [Required]
        public string Aciklama { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        // --- Foreign Keys (İlişkisel Anahtarlar) ---
        public int KategoriId { get; set; }
        public int DurumId { get; set; }
        public string OlusturanKullaniciId { get; set; }
        public string? AtananTeknisyenId { get; set; }

        // --- Navigation Properties (Hatanın Sebebi Olan Eksik Kısım) ---
        // Bu özellikler, EF Core'un ilişkileri anlaması için GEREKLİDİR.

        [ForeignKey("KategoriId")]
        public virtual Kategori Kategori { get; set; }

        [ForeignKey("DurumId")]
        public virtual Durum Durum { get; set; }

        // HATA BUNUN EKSİKLİĞİNDEN KAYNAKLANIYOR
        [ForeignKey("OlusturanKullaniciId")]
        public virtual ApplicationUser OlusturanKullanici { get; set; }

        // BU DA EKSİKSE BİR SONRAKİ HATAYI VERECEKTİ
        [ForeignKey("AtananTeknisyenId")]
        public virtual ApplicationUser? AtananTeknisyen { get; set; }

        public virtual ICollection<TalepGuncelleme> Guncellemeler { get; set; } = new List<TalepGuncelleme>();




    }

}
