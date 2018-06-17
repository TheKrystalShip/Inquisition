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
    public class AudioService : Service
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> _audioClients;
        private readonly ILogger<AudioService> _logger;

        public AudioService(ILogger<AudioService> logger)
        {
            _audioClients = new ConcurrentDictionary<ulong, IAudioClient>();
            _logger = logger;
        }

        public async Task JoinChannel(IVoiceChannel channel, ulong guildID)
        {
            var audioClient = await channel.ConnectAsync();
            _audioClients.TryAdd(guildID, audioClient);
        }

        public async Task LeaveChannel(SocketCommandContext Context)
        {
            _audioClients.TryGetValue(Context.Guild.Id, out IAudioClient aClient);

            if (aClient is null)
            {
                await Context.Channel.SendMessageAsync("Bot is not connected to any Voice Channels");
                return;
            }

            try
            {
                await aClient.StopAsync();
                _audioClients.TryRemove(Context.Guild.Id, out aClient);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Disconnected");
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

            if (_audioClients.TryGetValue(guild.Id, out IAudioClient client))
            {
                using (var output = CreateStream(filePath).StandardOutput.BaseStream)
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try
                    {
                        await output.CopyToAsync(stream);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Stopped audio stream");
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
