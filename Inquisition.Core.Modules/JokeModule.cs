using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Core.Modules;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class JokeModule : Module
    {
        public JokeModule()
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

            await ReplyAsync(embed);
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
                await ReplyAsync("No content");
                return;
            }

            EmbedBuilder embed = new EmbedBuilder().Create(Context.User);

            foreach (Joke joke in Jokes)
            {
                embed.AddField($"{joke.Id} - {joke.Text}", $"Submitted by {joke.User.Username}");
            }

            await ReplyAsync(embed);
        }

        [Command("add joke")]
        [Summary("Adds a new joke")]
        public async Task AddJokeAsync([Remainder] string jokeText)
        {
            if (jokeText is null)
            {
                await ReplyAsync("No content");
                return;
            }

            Joke joke = new Joke
            {
                Text = jokeText,
                User = User
            };

            Database.Jokes.Add(joke);

            await ReplyAsync("Success");
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
                await ReplyAsync("You're no the owner");
                return;
            }

            Database.Jokes.Remove(joke);

            await ReplyAsync("Success");
        }
    }
}
