using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NServiceBus;

namespace TCCC13Demo.Web.Controllers
{
	public static class ServiceBus
	{
		public static IBus Bus { get; private set; }

		public static void Init()
		{
			if (Bus != null)
				return;

			lock (typeof(ServiceBus))
			{
				if (Bus != null)
					return;

				Bus = Configure.With()
					.DefineEndpointName("TCCC13Demo.Web")
					.DefaultBuilder()
					.Log4Net()
					.XmlSerializer()
					.MsmqTransport()
						.IsTransactional(false)
						.PurgeOnStartup(true)
					.UnicastBus()
						.ImpersonateSender(false)
					// If we start receiving messages in message handlers, uncomment
					.LoadMessageHandlers()
					.CreateBus()
					.Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
			}

		}
	}
}