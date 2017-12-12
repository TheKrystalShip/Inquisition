using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.WebSocket;

namespace Inquisition.Services
{
    public class AudioService
    {
        private ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(SocketGuild guild, IVoiceChannel target)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                System.Console.WriteLine("Well the first thing failed");
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                System.Console.WriteLine("Second thing failed");
                return;
            }

            System.Console.WriteLine("About to connect");
            var audioClient = await target.ConnectAsync();

            System.Console.WriteLine("Should have connected by now");

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
                // If you add a method to log happenings from this service,
                // you can uncomment these commented lines to make use of that.
                //await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
            }
        }

        //public async Task LeaveAudio(SocketGuild guild)
        //{
        //    if (ConnectedChannels.TryRemove(guild.Id, out IAudioClient client))
        //    {
        //        await client.StopAsync();
        //        //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
        //    }
        //}

        //public async Task SendAudioAsync(SocketGuild guild, ISocketMessageChannel channel, string path)
        //{
        //    // Your task: Get a full path to the file if the value of 'path' is only a filename.
        //    if (!File.Exists(path))
        //    {
        //        await channel.SendMessageAsync("File does not exist.");
        //        return;
        //    }
        //    IAudioClient client;
        //    if (ConnectedChannels.TryGetValue(guild.Id, out client))
        //    {
        //        //await Log(LogSeverity.Debug, $"Starting playback of {path} in {guild.Name}");
        //        using (var output = CreateStream(path).StandardOutput.BaseStream)
        //        using (var stream = client.CreatePCMStream(AudioApplication.Music))
        //        {
        //            try { await output.CopyToAsync(stream); }
        //            finally { await stream.FlushAsync(); }
        //        }
        //    }
        //}

        //private Process CreateStream(string path)
        //{
        //    return Process.Start(new ProcessStartInfo
        //    {
        //        FileName = "ffmpeg.exe",
        //        Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
        //        UseShellExecute = false,
        //        RedirectStandardOutput = true
        //    });
        //}
    }
}