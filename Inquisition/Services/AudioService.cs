using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Services
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
            if (_audioClients.TryGetValue(Context.Guild.Id, out IAudioClient aClient))
            {
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
            else
            {
                await Context.Channel.SendMessageAsync("I'm not connected");
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
                using (Stream output = CreateStream(filePath).StandardOutput.BaseStream)
                using (AudioOutStream stream = client.CreatePCMStream(AudioApplication.Music))
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
            return Process.Start(new ProcessStartInfo
            {
                FileName = "youtube-dl.exe",
                Arguments = $"-i -x --no-playlist --max-filesize 100m --write-thumbnail " +
                $"-o \"Data/Music/%(title)s.mp3\" \"ytsearch:{path}\""
            });
        }
    }
}
