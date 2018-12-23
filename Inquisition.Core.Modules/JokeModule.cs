using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Tools;

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
                joke = await Database.Jokes
                    .OrderBy(x => Guid.NewGuid())
                    .FirstOrDefaultAsync();
            }
            else
            {
                joke = await Database.Jokes
                    .Where(x => x.User.Id == user.Id)
                    .OrderBy(x => Guid.NewGuid())
                    .FirstOrDefaultAsync();
            }

            if (joke is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "No jokes in the database");
            }                

            Embed embed = EmbedFactory.Create(ResultType.Info, builder => {
                builder.WithTitle($"{joke.Id} - {joke.Text}");
                builder.WithFooter($"Submitted by {joke.User.Username}", joke.User.AvatarUrl);
            });

            return new InfoResult(embed);
        }

        [Command("jokes")]
        [Alias("jokes by")]
        [Summary("Shows a list of all jokes from all users unless user is specified")]
        public async Task<RuntimeResult> ListJokesAsync(SocketGuildUser user = null)
        {
            List<Joke> jokesList;

            if (user is null)
            {
                jokesList = await Database.Jokes
                        .OrderBy(x => Guid.NewGuid())
                        .Take(10)
                        .ToListAsync();
            }
            else
            {
                jokesList = await Database.Jokes
                        .Where(x => x.User.Id == user.Id)
                        .OrderBy(x => Guid.NewGuid())
                        .Take(10)
                        .ToListAsync();
            }

            if (jokesList.Count is 0)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "No jokes in the database");
            }

            Embed embed = EmbedFactory.Create(ResultType.Info, builder => {
                builder.WithTitle("Here's the list of jokes");
                foreach (Joke joke in jokesList)
                {
                    builder.AddField($"{joke.Id} - {joke.Text}", $"Submitted by {joke.User.Username}");
                }
            });

            return new InfoResult(embed);
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

            await Database.Jokes.AddAsync(joke);

            return new SuccessResult();
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

            return new SuccessResult();
        }
    }
}
