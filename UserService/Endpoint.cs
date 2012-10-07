using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace UserService
{
	public class UserService : IConfigureThisEndpoint, AsA_Publisher
	{
	}
}
