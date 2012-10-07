using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace EmailService
{
	public class EmailService : IConfigureThisEndpoint, AsA_Publisher
	{
	}
}
