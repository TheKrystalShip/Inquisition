using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;

namespace Inquisition.Modules
{
    [Group("server")]
    public class GameServerCommands : ModuleBase<SocketCommandContext>
    {
        InquisitionContext db = new InquisitionContext();

        [Command("help")]
        [Summary("Retuns a list of all commands that can be used in the server group")]
        public async Task HelpAsync()
        {


            await ReplyAsync("");
        }

        [Command("status")]
        [Summary("Returns if a game server is on")]
        public async Task StatusAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            await ReplyAsync(game.IsOnline ? $"{name} server is online" : $"{name} server is offline");
        }

        [Command("start")]
        [Summary("Starts up a game server")]
        public async Task StartGameAsync(string name)
        {


            await ReplyAsync($"{name} server should be online in a few seconds");
        }

        [Command("stop")]
        [Summary("Stops a game server")]
        public async Task StopGameAsync(string name)
        {


            await ReplyAsync($"{name} server is shutting down");
        }

        [Command("list")]
        [Summary("Displays all the Game Servers with the corresponding port")]
        public async Task ServerListAsync()
        {
            List<Data.Game> Games = db.Games.ToList();
            EmbedBuilder builder = new EmbedBuilder
            {
                Color = Color.Orange,
                Title = "Domain: ffs.game-host.org",
                Description = "List of all game servers with ports"
            };

            foreach (Data.Game game in Games)
            {
                builder.AddInlineField(game.Name, game.Port);
            }
            await ReplyAsync($"Here's the server list {Context.User.Mention}:", false, builder.Build());
        }

        [Command("add")]
        [Summary("Add a game to the server list")]
        public async Task AddGameAsync(string name, string port = "?", string version = "?")
        {
            await db.AddAsync(new Data.Game { Name = name, Port = port, Version = version });
            await db.SaveChangesAsync();
            await ReplyAsync($"{name}, on port {port}, with version {version} successfully added to the server list");
        }

        [Command("delete")]
        [Summary("Delete a game server from the list")]
        public async Task DeleteGameAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            db.Remove(game);
            await db.SaveChangesAsync();
            await ReplyAsync($"{name} successfully deleted from the server list");
        }
    }
}
