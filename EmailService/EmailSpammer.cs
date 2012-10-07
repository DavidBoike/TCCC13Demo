using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserService.Messages.Events;
using NServiceBus;
using log4net;

namespace EmailService
{
	public class EmailSpammer : IHandleMessages<IUserCreatedEvent>
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(EmailSpammer));

		public void Handle(IUserCreatedEvent e)
		{
			log.InfoFormat("I sent some spam to {0}", e.Email);
		}
	}

	public class EmailSuperSpammer : IHandleMessages<IUserCreatedEventV2>
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(EmailSpammer));

		public void Handle(IUserCreatedEventV2 e)
		{
			log.InfoFormat("MOAR SPAM TO {0}", e.Email);
		}
	}
}
