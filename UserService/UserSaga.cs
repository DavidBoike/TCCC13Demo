using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using NServiceBus.Saga;
using UserService.Messages.Events;
using UserService.Messages.Commands;
using log4net;

namespace UserService
{
	public class UserSaga : Saga<UserSagaData>,
		IAmStartedByMessages<IUserCreatedEventV2>,
		IHandleMessages<IUserCertCreated>,
        IHandleMessages<IOtherUserSetupGarbageCompletedEvent>
	{
        private static ILog log = LogManager.GetLogger(typeof(UserSaga));

        // This method describes how to find the saga data based on properties in it.
        public override void ConfigureHowToFindSaga()
        {
            // We don't need to do anything for IUserCreatedEventV2, as it starts things up
            // For the other messages, match up the UserId properties between the saga data
            // and the incoming messages using LINQ expressions
            this.ConfigureMapping<IUserCertCreated>(data => data.UserId, msg => msg.UserId);
            this.ConfigureMapping<IOtherUserSetupGarbageCompletedEvent>(data => data.UserId, msg => msg.UserId);
        }

		public void Handle(IUserCreatedEventV2 e)
		{
            this.Data.UserId = e.UserId;
			this.Data.Email = e.Email;
            this.Data.Name = e.Name;

            log.InfoFormat("Creating new UserSaga for ID {0}, {1} and firing off messages for a bunch of work to be done.", e.UserId, e.Email);

            Bus.Send(new CreateUserCertCmd
            {
                UserId = e.UserId,
                Email = e.Email
            });

            Bus.Send(new DoOtherUserSetupGarbageCmd { UserId = e.UserId, TypeOfWork = "1of3" });
            Bus.Send(new DoOtherUserSetupGarbageCmd { UserId = e.UserId, TypeOfWork = "2of3" });
            Bus.Send(new DoOtherUserSetupGarbageCmd { UserId = e.UserId, TypeOfWork = "3of3" });
		}

        public void Handle(IUserCertCreated message)
        {
            Data.CertIsCreated = true;
            log.InfoFormat("{0}: The certificate has been created.", Data.Email);
            CheckForCompletion();
        }

        public void Handle(IOtherUserSetupGarbageCompletedEvent message)
        {
            Data.OtherWorkDone.Add(message.TypeOfWork);
            log.InfoFormat("{0}: Other Work {1} has been completed.", Data.Email, message.TypeOfWork);
            CheckForCompletion();
        }

        private void CheckForCompletion()
        {
            // We don't know in what order the messages will arrive, so we check for all
            // of our business logic to see if everything has completed.

            if (!Data.CertIsCreated)
                log.InfoFormat("{0}: The certificate has not been created yet.", Data.Email);
            else if(!Data.OtherWorkDone.Contains("1of3"))
                log.InfoFormat("{0}: Other Work 1 of 3 has not completed  yet.", Data.Email);
            else if(!Data.OtherWorkDone.Contains("2of3"))
                log.InfoFormat("{0}: Other Work 2 of 3 has not completed  yet.", Data.Email);
            else if (!Data.OtherWorkDone.Contains("3of3"))
                log.InfoFormat("{0}: Other Work 3 of 3 has not completed  yet.", Data.Email);
            else
            {
                Bus.Publish<IAllUserSagaTasksCompletedEvent>(e =>
                {
                    e.UserId = Data.UserId;
                    e.Email = Data.Email;
                    e.Name = Data.Name;
                });

                // We are all done, so time to delete this saga data
                this.MarkAsComplete();
            }
        }
    }

    /// <summary>
    /// Sagas do not automatically subscribe to the events they consume, because the Saga
    /// code may be deployed to multiple endpoints. So, we use this IWantToRunAtStartup
    /// class to manually subscribe to all of the events our Saga consumes. The endpoint
    /// knows where to send the subscription request because of the mapping in the
    /// app.config file.
    /// </summary>
    public class UserSagaSubscriber : IWantToRunAtStartup
    {
        public IBus Bus { get; set; }

        public void Run()
        {
            Bus.Subscribe<IUserCreatedEventV2>();
            Bus.Subscribe<IUserCertCreated>();
            Bus.Subscribe<IOtherUserSetupGarbageCompletedEvent>();
        }

        public void Stop() { }
    }

	public class UserSagaData : ISagaEntity
	{
        public Guid UserId { get; set; }
        public string Name { get; set; }
		public string Email { get; set; }
		public bool CertIsCreated { get; set; }
        public List<string> OtherWorkDone { get; set; }

        public UserSagaData()
        {
            OtherWorkDone = new List<string>();
        }

		// NServiceBus Internal - LEAVE THIS ALONE!
		public Guid Id { get; set; }
		public string OriginalMessageId { get; set; }
		public string Originator { get; set; }
	}
}
