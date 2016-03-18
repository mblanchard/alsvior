using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Alsvior.Utility.Config
{
    public class WeatherConfigSection : ConfigurationSection
    {
        #region Properties
        [ConfigurationProperty("APIKey", IsRequired = true)]
        public string APIKey
        {
            get { return (string)this["APIKey"]; }
            set { this["APIKey"] = value; }
        }

        [ConfigurationProperty("Nodes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(WeatherNodeCollection), AddItemName = "node")]
        public WeatherNodeCollection Nodes
        {
            get
            {
                return (WeatherNodeCollection)base["Nodes"];
            }
        }
        #endregion Properties


        public static WeatherConfig GetConfig()
        {
            var configSection = (WeatherConfigSection)ConfigurationManager.GetSection("WeatherConfig");
            if (configSection == null)
            {
                throw new Exception("Weather config section not provided!");
            }
            return new WeatherConfig()
            {
                APIKey = configSection.APIKey,
                Nodes = configSection.Nodes.All.Select(x => new WeatherNode() { Name = x.Name, Latitude = x.Latitude, Longitude = x.Longitude}).ToList()
            };

        }
    }

    public class WeatherNodeCollection : ConfigurationElementCollection
    {
        public List<WeatherNodeElement> All { get { return this.Cast<WeatherNodeElement>().ToList(); } }

        public WeatherNodeElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as WeatherNodeElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new WeatherNodeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WeatherNodeElement)element).Name;
        }
    }

    public class WeatherNodeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        [ConfigurationProperty("lat", IsRequired = true)]
        public float Latitude
        {
            get { return (float)this["lat"]; }
            set { this["lat"] = value; }
        }
        [ConfigurationProperty("long", IsRequired = true)]
        public float Longitude
        {
            get { return (float)this["long"]; }
            set { this["long"] = value; }
        }
    }
}
