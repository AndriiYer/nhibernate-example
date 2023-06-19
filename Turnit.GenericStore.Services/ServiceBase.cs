using NHibernate;

namespace Turnit.GenericStore.Services
{
    public class ServiceBase
    {
        protected readonly ISession Session;

        public ServiceBase(ISession session) 
        {
            Session = session;
        }
    }
}
