using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Messages
{
    public class LoginMessage : BaseMessage
    {
        public string username { get; set; }

        public string lobby { get; set; }
    }
}
