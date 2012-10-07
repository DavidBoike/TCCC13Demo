using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NServiceBus;
using UserService.Messages.Events;
using System.Collections.Concurrent;

namespace TCCC13Demo.Web.Controllers
{
	public class RecentUsers : IHandleMessages<IUserCreatedEventV2>
	{
		public static ConcurrentQueue<string> Recent = new ConcurrentQueue<string>();

		public void Handle(IUserCreatedEventV2 e)
		{
			string text = String.Format("{0} joined at {1:hh:mm:ss}", e.Name, e.TimestampUtc.ToLocalTime());

			Recent.Enqueue(text);

			string removed = null;
			while (Recent.Count > 10)
				Recent.TryDequeue(out removed);
		}
	}
}