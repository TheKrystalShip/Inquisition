using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannelAsync(SocketVoiceChannel voiceChannel = null)
        {
            voiceChannel = voiceChannel ?? (Context.Message.Author as SocketGuildUser)?.VoiceChannel;

            if (voiceChannel is null)
            {
                await ReplyAsync(Message.Error.NotInVoiceChannel);
                return;
            }

            await voiceChannel.ConnectAsync();
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayMusicAsync()
        {

        }

        [Command("queue", RunMode = RunMode.Async)]
        public async Task DisplayQueueAsync()
        {

        }
    }
}
