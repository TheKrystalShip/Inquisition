using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Services;

namespace Inquisition.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        // Scroll down further for the AudioService.
        // Like, way down
        private AudioService _service;

        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public AudioModule(AudioService service)
        {
            _service = service;
        }
        
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd()
        {
            System.Console.WriteLine("Command recieved");
            await _service.JoinAudio(Context.Guild, (Context.Message.Author as SocketGuildUser).VoiceChannel);
        }

        // Remember to add preconditions to your commands,
        // this is merely the minimal amount necessary.
        // Adding more commands of your own is also encouraged.

        //[Command("leave", RunMode = RunMode.Async)]
        //public async Task LeaveCmd()
        //{
        //    await _service.LeaveAudio(Context.Guild);
        //}

        //[Command("play", RunMode = RunMode.Async)]
        //public async Task PlayCmd([Remainder] string song)
        //{
        //    await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        //}
    }
}
