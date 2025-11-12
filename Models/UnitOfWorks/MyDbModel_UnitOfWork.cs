using SAT242516081ITHELPDESK.Data;
using SAT242516081ITHELPDESK.Models.Providers;
namespace SAT242516081ITHELPDESK.Models.UnitOfWorks
{
    public class MyDbModel_UnitOfWork
    {
        public MyDbModel_Provider Helpdesk { get; }
        public MyDbModel_UnitOfWork(MyDbModel_Provider helpdeskService)
        {
            Helpdesk = helpdeskService;
        }
    }
}
