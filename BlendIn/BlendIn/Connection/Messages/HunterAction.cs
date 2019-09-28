using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Messages
{
    public class HunterAction : BaseMessage
    {
        public string lobby { get; set; }
        public string username { get; set; }
        public string caught { get; set; }
    }
}
