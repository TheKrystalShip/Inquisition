using Discord;
using Discord.Commands;

using System;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Handlers;

namespace TheKrystalShip.Inquisition
{
    public class Module : ModuleBase<SocketCommandContext>
    {
        public Tools Tools { get; private set; }
        public SQLiteContext Database { get; private set; }
        public Guild Guild { get; private set; }
        public User User { get; private set; }

        public Module(Tools tools)
        {
            Tools = tools;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            base.BeforeExecute(command);

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandle;

            Database = Tools.Database;
            Guild = Tools.Database.Guilds.Find(Context.Guild.Id);
            User = Tools.Database.Users.Find(Context.User.Id);

            if (Guild is null)
            {
                Tools.Logger.LogError("Guild is null in current scope");
            }
            else
            {
                Tools.Logger.LogInformation($"Current guild in scope: {Guild.Name}");
            }

            if (User is null)
            {
                Tools.Logger.LogError("User is null in current scope");
            }
            else
            {
                Tools.Logger.LogInformation($"Current user in scope: {User.Username}");
            }
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);

            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandle;

            Tools.Database.Guilds.Update(Guild);
            Tools.Database.Users.Update(User);
            Tools.Database.SaveChanges();
        }

        public async Task<IUserMessage> ReplyAsync(EmbedBuilder embed)
        {
            return await Context.Channel.SendMessageAsync(string.Empty, false, embed.Build());
        }

        public string Reply(ReplyType replyType, string message = null)
        {
            return "";
        }

        private void UnhandledExceptionHandle(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;

            if (exception is null)
                return;

            Tools.Logger.LogError(exception);
            Tools.Report.ReportAsync(exception);
            ReplyAsync(ReplyHandler.Context(Result.Failed)).ConfigureAwait(false);
        }
    }
}
