using Discord;
using Discord.Commands;

using System;
using System.Threading.Tasks;

using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class Module : ModuleBase<SocketCommandContext>
    {
        public IDbContext Database { get; private set; }
        public Guild Guild { get; private set; }
        public User User { get; private set; }
        public IPrefixHandler Prefix { get; private set; }
        public ILogger<Module> Logger { get; private set; }

        public Module()
        {
            Database = Container.Get<IDbContext>();
            Prefix = Container.Get<IPrefixHandler>();
            Logger = new Logger<Module>();
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            base.BeforeExecute(command);

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandle;

            Guild = Database.Guilds.Find(Context.Guild.Id);
            User = Database.Users.Find(Context.User.Id);

            if (Guild is null)
            {
                Logger.LogError(GetType().Name, "Guild is null in current scope");
            }

            if (User is null)
            {
                Logger.LogError(GetType().Name, "User is null in current scope");
            }
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);

            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandle;

            Database.Guilds.Update(Guild);
            Database.Users.Update(User);
            Database.SaveChanges();
        }

        public async Task<IUserMessage> ReplyAsync(EmbedBuilder embedBuilder)
        {
            return await Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }

        public async Task<IUserMessage> ReplyAsync(Embed embed)
        {
            return await Context.Channel.SendMessageAsync(string.Empty, false, embed);
        }

        private void UnhandledExceptionHandle(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;

            if (exception is null)
                return;

            Logger.LogError(exception);
        }
    }
}
