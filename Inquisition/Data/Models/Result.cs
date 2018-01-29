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
}
