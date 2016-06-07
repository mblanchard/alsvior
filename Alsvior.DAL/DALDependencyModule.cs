using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Config;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alsvior.DAL.Cassandra;

namespace Alsvior.DAL
{
    public class DALDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CassandraClient>().As<ICassandraClient>().WithParameter("config", CassandraConfigSection.GetConfig());
            builder.RegisterType<CassandraSession>().As<ICassandraSession>();
            base.Load(builder);
        }
    }
}
