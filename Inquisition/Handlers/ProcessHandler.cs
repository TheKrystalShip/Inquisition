using Inquisition.Data.Models;
using Inquisition.Database;
using Inquisition.Database.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using TheKrystalShip.Logging;

namespace Inquisition.Handlers
{
    [Obsolete("Needs to be changed, do not use", true)]
	public class ProcessHandler
    {
		private static DatabaseContext db;
		public static Dictionary<Game, Process> GameProcessDictionary { get; set; } = new Dictionary<Game, Process>();
		public static Dictionary<string, Process> ProcessDictionary { get; set; } = new Dictionary<string, Process>();

        private static readonly ILogger<ProcessHandler> _logger = new Logger<ProcessHandler>();

        public ProcessHandler()
		{
			db = new DatabaseContext();
		}

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
                _logger.LogError(e);
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
                _logger.LogError(e);
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
                _logger.LogError(e);
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
                _logger.LogError(e);
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
