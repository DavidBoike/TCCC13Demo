using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserService.Messages.Commands;
using NServiceBus;
using log4net;
using UserService.Messages.Events;

namespace UserService
{
	public class CertCreator : IHandleMessages<CreateUserCertCmd>
	{
		static ILog log = LogManager.GetLogger("CC");

		public IBus Bus { get; set; }

		public void Handle(CreateUserCertCmd message)
		{
			log.InfoFormat("I created a cert for {0}", message.Email);

			Bus.Publish<IUserCertCreated>(e =>
			{
                e.UserId = message.UserId;
				e.Email = message.Email;
			});
		}
	}
}
