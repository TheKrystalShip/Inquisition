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

		public AudioModule(
            AudioService audioService,
            ReportHandler reportHandler,
            ILogger<AudioService> logger)
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

		//[Command("playlist")]
		//[Summary("show a playlist's songs")]
		//public async Task ListPlaylistSongsAsync(int id)
		//{
		//	try
		//	{
		//		User localUser = UserHandler.ToUser(Context.User);
		//		Playlist playlist = Select.Playlist(id);

		//		if (playlist is null)
		//		{
		//			await ReplyAsync(ReplyHandler.Error.NotFound.Playlist);
		//			return;
		//		}

		//		if (playlist.Songs.Count == 0)
		//		{
		//			await ReplyAsync($"{playlist.Name} doesn't have any songs");
		//			return;
		//		}

		//		EmbedBuilder embed = Handlers.EmbedHandler.Create(Context.User);
		//		embed.WithTitle(playlist.Name);

		//		foreach (Song s in playlist.Songs)
		//		{
		//			embed.AddField($"{s.Id} - {s.Name}", $"Duration: {s.Duration}");
		//		}

		//		await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
		//	}
		//	catch (Exception e)
		//	{
		//		ReportService.Report(Context, e);
		//	}
		//}

		//[Command("playlists")]
		//[Alias("playlists by")]
		//[Summary("Shows a user's playlists")]
		//public async Task ListPlaylistsAsync(SocketGuildUser user = null)
		//{
		//	try
		//	{
		//		User localUser;
		//		List<Playlist> Playlists;
		//		switch (user)
		//		{
		//			case null:
		//				localUser = UserHandler.ToUser(Context.User);
		//				Playlists = Select.PlaylistList(10);
		//				break;
		//			default:
		//				localUser = UserHandler.ToUser(user);
		//				Playlists = Select.PlaylistList(10, localUser);
		//				break;
		//		}

		//		if (Playlists.Count == 0)
		//		{
		//			await ReplyAsync(ReplyHandler.Error.NoContent(localUser));
		//			return;
		//		}

		//		EmbedBuilder embed = Handlers.EmbedHandler.Create(Context.User);

		//		foreach (Playlist p in Playlists)
		//		{
		//			embed.AddField($"{p.Id} - {p.Name}, has {p.Songs.Count} songs", $"Created by {p.User.Username}");
		//		}

		//		await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
		//	}
		//	catch (Exception e)
		//	{
		//		ReportService.Report(Context, e);
		//	}
		//}

		//[Command("download")]
		//public async Task DownloadSongAsync([Remainder] string name)
		//{
		//	try
		//	{
		//		AudioService.YTDownload(name);
		//	}
		//	catch (Exception e)
		//	{
		//		ReportService.Report(Context, e);
		//	}
		//}
	}
}