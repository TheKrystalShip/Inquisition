
using Inquisition.Logging;
using Inquisition.Services;

using System;
using System.Collections.Generic;

namespace Inquisition.Handlers
{
	public class ServiceHandler : Handler
	{
		private static List<Service> ServiceList = new List<Service>();
		private const int InitDelay = 5000;
		private const int Interval = 1000;

		public ServiceHandler()
		{
			//ReminderService reminderService = ContainerHandler.Resolve<ReminderService>();
			//DealService dealService = ContainerHandler.Resolve<DealService>();
			//ActivityService activityService = ContainerHandler.Resolve<ActivityService>();

			//RegisterService(reminderService);
			//RegisterService(dealService);
			//RegisterService(activityService);

			//StartAllLoops();
		}

		private void RegisterService(Service service)
		{
			HandleEvents(service);
			ServiceList.Add(service);
		}

		private void HandleEvents(Service service)
		{
			service.Start += Service_Start;
			service.Tick += Service_Tick;
			service.Stop += Service_Stop;
		}

		private void Service_Start(Service service)
		{
			LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Started");
		}

		private void Service_Tick(Service service)
		{
			LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Ticked");
		}

		private void Service_Stop(Service service)
		{
			LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Stopped");
		}

		public static void StartAllLoops()
		{
			LogHandler.WriteLine(LogTarget.Console, "Starting loops...");

			foreach (Service service in ServiceList)
			{
				service.Init(InitDelay, Interval);
			}
		}

		public static void StopAllLoops()
		{
			LogHandler.WriteLine(LogTarget.Console, "Stopping loops...");

			foreach (Service service in ServiceList)
			{
				service.Dispose();
			}
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
