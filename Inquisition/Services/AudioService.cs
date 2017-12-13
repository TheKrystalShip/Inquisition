using Discord;
using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inquisition.Services
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(SocketGuild guild, SocketVoiceChannel target)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();
        }

        public async Task LeaveAudio(SocketGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out IAudioClient client))
            {
                await client.StopAsync();
            }
        }
    }
}
