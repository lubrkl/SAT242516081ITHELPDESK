using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SAT242516081ITHELPDESK.Models; // Modellerimizi kullanacağız
using SAT242516081ITHELPDESK.Models.UnitOfWorks; // UnitOfWork'ü kullanacağız

namespace SAT242516081ITHELPDESK.Components.Pages
{
    public partial class TalepListesi
    {
        // Refactor ettiğimiz yeni sınıfları Inject ediyoruz
        [Inject]
        public MyDbModel_UnitOfWork UnitOfWork { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        // Sayfada gösterilecek taleplerin listesi
        private List<Talep> taleplerim;

        // Sayfa ilk yüklendiğinde
        protected override async Task OnInitializedAsync()
        {
            // Giriş yapan kullanıcıyı al
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Provider'daki yeni metodumuzu kullanarak talepleri çek
            taleplerim = await UnitOfWork.Helpdesk.GetTaleplerimAsync(user);
        }
    }
}