using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace UserService.Messages.Commands
{
	public class CreateUserCmd : ICommand
	{
		public string Name { get; set; }
		public string Email { get; set; }
	}
}
