using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Messages
{
    public class PreyAction : BaseMessage
    {
        public string lobby { get; set; }
        public string username { get; set; }
    }
}
