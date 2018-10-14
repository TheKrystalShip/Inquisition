using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Inquisition.Handlers;

namespace TheKrystalShip.Inquisition.Modules
{
    public class JokeModule : Module
    {
        public JokeModule(Tools tools) : base(tools)
        {

        }

        [Command("joke")]
        [Alias("joke by")]
        [Summary("Displays a random joke by random user unless user is specified")]
        public async Task ShowJokeAsync(SocketGuildUser user = null)
        {
            Joke joke;

            if (user is null)
            {
                joke = Database.Jokes
                    .OrderBy(x => Guid.NewGuid())
                    .FirstOrDefault();
            }
            else
            {
                joke = Database.Jokes
                    .Where(x => x.User.Id == user.Id)
                    .OrderBy(x => Guid.NewGuid())
                    .FirstOrDefault();
            }

            if (joke is null)
            {
                await ReplyAsync("There doesn't seem to be any jokes in the database");
                return;
            }

            EmbedBuilder embed = new EmbedBuilder().Create(Context.User)
                .WithTitle($"{joke.Id} - {joke.Text}")
                .WithFooter($"Submitted by {joke.User.Username}", joke.User.AvatarUrl);

            await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
        }

        [Command("jokes")]
        [Alias("jokes by")]
        [Summary("Shows a list of all jokes from all users unless user is specified")]
        public async Task ListJokesAsync(SocketGuildUser user = null)
        {
            List<Joke> Jokes;

            if (user is null)
            {
                Jokes = Database.Jokes
                        .OrderBy(x => Guid.NewGuid())
                        .Take(10)
                        .ToList();
            }
            else
            {
                Jokes = Database.Jokes
                        .Where(x => x.User.Id == user.Id)
                        .OrderBy(x => Guid.NewGuid())
                        .Take(10)
                        .ToList();
            }

            if (Jokes.Count is 0)
            {
                await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
                return;
            }

            EmbedBuilder embed = new EmbedBuilder().Create(Context.User);

            foreach (Joke joke in Jokes)
            {
                embed.AddField($"{joke.Id} - {joke.Text}", $"Submitted by {joke.User.Username}");
            }

            await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
        }

        [Command("add joke")]
        [Summary("Adds a new joke")]
        public async Task AddJokeAsync([Remainder] string jokeText)
        {
            if (jokeText is null)
            {
                await ReplyAsync(ReplyHandler.Error.Command.Joke);
                return;
            }

            Joke joke = new Joke
            {
                Text = jokeText,
                User = User
            };

            Database.Jokes.Add(joke);

            await ReplyAsync(ReplyHandler.Context(Result.Successful));
        }

        [Command("delete joke")]
        [Alias("remove joke")]
        [Summary("Delete a joke")]
        public async Task RemoveJokeAsync(int id)
        {
            Joke joke = Database.Jokes
                .FirstOrDefault(x => x.User.Id == User.Id && x.Id == id);

            if (joke is null)
            {
                await ReplyAsync(ReplyHandler.Error.NotTheOwner);
                return;
            }

            Database.Jokes.Remove(joke);

            await ReplyAsync(ReplyHandler.Context(Result.Successful));
        }
    }
}
