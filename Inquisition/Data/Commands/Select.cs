using Inquisition.Data.Handlers;
using Inquisition.Data.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Data.Commands
{
	// TODO: Move to Generic types in CrudHandler class
	public class Select
	{
		private static DbHandler db;

		public static User User(User user)
		{
			db = new DbHandler();
			User u = db.Users
					.Where(x => x.Id == user.Id)
					.Include(x => x.Jokes)
					.Include(x => x.Reminders)
					.Include(x => x.Alerts)
					.Include(x => x.TargetAlerts)
					.FirstOrDefault();
			return u;
		}
		public static User User(string id)
		{
			db = new DbHandler();
			User u = db.Users
					.Where(x => x.Id == id)
					.Include(x => x.Jokes)
					.Include(x => x.Reminders)
					.Include(x => x.Alerts)
					.Include(x => x.TargetAlerts)
					.FirstOrDefault();

			var state = db.Entry(u).State;
			return u;
		}
		public static Joke Joke(bool random)
		{
			using (DbHandler db = new DbHandler())
			{
				Joke j = db.Jokes
						.OrderBy(x => Guid.NewGuid())
						.Take(1)
						.Include(x => x.User)
						.FirstOrDefault();

				return j;
			}
		}
		public static Joke Joke(int id, User user)
		{
			using (DbHandler db = new DbHandler())
			{
				Joke j = db.Jokes
							   .Where(x => x.Id == id && x.User == user)
							   .FirstOrDefault();
				return j;
			}
		}
		public static Joke Joke(User user, bool random)
		{
			using (DbHandler db = new DbHandler())
			{
				Joke j = db.Jokes
						.Where(x => x.User == user)
						.OrderBy(x => Guid.NewGuid())
						.Take(1)
						.Include(x => x.User)
						.FirstOrDefault();

				return j;
			}
		}
		public static Reminder Reminder(int id, User user)
		{
			using (DbHandler db = new DbHandler())
			{
				Reminder r = db.Reminders
								   .Where(x => x.Id == id && x.User == user)
								   .FirstOrDefault();
				return r;
			}
		}
		public static Alert Alert(int id, User user)
		{
			using (DbHandler db = new DbHandler())
			{
				Alert a = db.Alerts
								.Where(x => x.Id == id && x.User == user)
								.FirstOrDefault();
				return a;
			}
		}
		public static Alert Alert(User author, User target)
		{
			using (DbHandler db = new DbHandler())
			{
				Alert a = db.Alerts
								.Where(x => x.User == author && x.TargetUser == target)
								.FirstOrDefault();
				return a;
			}
		}
		public static Game Game(string name)
		{
			Game g = db.Games.FirstOrDefault(x => x.Name == name);
			return g;
		}

		public static List<Joke> JokeList()
		{
			using (DbHandler db = new DbHandler())
			{
				List<Joke> Jokes = db.Jokes
										 .Include(x => x.User)
										 .ToList();
				return Jokes;
			}
		}
		public static List<Joke> JokeList(int amount)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Joke> Jokes = db.Jokes
										 .Include(x => x.User)
										 .Take(amount)
										 .ToList();
				return Jokes;
			}
		}
		public static List<Joke> JokeList(User user)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Joke> Jokes = db.Jokes
										 .Where(x => x.User == user)
										 .Include(x => x.User)
										 .ToList();
				return Jokes;
			}
		}
		public static List<Joke> JokeList(int amount, User user)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Joke> Jokes = db.Jokes
										 .Where(x => x.User == user)
										 .Include(x => x.User)
										 .Take(amount)
										 .ToList();
				return Jokes;
			}
		}
		public static List<Reminder> ReminderList()
		{
			using (DbHandler db = new DbHandler())
			{
				List<Reminder> Reminders = db.Reminders
												 .Where(x => x.DueDate <= DateTimeOffset.UtcNow)
												 .Include(x => x.User)
												 .ToList();
				return Reminders;
			}
		}
		public static List<Reminder> ReminderList(int amount)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Reminder> Reminders =
					db.Reminders
					.Where(x => x.DueDate <= DateTimeOffset.UtcNow)
					.Include(x => x.User)
					.Take(amount)
					.ToList();

				return Reminders;
			}
		}
		public static List<Reminder> ReminderList(User user)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Reminder> Reminders = db.Reminders
												 .Where(x => x.User == user)
												 .Include(x => x.User)
												 .ToList();
				return Reminders;
			}
		}
		public static List<Reminder> ReminderList(int amount, User user)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Reminder> Reminders = db.Reminders
												 .Where(x => x.User == user)
												 .Include(x => x.User)
												 .Take(amount)
												 .ToList();
				return Reminders;
			}
		}
		public static List<Alert> AlertList()
		{
			using (DbHandler db = new DbHandler())
			{
				List<Alert> Alerts = db.Alerts
										   .Include(x => x.User)
										   .Include(x => x.TargetUser)
										   .ToList();
				return Alerts;
			}
		}
		public static List<Alert> AlertList(int amount)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Alert> Alerts = db.Alerts
										   .Include(x => x.User)
										   .Include(x => x.TargetUser)
										   .Take(amount)
										   .ToList();
				return Alerts;
			}
		}
		public static List<Alert> AlertList(User user)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Alert> Alerts = db.Alerts
										   .Where(x => x.User == user)
										   .Include(x => x.User)
										   .Include(x => x.TargetUser)
										   .ToList();
				return Alerts;
			}
		}
		public static List<Alert> AlertList(int amount, User user)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Alert> Alerts = db.Alerts
										   .Where(x => x.User == user)
										   .Include(x => x.User)
										   .Include(x => x.TargetUser)
										   .Take(amount)
										   .ToList();
				return Alerts;
			}
		}
		public static List<Alert> TargetAlertList(User targetUser)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Alert> Alerts = db.Alerts
					.Where(x => x.TargetUser == targetUser)
					.Include(x => x.User)
					.Include(x => x.TargetUser)
					.ToList();

				return Alerts;
			}
		}
		public static List<Alert> TargetAlertList(int amount, User targetUser)
		{
			using (DbHandler db = new DbHandler())
			{
				List<Alert> Alerts = db.Alerts
					.Where(x => x.TargetUser == targetUser)
					.Include(x => x.User)
					.Include(x => x.TargetUser)
					.Take(amount)
					.ToList();

				return Alerts;
			}
		}
		public static List<Game> GameList()
		{
			return db.Games.ToList();
		}
		public static List<Game> GameList(int amount)
		{
			return db.Games.Take(amount).ToList();
		}
	}
}