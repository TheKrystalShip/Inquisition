using Inquisition.Logging;

namespace Inquisition.Database.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IActivityRepository _activities;
        private IAlertRepository _alerts;
        private IDealRepository _deals;
        private IGameRepository _games;
        private IGuildRepository _guilds;
        private IJokeRepository _jokes;
        private IReminderRepository _reminders;
        private IUserRepository _users;
        private ILogger<RepositoryWrapper> _logger;

        public IActivityRepository Activities
        {
            get
            {
                if (_activities is null)
                {
                    _activities = new ActivityRepository(Context);
                    _activities.ActionExecuted += SubscribeToActionExecuted;
                }
                return _activities;
            }
        }

        public IAlertRepository Alerts
        {
            get
            {
                if (_alerts is null)
                {
                    _alerts = new AlertRepository(Context);
                    _alerts.ActionExecuted += SubscribeToActionExecuted;
                }
                return _alerts;
            }
        }

        public IDealRepository Deals
        {
            get
            {
                if (_deals is null)
                {
                    _deals = new DealRepository(Context);
                    _deals.ActionExecuted += SubscribeToActionExecuted;
                }
                return _deals;
            }
        }

        public IGameRepository Games
        {
            get
            {
                if (_games is null)
                {
                    _games = new GameRepository(Context);
                    _games.ActionExecuted += SubscribeToActionExecuted;
                }
                return _games;
            }
        }

        public IGuildRepository Guilds
        {
            get
            {
                if (_guilds is null)
                {
                    _guilds = new GuildRepository(Context);
                    _guilds.ActionExecuted += SubscribeToActionExecuted;
                }
                return _guilds;
            }
        }

        public IJokeRepository Jokes
        {
            get
            {
                if (_jokes is null)
                {
                    _jokes = new JokeRepository(Context);
                    _jokes.ActionExecuted += SubscribeToActionExecuted;
                }
                return _jokes;
            }
        }

        public IReminderRepository Reminders
        {
            get
            {
                if (_reminders is null)
                {
                    _reminders = new ReminderRepository(Context);
                    _reminders.ActionExecuted += SubscribeToActionExecuted;
                }
                return _reminders;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (_users is null)
                {
                    _users = new UserRepository(Context);
                    _users.ActionExecuted += SubscribeToActionExecuted;
                }
                return _users;
            }
        }

        private readonly DatabaseContext Context;

        public RepositoryWrapper(DatabaseContext context)
        {
            Context = context;
            _logger = new Logger<RepositoryWrapper>();
        }

        private void SubscribeToActionExecuted(Message message)
        {
            _logger.LogInformation(message.Content);
        }
    }
}
