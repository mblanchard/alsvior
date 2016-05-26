using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Alsvior.Representations.Config
{
    public class EventHubConfigSection : ConfigurationSection
    {
        #region Properties
        [ConfigurationProperty("Namespace", IsRequired = true)]
        public string Namespace
        {
            get { return (string)this["Namespace"]; }
            set { this["Namespace"] = value; }
        }

        [ConfigurationProperty("ConnectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)this["ConnectionString"]; }
            set { this["ConnectionString"] = value; }
        }

        [ConfigurationProperty("StorageAccountName", IsRequired = true)]
        public string StorageAccountName
        {
            get { return (string)this["StorageAccountName"]; }
            set { this["StorageAccountName"] = value; }
        }

        [ConfigurationProperty("StorageAccountKey", IsRequired = true)]
        public string StorageAccountKey
        {
            get { return (string)this["StorageAccountKey"]; }
            set { this["StorageAccountKey"] = value; }
        }

        [ConfigurationProperty("Hubs", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(EventHubCollection), AddItemName = "hub")]
        public EventHubCollection Hubs
        {
            get
            {
                return (EventHubCollection)base["Hubs"];
            }
        }
        #endregion Properties

        public static EventHubNamespaceConfig GetConfig()
        {
            var configSection = (EventHubConfigSection)ConfigurationManager.GetSection("EventConfig");
            if (configSection == null)
            {
                throw new Exception("Event config section not provided!");
            }
            return new EventHubNamespaceConfig()
            {
                Namespace = configSection.Namespace,
                ConnectionString = configSection.ConnectionString,
                StorageAccountKey = configSection.StorageAccountKey,
                StorageAccountName = configSection.StorageAccountName,
                Hubs = configSection.Hubs.All.Select(x => new EventHubConfig(x)).ToList()
            };

        }
    }


    public class EventHubCollection : ConfigurationElementCollection
    {
        public List<EventHubElement> All { get { return this.Cast<EventHubElement>().ToList(); } }

        public EventHubElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as EventHubElement;
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
            return new EventHubElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EventHubElement)element).Name;
        }
    }

    public class EventHubElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("sendPolicy", IsRequired = false)]
        public string SendPolicy
        {
            get { return (string)this["sendPolicy"]; }
            set { this["sendPolicy"] = value; }
        }

        [ConfigurationProperty("listenPolicy", IsRequired = false)]
        public string ListenPolicy
        {
            get { return (string)this["listenPolicy"]; }
            set { this["listenPolicy"] = value; }
        }
    }
}
