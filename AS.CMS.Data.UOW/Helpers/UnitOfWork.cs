using AS.CMS.Data.UOW.CustomMappings;
using AS.CMS.Data.UOW.Interfaces;
using AS.CMS.Domain.Interfaces;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;


namespace AS.CMS.Data.UOW.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public ISession Session { get; set; }

        static UnitOfWork()
        {
            _sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey("DefaultConnection")))
                .Mappings(x => x.AutoMappings.Add(
                 AutoMap.AssemblyOf<ICMSEntity>(new AutoMappingConfiguration()).UseOverridesFromAssemblyOf<MenuMappings>()
                 .Conventions.AddFromAssemblyOf<SchemaConvention>()
                 .Conventions.AddFromAssemblyOf<EnumConvention>()))
                .ExposeConfiguration(config => new SchemaUpdate(config).Execute(false, false))    
                .BuildSessionFactory();
        }

        public UnitOfWork()
        {
            Session = _sessionFactory.OpenSession();
        }

        public void BeginTransaction()
        {
            if (Session.SessionFactory.IsClosed)
            {
                Session.SessionFactory.OpenSession();
            }

            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {           
            try
            {
                if (Session.Transaction != null && Session.Transaction.IsActive)
                    Session.Transaction.Commit();
            }
            catch
            {
                if (Session.Transaction != null && Session.Transaction.IsActive)
                    Session.Transaction.Rollback();
                throw;
            }
            //finally
            //{
            //    Session.Dispose();
            //}
        }

        public void Rollback()
        {
            try
            {
                if (Session.Transaction != null && Session.Transaction.IsActive)
                    Session.Transaction.Rollback();
            }
            catch
            {
                throw;
            }
        }
    }
}
