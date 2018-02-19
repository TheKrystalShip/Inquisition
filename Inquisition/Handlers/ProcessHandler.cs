using Inquisition.Data.Handlers;
using Inquisition.Data.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Inquisition.Handlers
{
	public class ProcessHandler
    {
		private static DbHandler db;
		public static Dictionary<Game, Process> GameProcessDictionary { get; set; } = new Dictionary<Game, Process>();
		public static Dictionary<string, Process> ProcessDictionary { get; set; } = new Dictionary<string, Process>();

		public ProcessHandler(DbHandler dbHandler) => db = dbHandler;

		public static Result StartProcess(string process)
		{
			try
			{
				Game game = db.Games.FirstOrDefault(x => x.Name.ToLower() == process.ToLower());

				if (GameProcessDictionary.ContainsKey(game) || game is null)
					return Result.Failed;

				Process p = new Process()
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = game.FileName,
						Arguments = game.Arguments
					}
				};

				p.OutputDataReceived += P_OutputDataReceived;
				p.ErrorDataReceived += P_ErrorDataReceived;

				p.Start();

				GameProcessDictionary.Add(game, p);
				game.IsOnline = true;
				db.SaveChanges();

				return Result.Successful;
			}
			catch (Exception e)
			{
				LogHandler.WriteLine(e);
				return Result.Failed;
			}
		}

		public static Result StartProcess(string process, string args)
		{
			try
			{
				if (ProcessDictionary.ContainsKey(process))
					return Result.Failed;

				Process p = new Process()
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = process,
						Arguments = args
					}
				};

				p.OutputDataReceived += P_OutputDataReceived;
				p.ErrorDataReceived += P_ErrorDataReceived;

				p.Start();

				ProcessDictionary.Add(process, p);
				return Result.Successful;
			}
			catch (Exception e)
			{
				LogHandler.WriteLine(e);
				return Result.Failed;
			}
		}

		public static Result StopProcess(string process)
		{
			try
			{
				Game game = db.Games.FirstOrDefault(x => x.Name.ToLower() == process.ToLower());

				if (!GameProcessDictionary.ContainsKey(game) || game is null)
				{
					return Result.DoesNotExist;
				}

				GameProcessDictionary.TryGetValue(game, out Process p);
				p.CloseMainWindow();
				p.Close();

				game.IsOnline = false;
				db.SaveChanges();
				return Result.Successful;
			}
			catch (Exception e)
			{
				LogHandler.WriteLine(e);
				return Result.Failed;
			}
		}

		public static Result StopProcess(string process, string args)
		{
			try
			{
				if (!ProcessDictionary.ContainsKey(process))
				{
					return Result.DoesNotExist;
				}

				ProcessDictionary.TryGetValue(process, out Process p);
				p.CloseMainWindow();
				p.Close();
				return Result.Successful;
			}
			catch (Exception e)
			{
				LogHandler.WriteLine(e);
				return Result.Failed;
			}
		}

		private static void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			Console.WriteLine(e);
		}

		private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{

		}
	}
}
