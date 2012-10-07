using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace UserService.Messages.Events
{
    public interface IAllUserSagaTasksCompletedEvent : IEvent
    {
        Guid UserId { get; set; }
        string Email { get; set; }
        string Name { get; set; }
    }
}
