using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace UserService.Messages.Commands
{
	public class CreateUserCertCmd : NServiceBus.ICommand
	{
        public Guid UserId { get; set; }
		public string Email { get; set; }
	}

	
}
