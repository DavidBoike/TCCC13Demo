using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace UserService.Messages.Commands
{
    public class DoOtherUserSetupGarbageCmd : ICommand
    {
        public Guid UserId { get; set; }
        public string TypeOfWork { get; set; }
    }
}
