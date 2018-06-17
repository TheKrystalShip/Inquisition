namespace Inquisition.Data.Models
{
	public enum Result
	{
		Failed,
		Successful,
		Exists,
		DoesNotExist,
        AlreadyExists,
		AlreadyRunning,
		Offline,
		Online,
		ProcessRunningButOfflineInDb,
		ProcessNotRunningButOnlineInDb,
		GenericError
	}
}
