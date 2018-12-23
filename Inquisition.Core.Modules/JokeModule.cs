using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class JokeModule : Module
    {
        [Command("joke")]
        [Alias("joke by")]
        [Summary("Displays a random joke by random user unless user is specified")]
        public async Task<RuntimeResult> ShowJokeAsync(SocketGuildUser user = null)
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
                return new ErrorResult(CommandError.ObjectNotFound, "No jokes in the database");
            }

            EmbedBuilder embedBuilder = new EmbedBuilder().Create(Context.User)
                .WithTitle($"{joke.Id} - {joke.Text}")
                .WithFooter($"Submitted by {joke.User.Username}", joke.User.AvatarUrl);

            return new InfoResult("Info", embedBuilder);
        }

        [Command("jokes")]
        [Alias("jokes by")]
        [Summary("Shows a list of all jokes from all users unless user is specified")]
        public async Task<RuntimeResult> ListJokesAsync(SocketGuildUser user = null)
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
                return new ErrorResult(CommandError.ObjectNotFound, "No jokes in the database");
            }

            EmbedBuilder embedBuilder = new EmbedBuilder().Create(Context.User);

            foreach (Joke joke in Jokes)
            {
                embedBuilder.AddField($"{joke.Id} - {joke.Text}", $"Submitted by {joke.User.Username}");
            }

            return new InfoResult("Info", embedBuilder);
        }

        [Command("add joke")]
        [Summary("Adds a new joke")]
        public async Task<RuntimeResult> AddJokeAsync([Remainder] string jokeText)
        {
            if (jokeText is null)
            {
                return new ErrorResult("Joke is empty");
            }

            Joke joke = new Joke
            {
                Text = jokeText,
                User = User
            };

            Database.Jokes.Add(joke);

            return new SuccessResult("Success");
        }

        [Command("delete joke")]
        [Alias("remove joke")]
        [Summary("Delete a joke")]
        public async Task<RuntimeResult> RemoveJokeAsync(int id)
        {
            Joke joke = await Database.Jokes
                .FirstOrDefaultAsync(x => x.User.Id == User.Id && x.Id == id);

            if (joke is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Joke not found in the database");
            }

            Database.Jokes.Remove(joke);

            return new SuccessResult("Success");
        }
    }
}
