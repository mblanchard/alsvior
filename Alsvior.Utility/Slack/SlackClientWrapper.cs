using Alsvior.Utility.Config;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Utility.Slack
{
    public class SlackClientWrapper
    {
        #region Properties
        SlackClient _client;
        SlackConfig _config;
        #endregion Properties

        public SlackClientWrapper(SlackConfig config)
        {
            _config = config;
            _client = new SlackClient(_config.WebhookURL);
        }

        public async Task<bool> PostMessageAsync(string message, string iconName)
        {
            var iconEmoji = _config.Icons.FirstOrDefault(x => x.Name == iconName)?.EmojiName;

            var slackMessage = new SlackMessage() { Channel = _config.Channel, IconEmoji = iconEmoji, Username = _config.Username, Text = message };
            var result = await _client.PostAsync(slackMessage);
            return ((int)result.StatusCode < 400 && String.IsNullOrEmpty(result.ErrorMessage));
        }

        public bool PostMessage(string message, string iconName)
        {
            var iconEmoji = _config.Icons.FirstOrDefault(x => x.Name == iconName)?.EmojiName;

            var slackMessage = new SlackMessage() { Channel = _config.Channel, IconEmoji = iconEmoji, Username = _config.Username, Text = message };
            return _client.Post(slackMessage);
        }

    }
}
