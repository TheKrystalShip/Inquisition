namespace Inquisition.Data.Models
{
	public enum Result
	{
		Failed,
		Successful,
		Exists,
		DoesNotExist,
		AlreadyRunning,
		Offline,
		Online,
		ProcessRunningButOfflineInDb,
		ProcessNotRunningButOnlineInDb,
		GenericError
	}

	public enum LoopType
	{
		Reminder,
		Deal,
		Activity
	}

	public enum Severity
	{
		Critical,
		Warning
	}
	public enum Type
	{
		Guild,
		General,
		Database,
		Inner
	}
}
