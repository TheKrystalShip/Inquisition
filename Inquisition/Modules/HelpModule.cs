using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [Group("help")]
    [Alias("how to", "how do i", "tell me how to", "command to", "what is the command to", "commands")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("")]
        public async Task HelpCommandsAsync()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.WithAuthor(Context.User);
            embedBuilder.WithDescription("Optional: [] - Mandatory: {}");
            embedBuilder.AddField("add/create/make [a]/[a new]", "joke, meme, reminder, game");
            embedBuilder.AddField("show/tell [me]/[me a]", "joke, meme, reminder, game");
            embedBuilder.AddField("list/show/tell {me all}", "jokes, memes, reminders, games");
            
            await ReplyAsync($"Here's a list of all available commands {Context.User.Mention}:", false, embedBuilder.Build());
        }

        [Group("add")]
        [Alias("add a", "add a new", "create", "create a", "create a new", "make", "make a", "make a new")]
        public class HelpAddModule : ModuleBase<SocketCommandContext>
        {
            [Command("meme")]
            [Alias("shitpost")]
            public async Task HelpAddMemeAsync()
            {
                List<string> Commands = new List<string>
                {
                    "meme", "a meme", "new meme", "a new meme"
                };
                EmbedBuilder embedBuilder = new EmbedBuilder();
                int i = 1;

                foreach (string item in Commands)
                {
                    embedBuilder.AddField($"Option {i}:", $"add {item} URL/Link");
                    i++;
                }
                await ReplyAsync($"To add a new meme you can use:", false, embedBuilder.Build());
            }

            [Command("game")]
            public async Task HelpAddGameAsync()
            {
                await ReplyAsync($"Yeah only the admin can do that, sorry");
            }

            [Command("joke")]
            public async Task HelpAddJokeAsync()
            {
                List<string> Commands = new List<string>
                {
                    "joke", "a joke", "new joke", "a new joke"
                };

                EmbedBuilder embedBuilder = new EmbedBuilder();
                int i = 1;

                foreach (string item in Commands)
                {
                    embedBuilder.AddField($"Option {i}:", $"add {item} \"Text\"");
                    i++;
                }
                await ReplyAsync($"To add a new joke you can use:", false, embedBuilder.Build());
            }

            [Command("reminder")]
            public async Task HelpAddReminderAsync()
            {
                List<string> Commands = new List<string>
                {
                    "reminder", "a reminder", "new reminder", "a new reminder"
                };

                EmbedBuilder embedBuilder = new EmbedBuilder();
                int i = 1;

                foreach (string item in Commands)
                {
                    embedBuilder.AddField($"Option {i}:", $"add {item} \"(Timer example:) 30m\" \"Message\"");
                    i++;
                }
                await ReplyAsync($"To add a new reminder you can use:", false, embedBuilder.Build());
            }
        }

        [Group("ask")]
        [Alias("ask for", "ask for a","get", "get a", "make you show", "make you show me")]
        public class HelpShowModule : ModuleBase<SocketCommandContext>
        {
            
        }
    }
}
