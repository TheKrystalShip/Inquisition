using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Inquisition.Services;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService _audioService;
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<AudioService> _logger;

        public AudioModule(AudioService audioService, ReportHandler reportHandler, ILogger<AudioService> logger)
        {
            _audioService = audioService;
            _reportHandler = reportHandler;
            _logger = logger;
        }

        [Command("join")]
        [Summary("Joines the channel of the User or the one passed as an argument")]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            try
            {
                SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser).VoiceChannel;

                if (voiceChannel is null)
                {
                    await ReplyAsync(ReplyHandler.Error.NotInVoiceChannel);
                    return;
                }

                await _audioService.JoinChannel(voiceChannel, Context.Guild.Id);
            }
            catch (Exception e)
            {
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
            }
        }

        [Command("leave")]
        [Alias("fuckoff", "fuck off")]
        [Summary("Kick the bot from the voice channel")]
        public async Task LeaveChannel(IVoiceChannel channel = null)
        {
            try
            {
                await _audioService.LeaveChannel(Context);
            }
            catch (Exception e)
            {
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
            }
        }

        [Command("play")]
        [Summary("Request a song to be played")]
        public async Task PlayCmd([Remainder] string song)
        {
            try
            {
                SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser)?.VoiceChannel;

                if (voiceChannel is null)
                {
                    await ReplyAsync(ReplyHandler.Error.NotInVoiceChannel);
                    return;
                }

                if (Context.Guild.CurrentUser.VoiceChannel != voiceChannel)
                {
                    await _audioService.JoinChannel(voiceChannel, Context.Guild.Id);
                }

                await _audioService.SendAudioAsync(Context.Guild, Context.Channel, song);
            }
            catch (Exception e)
            {
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
            }
        }
    }
}
