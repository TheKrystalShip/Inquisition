using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Audio;
using Inquisition.Data;
using Inquisition.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Inquisition.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService _service;

        public AudioModule(AudioService service)
        {
            _service = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        [Summary("Make the bot join your voice channel")]
        public async Task JoinVoiceChannelAsync()
        {
            SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser).VoiceChannel;

            if (voiceChannel is null)
            {
                await ReplyAsync(Message.Error.NotInVoiceChannel);
                return;
            }

            var audioClient = await voiceChannel.ConnectAsync();
        }

        [Command("leave")]
        [Alias("fuck off")]
        [Summary("Kick the bot from the voice channel")]
        public async Task LeaveVoiceChannelAsync()
        {
            SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser).VoiceChannel;

            if (voiceChannel is null)
            {
                await ReplyAsync(Message.Error.NotInVoiceChannel);
                return;
            }

            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        [Summary("Request a song to be played")]
        public async Task PlayCmd([Remainder] string song)
        {
            SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser).VoiceChannel;

            if (voiceChannel is null)
            {
                await ReplyAsync(Message.Error.NotInVoiceChannel);
                return;
            }

            await JoinVoiceChannelAsync();
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }
    }
}