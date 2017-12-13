using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
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

        public async Task SendAudioAsync(SocketGuild guild, ISocketMessageChannel channel, string path)
        {
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("File does not exist.");
                return;
            }

            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                using (var output = CreateStream(path).StandardOutput.BaseStream)
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try
                    {
                        await output.CopyToAsync(stream);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
