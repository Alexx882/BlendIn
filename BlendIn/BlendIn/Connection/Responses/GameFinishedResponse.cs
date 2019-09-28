using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Responses
{
    public class GameFinishedResponse : BaseMessage
    {
        public string winner { get; set; }
    }
}
