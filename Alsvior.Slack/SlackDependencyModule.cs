using Alsvior.Representations.Config;
using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Slack;
using Autofac;

namespace Alsvior.Slack
{
    public class SlackDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SlackClientWrapper>().As<ISlackClientWrapper>().WithParameter("config", SlackConfigSection.GetConfig());
            base.Load(builder);
        }
    }
}
