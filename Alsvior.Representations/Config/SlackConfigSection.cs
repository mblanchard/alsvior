using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Alsvior.Representations.Config
{
    public class SlackConfigSection : ConfigurationSection
    {
        #region Properties
        [ConfigurationProperty("WebhookURL", IsRequired = true)]
        public string WebhookURL
        {
            get { return (string)this["WebhookURL"]; }
            set { this["WebhookURL"] = value; }
        }

        [ConfigurationProperty("Channel", IsRequired = true)]
        public string Channel
        {
            get { return (string)this["Channel"]; }
            set { this["Channel"] = value; }
        }

        [ConfigurationProperty("Username", IsRequired = true)]
        public string Username
        {
            get { return (string)this["Username"]; }
            set { this["Username"] = value; }
        }


        [ConfigurationProperty("Icons", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SlackIconCollection), AddItemName = "icon")]
        public SlackIconCollection Icons
        {
            get
            {
                return (SlackIconCollection)base["Icons"];
            }
        }
        #endregion Properties


        public static SlackConfig GetConfig()
        {
            var configSection = (SlackConfigSection)ConfigurationManager.GetSection("SlackConfig");
            if (configSection == null)
            {
                throw new Exception("Slack config section not provided!");
            }
            return new SlackConfig()
            {
                WebhookURL = configSection.WebhookURL,
                Channel = configSection.Channel,
                Username = configSection.Username,
                Icons = configSection.Icons.All.Select(x => new SlackIcon() { Name = x.Name, EmojiName = x.EmojiName }).ToList()
            };

        }
    }

    public class SlackIconCollection : ConfigurationElementCollection
    {
        public List<SlackIconElement> All { get { return this.Cast<SlackIconElement>().ToList(); } }

        public SlackIconElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as SlackIconElement;
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
            return new SlackIconElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SlackIconElement)element).Name;
        }
    }

    public class SlackIconElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
            
        [ConfigurationProperty("emojiName", IsRequired = true)]
        public string EmojiName
        {
            get { return (string)this["emojiName"]; }
            set { this["emojiName"] = value; }
        }
    }
}
