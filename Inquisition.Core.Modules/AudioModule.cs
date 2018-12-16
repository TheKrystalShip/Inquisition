using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Core.Modules;
using TheKrystalShip.Inquisition.Services;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class AudioModule : Module
    {
        private readonly AudioService _audioService;

        public AudioModule(AudioService audioService)
        {
            _audioService = audioService;
        }

        [Command("join")]
        [Summary("Joines the channel of the User or the one passed as an argument")]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser)?.VoiceChannel;

            if (voiceChannel is null)
            {
                await ReplyAsync("Not in a voice channel");
                return;
            }

            await _audioService.JoinChannel(voiceChannel, Context.Guild.Id);
        }

        [Command("leave")]
        [Alias("fuckoff", "fuck off")]
        [Summary("Kick the bot from the voice channel")]
        public async Task LeaveChannel(IVoiceChannel channel = null)
        {
            await _audioService.LeaveChannel(Context);
        }

        [Command("play")]
        [Summary("Request a song to be played")]
        public async Task PlayCmd([Remainder] string song)
        {
            SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser)?.VoiceChannel;

            if (voiceChannel is null)
            {
                await ReplyAsync("Not in a voice channel");
                return;
            }

            if (Context.Guild.CurrentUser.VoiceChannel != voiceChannel)
            {
                await _audioService.JoinChannel(voiceChannel, Context.Guild.Id);
            }

            await _audioService.SendAudioAsync(Context.Guild, Context.Channel, song);
        }
    }
}
