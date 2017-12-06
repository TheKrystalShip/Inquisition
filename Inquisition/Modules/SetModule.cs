using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;
using Discord;

namespace Inquisition.Modules
{
    [Group("set")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class SetModule : ModuleBase<SocketCommandContext>
    {
        [Group("game")]
        public class SetGame : ModuleBase<SocketCommandContext>
        {
            InquisitionContext db = new InquisitionContext();

            [Command("name")]
            public async Task SetGameNameAsync(string name, string newName)
            {
                if (name is null || newName is null)
                {
                    await ReplyAsync("Incorrect command structure, use: db port \"[Game]\" \"[NewPort]\"");
                    return;
                }

                Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
                if (game is null)
                {
                    await ReplyAsync($"Sorry, {name} not found in the database");
                    return;
                }
                try
                {
                    game.Name = newName;
                    await db.SaveChangesAsync();
                    await ReplyAsync($"Changed {game.Name}'s name to {newName} successfully");
                }
                catch (Exception ex)
                {
                    await ReplyAsync($"Nope, something went wrong. " +
                            $"Please let the knobhead who programmed this know about it, thanks");
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            [Command("port")]
            public async Task SetGamePortAsync(string name, string port)
            {
                if (name is null || port is null)
                {
                    await ReplyAsync("Incorrect command structure, use: db port \"[Game]\" \"[NewPort]\"");
                    return;
                }

                Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
                if (game is null)
                {
                    await ReplyAsync($"Sorry, {name} not found in the database");
                    return;
                }
                try
                {
                    game.Port = port;
                    await db.SaveChangesAsync();
                    await ReplyAsync($"Changed {game.Name}'s port to {port} successfully");
                }
                catch (Exception ex)
                {
                    await ReplyAsync($"Nope, something went wrong. " +
                            $"Please let the knobhead who programmed this know about it, thanks");
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            [Command("version")]
            public async Task SetGameVersionAsync(string name, string version)
            {
                if (name is null || version is null)
                {
                    await ReplyAsync("Incorrect command structure, use: db port \"[Game]\" \"[NewVersion]\"");
                    return;
                }

                Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
                if (game is null)
                {
                    await ReplyAsync($"Sorry, {name} not found in the database");
                    return;
                } else
                {
                    try
                    {
                        game.Version = version;
                        await db.SaveChangesAsync();
                        await ReplyAsync($"Changed {game.Name}'s version to {version} successfully");
                    }
                    catch (Exception ex)
                    {
                        await ReplyAsync($"Nope, something went wrong. " +
                            $"Please let the knobhead who programmed this know about it, thanks");
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }

            [Command("exe")]
            public async Task SetGameExeAsync(string name, string path)
            {
                if (name is null || path is null)
                {
                    await ReplyAsync("Incorrect command structure, use: set game port \"[Game]\" \"[NewPath]\"");
                    return;
                }

                Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
                if (game is null)
                {
                    await ReplyAsync(Message.Error.GameNotFound(game.Name));
                    return;
                }
                try
                {
                    game.Exe = path;
                    await db.SaveChangesAsync();
                    await ReplyAsync($"Changed {game.Name}'s exe path to {path} successfully");
                }
                catch (Exception ex)
                {
                    await ReplyAsync(Message.Error.Generic);
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
