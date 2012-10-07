using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace UserService.Messages.Events
{
    public interface IOtherUserSetupGarbageCompletedEvent : IEvent
    {
        Guid UserId { get; set; }
        string TypeOfWork { get; set; }
    }
}
