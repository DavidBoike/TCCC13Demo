using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using UserService.Messages.Commands;
using UserService.Messages.Events;
using log4net;

namespace UserService
{
    public class OtherUserStuffProcessor : IHandleMessages<DoOtherUserSetupGarbageCmd>
    {
        static ILog log = LogManager.GetLogger("CC");

        public IBus Bus { get; set; }

        public void Handle(DoOtherUserSetupGarbageCmd message)
        {
            log.InfoFormat("I did work type {0} for UserId {1}", message.TypeOfWork, message.UserId);

            Bus.Publish<IOtherUserSetupGarbageCompletedEvent>(e =>
            {
                e.UserId = message.UserId;
                e.TypeOfWork = message.TypeOfWork;
            });
        }
    }
}
