using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data;
using Inquisition.Services;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService audioService;

        public AudioModule(AudioService service)
        {
            audioService = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        [Summary("Joines the channel of the User or the one passed as an argument")]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync(
                    "User must be in a voice channel, or a voice channel must be passed as an argument");
                return;
            }

            await audioService.JoinChannel(channel, Context.Guild.Id);
        }
        
        [Command("leave")]
        [Alias("fuck off")]
        [Summary("Kick the bot from the voice channel")]
        public async Task LeaveChannel(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync(Message.Error.NotInVoiceChannel);
                return;
            }

            await audioService.LeaveChannel(Context);
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
            
            await audioService.SendAudioAsync(Context.Guild, Context.Channel, song);
        }
    }
}