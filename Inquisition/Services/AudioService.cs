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

        public async Task JoinAudio(SocketGuild guild, SocketVoiceChannel target)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                // Connection already exists
                return;
            }
            
            IAudioClient audioClient = await target.ConnectAsync();
        }

        public async Task LeaveAudio(SocketGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out IAudioClient client))
            {
                await client.StopAsync();
            }
        }

        public async Task SendAudioAsync(SocketGuild guild, ISocketMessageChannel channel, string path)
        {
            // Your task: Get a full path to the file if the value of 'path' is only a filename.
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("File does not exist");
                return;
            }
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                //await Log(LogSeverity.Debug, $"Starting playback of {path} in {guild.Name}");
                using (Stream output = CreateStream(path).StandardOutput.BaseStream)
                using (AudioOutStream stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try {
                        await output.CopyToAsync(stream);
                    }
                    finally
                    {
                        await stream.FlushAsync();
                    }
                }
            }
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "Programs/ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}