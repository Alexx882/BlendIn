using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Responses
{
    public class TimerResponse : BaseMessage
    {
        public long starttime { get; set; }
        public string hunter_username { get; set; }
    }
}
