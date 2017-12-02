using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;
using System.Diagnostics;

namespace Inquisition.Modules
{
    [Group("game")]
    public class GameCommands : ModuleBase<SocketCommandContext>
    {
        InquisitionContext db = new InquisitionContext();

        [Command("help")]
        [Summary("Retuns a list of all commands that can be used in the server group")]
        public async Task HelpAsync()
        {
            await ReplyAsync("Not implemented yet");
        }

        [Command("status")]
        [Summary("Returns if a game server is on")]
        public async Task StatusAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            if (game is null)
            {
                await ReplyAsync($"Sorry, {name} not found in the database");
                return;
            }
            await ReplyAsync(game.IsOnline ? $"{game.Name} server is online" : $"{game.Name} server is offline");
        }

        [Command("start")]
        [Summary("Starts up a game server")]
        public async Task StartGameAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            if (game is null)
            {
                await ReplyAsync($"Sorry, {name} not found in the database");
                return;
            }

            try
            {
                Process p = new Process();

                game.IsOnline = true;
                await db.SaveChangesAsync();

                await ReplyAsync($"{game.Name} server should be online in a few seconds");
            }
            catch (System.Exception ex)
            {
                await ReplyAsync($"Somethng went wrong, couldn't start {game.Name} server, please contact an Administrator.");
                System.Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Command("stop")]
        [Summary("Stops a game server")]
        public async Task StopGameAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            if (game is null)
            {
                await ReplyAsync($"Sorry, {name} not found in the database");
                return;
            }

            try
            {


                game.IsOnline = false;
                await db.SaveChangesAsync();

                await ReplyAsync($"{game.Name} server is shutting down");
            }
            catch (System.Exception ex)
            {
                await ReplyAsync($"Something went wrong, couldn't stop {game.Name} server, please contact an Administrator");
                System.Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Command("list")]
        [Summary("Displays all the game servers with the corresponding port")]
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
            if (game is null)
            {
                await ReplyAsync($"Sorry, {name} not found in the database");
                return;
            }
            db.Remove(game);
            await db.SaveChangesAsync();
            await ReplyAsync($"{game.Name} successfully deleted from the server list");
        }

        [Command("remove")]
        [Summary("Same as delete, removes a game server from the list")]
        public async Task RemoveGameAsync(string name)
        {
            await DeleteGameAsync(name);
        }
    }
}
