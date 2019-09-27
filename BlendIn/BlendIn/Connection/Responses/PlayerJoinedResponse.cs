using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Responses
{
    public class PlayerJoinedResponse : BaseMessage
    {
        public string user { get; set; }
    }
}
