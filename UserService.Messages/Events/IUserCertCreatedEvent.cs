using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace UserService.Messages.Events
{
	public interface IUserCertCreated : IEvent
	{
        Guid UserId { get; set; }
		string Email { get; set; }
	}
}
