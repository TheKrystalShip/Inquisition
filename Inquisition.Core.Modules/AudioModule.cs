using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System.Threading.Tasks;

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
        public async Task<RuntimeResult> JoinChannel(IVoiceChannel channel = null)
        {
            SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser)?.VoiceChannel;

            if (voiceChannel is null)
            {
                return new ErrorResult("Not in a voice channel");
            }

            await _audioService.JoinChannel(voiceChannel, Context.Guild.Id);
            return new EmptyResult();
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
        public async Task<RuntimeResult> PlayCmd([Remainder] string song)
        {
            SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser)?.VoiceChannel;

            if (voiceChannel is null)
            {
                return new ErrorResult("Not in a voice channel");
            }

            if (Context.Guild.CurrentUser.VoiceChannel != voiceChannel)
            {
                await _audioService.JoinChannel(voiceChannel, Context.Guild.Id);
            }

            await _audioService.SendAudioAsync(Context.Guild, Context.Channel, song);
            return new EmptyResult();
        }
    }
}
