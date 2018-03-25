using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Logging;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Inquisition.Services
{
	public class AudioService : BaseService
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

            try
            {
                await aClient.StopAsync();
                ConnectedChannels.TryRemove(Context.Guild.Id, out aClient);
            }
            catch (Exception)
            {
				LogHandler.WriteLine(LogTarget.Console, "Disconnected");
            }
        }

        public async Task SendAudioAsync(SocketGuild guild, ISocketMessageChannel channel, string path)
        {
            string filePath = $"Data/Music/{path}.mp3";

            if (!File.Exists(filePath))
            {
                await channel.SendMessageAsync("File does not exist.");
                return;
            }

            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                using (var output = CreateStream(filePath).StandardOutput.BaseStream)
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try
                    {
                        await output.CopyToAsync(stream);
                    }
                    catch (Exception)
                    {
						LogHandler.WriteLine(LogTarget.Console, "Stopped audio stream");
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

        public Process YTDownload(string path)
        {
            return Process.Start(new ProcessStartInfo {
                FileName = "Programs/youtube-dl.exe",
                Arguments = $"-i -x --no-playlist --max-filesize 100m --write-thumbnail " +
                $"-o \"Data/Music/%(title)s.mp3\" \"ytsearch:{path}\""
            });
        }
    }
}
