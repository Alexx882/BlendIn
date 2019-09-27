using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Responses
{
    public class LoginResponse : BaseMessage
    {
        public string lobby_name { get; set; }
    }
}
