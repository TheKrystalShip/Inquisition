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
            await ReplyAsync("", false, builder.Build());
        }

        [Command("add")]
        [Summary("Add a game to the server list")]
        public async Task AddGameAsync(string name, string port = "?", string version = "?")
        {
            await db.AddAsync(new Data.Game { Name = name, Port = port, Version = version });
            await db.SaveChangesAsync();
        }

        [Command("delete")]
        [Summary("Delete a game server from the list")]
        public Task DeleteGameAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            db.Remove(game);
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
