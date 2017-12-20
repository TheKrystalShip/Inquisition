using Discord;
using Discord.Audio;
using Discord.Commands;
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
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels =
            new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinChannel(IVoiceChannel channel, ulong guildID)
        {
            var audioClient = await channel.ConnectAsync();
            ConnectedChannels.TryAdd(guildID, audioClient);
        }

        public async Task LeaveChannel(SocketCommandContext Context)
        {
            ConnectedChannels.TryGetValue(Context.Guild.Id, out IAudioClient aClient);

            if (aClient is null)
            {
                await Context.Channel.SendMessageAsync("Bot is not connected to any Voice Channels");
                return;
            }

            await aClient.StopAsync();
            ConnectedChannels.TryRemove(Context.Guild.Id, out aClient);
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
