using System;

namespace Inquisition.Database.Repositories
{
    public interface IRepositoryWrapper
    {
        IActivityRepository Activities { get; }
        IAlertRepository Alerts { get; }
        IDealRepository Deals { get; }
        IGameRepository Games { get; }
        IGuildRepository Guilds { get; }
        IJokeRepository Jokes { get; }
        IReminderRepository Reminders { get; }
        IUserRepository Users { get; }
    }
}
