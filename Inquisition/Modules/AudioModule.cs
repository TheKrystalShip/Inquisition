using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Services;
using System;
using System.Threading.Tasks;
using Inquisition.Data;
using Inquisition.Handlers;
using System.Collections.Generic;

namespace Inquisition.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService AudioService;

        public AudioModule(AudioService service)
        {
            AudioService = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        [Summary("Joines the channel of the User or the one passed as an argument")]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            try
            {
                SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser).VoiceChannel;

                if (voiceChannel is null)
                {
                    await ReplyAsync(Reply.Error.NotInVoiceChannel);
                    return;
                }

                await AudioService.JoinChannel(channel, Context.Guild.Id);
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        [Command("leave")]
        [Alias("fuckoff", "fuck off")]
        [Summary("Kick the bot from the voice channel")]
        public async Task LeaveChannel(IVoiceChannel channel = null)
        {
            try
            {
                await AudioService.LeaveChannel(Context);
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        [Command("play", RunMode = RunMode.Async)]
        [Summary("Request a song to be played")]
        public async Task PlayCmd([Remainder] string song)
        {
            try
            {
                SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser).VoiceChannel;

                if (voiceChannel is null)
                {
                    await ReplyAsync(Reply.Error.NotInVoiceChannel);
                    return;
                }

                if (Context.Guild.CurrentUser.VoiceChannel != voiceChannel)
                {
                    await AudioService.JoinChannel(voiceChannel, Context.Guild.Id);
                }

                await AudioService.SendAudioAsync(Context.Guild, Context.Channel, song);
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        [Command("playlist")]
        [Summary("show a playlist's songs")]
        public async Task ListPlaylistSongsAsync(int id)
        {
            try
            {
                User localUser = DbHandler.Select.User(Context.User);
                Playlist playlist = DbHandler.Select.Playlist(id);

                if (playlist is null)
                {
                    await ReplyAsync(Reply.Error.NotFound.Playlist);
                    return;
                }

                if (playlist.Songs.Count == 0)
                {
                    await ReplyAsync($"{playlist.Name} doesn't have any songs");
                    return;
                }

                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
                embed.WithTitle(playlist.Name);

                foreach (Song s in playlist.Songs)
                {
                    embed.AddField($"{s.Id} - {s.Name}", $"Duration: {s.Duration}");
                }

                await ReplyAsync(Reply.Generic, false, embed.Build());
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        [Command("playlists")]
        [Alias("playlists by")]
        [Summary("Shows a user's playlists")]
        public async Task ListPlaylistsAsync(SocketGuildUser user = null)
        {
            try
            {
                User localUser;
                List<Playlist> Playlists;
                switch (user)
                {
                    case null:
                        localUser = DbHandler.Select.User(Context.User);
                        Playlists = DbHandler.Select.Playlists(10);
                        break;
                    default:
                        localUser = DbHandler.Select.User(user);
                        Playlists = DbHandler.Select.Playlists(10, localUser);
                        break;
                }

                if (Playlists.Count == 0)
                {
                    await ReplyAsync(Reply.Error.NoContent(localUser));
                    return;
                }

                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Playlist p in Playlists)
                {
                    embed.AddField($"{p.Id} - {p.Name}, has {p.Songs.Count} songs", $"Created: {p.CreatedAt} by {p.User.Username}");
                }

                await ReplyAsync(Reply.Generic, false, embed.Build());
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        [Command("download", RunMode = RunMode.Async)]
        public async Task DownloadSongAsync([Remainder] string name)
        {
            try
            {
                AudioService.YTDownload(name);
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }
    }

    [Group("add")]
    public class AddAudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService AudioService;

        public AddAudioModule(AudioService service)
        {
            AudioService = service;
        }

        [Command("playlist")]
        [Summary("Create a new playlist")]
        public async Task AddPlaylistAsync([Remainder] string name)
        {
            try
            {
                User localUser = DbHandler.Select.User(Context.User);

                if (localUser.TimezoneOffset is null)
                {
                    await ReplyAsync(Reply.Error.TimezoneNotSet);
                    return;
                }

                Playlist playlist = new Playlist
                {
                    Name = name,
                    User = localUser
                };

                if (DbHandler.Exists(playlist))
                {
                    await ReplyAsync("Already exists in the database");
                    return;
                }

                Result result = DbHandler.Insert.Playlist(playlist);
                await ReplyAsync(Reply.Context(result));
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        //[Command("song")]
        //[Summary("Adds a song to the queue")]
        //public async Task AddSongAsync([Remainder] string name)
        //{
        //    User localUser = DbHandler.Select.User(Context.User);
        //}
    }

    [Group("remove")]
    [Alias("delete")]
    public class RemoveAudioModule : ModuleBase<SocketCommandContext>
    {
        [Command("playlist")]
        [Summary("Delete a playlist")]
        public async Task RemovePlaylistAsync(int id)
        {
            try
            {
                User localUser = DbHandler.Select.User(Context.User);

                Playlist playlist = DbHandler.Select.Playlist(id, localUser);

                if (playlist is null)
                {
                    await ReplyAsync(Reply.Error.NotTheOwner);
                    return;
                }

                Result result = DbHandler.Delete.Playlist(playlist);
                await ReplyAsync(Reply.Context(result));
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }
    }
}