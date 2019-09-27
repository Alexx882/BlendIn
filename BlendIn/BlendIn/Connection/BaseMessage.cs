using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection
{
    public class BaseMessage
    {
        public string @event { get; set; }
        public string status { get; set; }
        public string error { get; set; }
    }
}
