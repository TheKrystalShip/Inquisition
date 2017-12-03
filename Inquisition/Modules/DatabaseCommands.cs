using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [Group("db")]
    [RequireUserPermission(Discord.GuildPermission.Administrator)]
    public class DatabaseCommands : ModuleBase<SocketCommandContext>
    {
        Data.InquisitionContext db = new Data.InquisitionContext();

        [Command("add")]
        [Summary("Add a game to the server list")]
        public async Task AddGameAsync(string name, string port = "?", string version = "?")
        {
            await db.AddAsync(new Data.Game { Name = name, Port = port, Version = version });
            await db.SaveChangesAsync();
            await ReplyAsync($"{name}, on port {port}, with version {version} successfully added to the server list");
        }

        [Command("delete")]
        [Alias("remove")]
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

    }
}
