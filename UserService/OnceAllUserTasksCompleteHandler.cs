using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using UserService.Messages.Events;
using log4net;

namespace UserService
{
    public class OnceAllUserTasksCompleteHandler : IHandleMessages<IAllUserSagaTasksCompletedEvent>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OnceAllUserTasksCompleteHandler));

        public void Handle(IAllUserSagaTasksCompletedEvent message)
        {
            log.InfoFormat("Hooray! Setup for {0} is complete!", message.Email);
        }
    }
}
