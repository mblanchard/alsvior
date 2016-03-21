using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Alsvior.Representations.Config
{
    public class CassandraConfigSection : ConfigurationSection
    {
        #region Properties
        [ConfigurationProperty("Username", IsRequired = true)]
        public string Username
        {
            get { return (string)this["Username"]; }
            set { this["Username"] = value; }
        }

        [ConfigurationProperty("Password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }

        [ConfigurationProperty("Keyspace", IsRequired = true)]
        public string Keyspace
        {
            get { return (string)this["Keyspace"]; }
            set { this["Keyspace"] = value; }
        }

        [ConfigurationProperty("Nodes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CassandraNodeCollection),AddItemName = "node")]
        public CassandraNodeCollection Nodes
        {
            get
            {
                return (CassandraNodeCollection)base["Nodes"];
            }
        }
        #endregion Properties


        public static CassandraConfig GetConfig()
        {
            var configSection = (CassandraConfigSection)ConfigurationManager.GetSection("CassandraConfig");
            if (configSection == null)
            {
                throw new Exception("Cassandra config section not provided!");
            }
            return new CassandraConfig()
            {
                Username = configSection.Username,
                Password = configSection.Password,
                Keyspace = configSection.Keyspace,
                Nodes = configSection.Nodes.All.Select(x => x.IP).ToList()
            };

        }
    }

    public class CassandraNodeCollection : ConfigurationElementCollection
    {
        public List<CassandraNodeElement> All { get { return this.Cast<CassandraNodeElement>().ToList(); } }

        public CassandraNodeElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as CassandraNodeElement;
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
            return new CassandraNodeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CassandraNodeElement)element).Name;
        }
    }

    public class CassandraNodeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        [ConfigurationProperty("ip", IsRequired = true)]
        public string IP
        {
            get { return (string)this["ip"]; }
            set { this["ip"] = value; }
        }
    }
}
