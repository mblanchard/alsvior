using Alsvior.Representations.Config;
using Alsvior.Representations.Interfaces;
using Autofac;

namespace Alsvior.Weather
{
    public class WeatherDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WeatherClientWrapper>().As<IWeatherClientWrapper>().WithParameter("config", WeatherConfigSection.GetConfig());
            base.Load(builder);
        }
    }
}
