using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SAT242516081ITHELPDESK.Models; // Modellerimizi kullanacağız
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SAT242516081ITHELPDESK.Models.UnitOfWorks;

namespace SAT242516081ITHELPDESK.Components.Pages
{
    // "partial class" olması, .razor dosyasındaki tasarımla
    // bu C# kodunun birleşmesini sağlar.
    public partial class TalepOlustur
    {
        // Servisleri kullanmak için "Inject" ediyoruz
        // (Yönergedeki UnitOfWork'ü de ekledik)
        [Inject]
        public MyDbModel_UnitOfWork UnitOfWork { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        // Formu doldurmak için Kategori ve Durum listeleri
        private List<Kategori> kategoriler = new List<Kategori>();
        private List<Durum> durumlar = new List<Durum>();

        // Formun bağlı olduğu Talep nesnesi
        private Talep yeniTalep = new Talep();

        // Kullanıcıya mesaj göstermek için
        private string mesaj = string.Empty;

        // Sayfa ilk yüklendiğinde çalışacak metod
        protected override async Task OnInitializedAsync()
        {
            // Servisimizi (Provider) kullanarak veritabanından kategorileri çek
            kategoriler = await UnitOfWork.Helpdesk.GetKategorilerAsync();
            // Servisimizi (Provider) kullanarak veritabanından durumları çek
            durumlar = await UnitOfWork.Helpdesk.GetDurumlarAsync();
        }

        // Formdaki "Gönder" butonuna basıldığında çalışacak metod
        private async Task HandleTalepOlustur()
        {
            // 1. Giriş yapan kullanıcının kim olduğunu bul
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var mevcutKullaniciId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. "Açık" durumunun Id'sini bul (veya varsay)
            // Not: "Basic" seviye için ID'sinin 1 olduğunu varsayıyoruz.
            // Gerçek projede veritabanından sorgulanmalı.
            var acikDurumId = durumlar.FirstOrDefault(d => d.Ad == "Açık")?.Id ?? 1;

            // 3. Yeni Talep nesnesinin eksik bilgilerini doldur
            yeniTalep.OlusturanKullaniciId = mevcutKullaniciId;
            yeniTalep.OlusturmaTarihi = DateTime.Now;
            yeniTalep.DurumId = acikDurumId; // Varsayılan "Açık" durumu

            // 4. Servisimizi kullanarak veritabanına kaydet
            await UnitOfWork.Helpdesk.AddTalepAsync(yeniTalep);

            // 5. Kullanıcıyı bilgilendir ve formu temizle
            mesaj = "Talebiniz başarıyla oluşturuldu!";
            yeniTalep = new Talep(); // Formu sıfırla
        }
    }
}