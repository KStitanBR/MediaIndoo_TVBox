using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MediaIndoo_TVBox.Helps.Message
{
    public class MessageSentEvent : PubSubEvent<List<string>>
    {
    }
}
