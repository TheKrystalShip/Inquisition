using Discord.Commands;

using Inquisition.Core.Handlers;

using Inquisition.Database.Commands;
using Inquisition.Database.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Inquisition.Core.Services
{
	//public class GameService
 //   {
	//	public static Dictionary<string, Process> RunningServers { get; set; } = new Dictionary<string, Process>();
	//	public static string Path = $"";

	//	public async Task StartServer(Game game, SocketCommandContext context)
	//	{
	//		try
	//		{
	//			if (RunningServers.TryGetValue(game.Name, out Process temp))
	//			{
	//				await ReportService.Report(ReplyHandler.Error.GameAlreadyRunning(game), context.Message);
	//				return;
	//			}

	//			Process p = new Process();
	//			p.StartInfo.FileName = $"C:\\Windows\\system32\\notepad.exe";
	//			p.StartInfo.Arguments = game.LaunchArgs;
	//			p.Start();

	//			RunningServers.Add(game.Name, p);

	//			game.IsOnline = true;
	//			Update.Game(game);

	//			await context.Channel.SendMessageAsync(ReplyHandler.Info.GameStarting(game));
	//		}
	//		catch (Exception e)
	//		{
	//			ReportService.Report(context, e);
	//		}
	//	}

	//	public async Task StopServer(Game game, SocketCommandContext context)
	//	{
	//		try
	//		{
	//			if (RunningServers.TryGetValue(game.Name, out Process p))
	//			{
	//				p.CloseMainWindow();
	//				p.Close();
	//				RunningServers.Remove(game.Name);

	//				game.IsOnline = false;
	//				Update.Game(game);

	//				await context.Channel.SendMessageAsync(ReplyHandler.Info.GameStopping(game));
	//				return;
	//			}

	//			await context.Channel.SendMessageAsync(ReplyHandler.Error.GameNotRunning(game));
	//		}
	//		catch (Exception e)
	//		{
	//			ReportService.Report(context, e);
	//		}
	//	}

	//	public Result ServerStatus(Game game, SocketCommandContext context)
	//	{
	//		bool ProcessRunning = RunningServers.TryGetValue(game.Name, out Process p);
	//		bool GameMarkedOnline = game.IsOnline;

	//		if (ProcessRunning && GameMarkedOnline)
	//		{
	//			return Result.Online;
	//		}
	//		else if (!ProcessRunning && !GameMarkedOnline)
	//		{
	//			return Result.Offline;
	//		}
	//		else if (ProcessRunning && !GameMarkedOnline)
	//		{
	//			return Result.ProcessRunningButOfflineInDb;
	//		}
	//		else if (!ProcessRunning && GameMarkedOnline)
	//		{
	//			return Result.ProcessNotRunningButOnlineInDb;
	//		}
	//		else
	//		{
	//			return Result.GenericError;
	//		}
	//	}
	//}
}
