using Microsoft.EntityFrameworkCore;
using SAT242516081ITHELPDESK.Models;
using SAT242516081ITHELPDESK.Models.DbContexts;
using System.Security.Claims;

namespace SAT242516081ITHELPDESK.Models.Providers
{
    public class MyDbModel_Provider
    {
        private readonly MyDbModel_DbContext _context;

        public MyDbModel_Provider(MyDbModel_DbContext context)
        {
            _context = context;
        }

        // --- Veri Çekme İşlemleri ---

        // Yeni talep oluşturma sayfasında kullanılacak Kategorileri getir
        public async Task<List<Kategori>> GetKategorilerAsync()
        {
            return await _context.Kategoriler.ToListAsync();
        }
        public async Task<Talep> GetTalepByIdAsync(int talepId)
        {
            // Talebi, ilişkili Kategori ve Durum bilgileriyle birlikte bul
            return await _context.Talepler
                .Include(t => t.Kategori)
                .Include(t => t.Durum)
                .FirstOrDefaultAsync(t => t.Id == talepId);

        }

        // Yeni talep oluşturma sayfasında kullanılacak Durumları getir
        // (örn: "Açık" durumunu otomatk seçmek için)
        public async Task<List<Durum>> GetDurumlarAsync()
        {
            return await _context.Durumlar.ToListAsync();
        }
        public async Task<List<Talep>> GetTaleplerimAsync(ClaimsPrincipal user)
        {
            // Kullanıcının ID'sini al
            var kullaniciId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(kullaniciId))
            {
                return new List<Talep>(); // Kullanıcı giriş yapmamışsa boş liste döndür
            }

            // Veritabanından, bu kullanıcının oluşturduğu talepleri bul.
            // Taleplerin Kategori ve Durum bilgilerini de "Include" ile yüklüyoruz.
            return await _context.Talepler
                .Include(t => t.Kategori) // Kategori adını göstermek için
                .Include(t => t.Durum)     // Durum adını göstermek için
                .Where(t => t.OlusturanKullaniciId == kullaniciId)
                .OrderByDescending(t => t.OlusturmaTarihi) // En yeniden eskiye sırala
                .ToListAsync();
        }

        // --- Veri Kaydetme İşlemleri ---

        // Yeni talep oluştur
        public async Task AddTalepAsync(Talep yeniTalep)
        {
            _context.Talepler.Add(yeniTalep);
            await _context.SaveChangesAsync();
        }

        // TODO: Diğer işlemler (TaleplerimiListele, TalepGuncelle vb.)
        // Şimdilik "basic" seviyede bu kadarı yeterli.
    }
}
