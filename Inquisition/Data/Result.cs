namespace Inquisition.Data
{
    public enum Result
    {
        Failed,
        Successful,
        AlreadyExists,
        DoesNotExist,
        AlreadyRunning,
        Offline,
        Online,
        ProcessRunningButOfflineInDb,
        ProcessNotRunningButOnlineInDb,
        GenericError
    }
}
