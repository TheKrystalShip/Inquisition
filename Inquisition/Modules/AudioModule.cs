using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Services;

namespace Inquisition.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private AudioService _service;

        // _services.AddSingleton(new AudioService()) in Program.cs
        public AudioModule(AudioService service)
        {
            _service = service;
        }
        
        [Command("join", RunMode = RunMode.Async)]
        [Summary("make the bot join the voice channel you're connected to")]
        public async Task JoinCmd()
        {
            await _service.JoinAudio(Context.Guild, (Context.Message.Author as SocketGuildUser).VoiceChannel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Make the bot leave the voice channel you're connected to")]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        [Summary("Play a song")]
        public async Task PlayCmd([Remainder] string song)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }
    }
}
