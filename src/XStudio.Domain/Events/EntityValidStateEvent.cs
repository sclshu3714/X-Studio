using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Events
{
    public class EntityValidStateEvent : EventData
    {
        public EntityValidStateEvent(EventDefinitionBase eventDefinition, 
                                     Func<EventDefinitionBase, EventData, string> messageGenerator) 
            : base(eventDefinition, messageGenerator)
        {

        }
    }
}
