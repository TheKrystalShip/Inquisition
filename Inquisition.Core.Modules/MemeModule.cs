using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Tools;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class MemeModule : Module
    {
        [Command("meme")]
        [Alias("meme by")]
        [Summary("Displays a random meme by random user unless user is specified")]
        public async Task ShowMemeAsync(SocketGuildUser user = null)
        {

        }

        [Command("meme random")]
        [Alias("random meme")]
        [Summary("Shows a random meme")]
        public async Task<RuntimeResult> ShowRandomMemeAsync()
        {
            string meme = string.Format("http://images.memes.com/meme/{0}.jpg", new Random().Next(33000));

            Embed embed = EmbedFactory.Create(ResultType.Info, builder => {
                builder.WithTitle(meme);
                builder.WithImageUrl(meme);
            });

            return new SuccessResult(embed);
        }

        [Command("memes")]
        [Alias("memes by")]
        [Summary("Shows a list of all memes from all users unless user is specified")]
        public async Task ListMemesAsync(SocketUser user = null)
        {

        }

        [Command("add meme")]
        [Summary("Adds a new meme")]
        public async Task AddMemeAsync([Remainder] string url)
        {

        }

        [Command("delete meme")]
        [Alias("remove meme")]
        [Summary("Delete a meme")]
        public async Task RemoveMemeAsync(int id)
        {

        }
    }
}
