using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using UserService.Messages.Commands;
using UserService.Messages.Events;
using log4net;

namespace UserService
{
	public class UserCreator : IHandleMessages<CreateUserCmd>
	{
		public IBus Bus { get; set; }

		private static readonly ILog log = LogManager.GetLogger(typeof(UserCreator));

		public void Handle(CreateUserCmd cmd)
		{
			// Do whatever DB work we need to do to create the user
			// An implicit transaction exists, so our DB procedures
			// will enlist in that, along with MSMQ
			log.InfoFormat("We just created {0} with email {1}", cmd.Name, cmd.Email);

			Bus.Publish<IUserCreatedEventV2>(e =>
			{
                e.UserId = Guid.NewGuid();
				e.Email = cmd.Email;
				e.Name = cmd.Name;
				e.TimestampUtc = DateTime.UtcNow;
			});
		}
	}
}
