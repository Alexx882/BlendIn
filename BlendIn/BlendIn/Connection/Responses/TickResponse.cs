using System;
using System.Collections.Generic;
using System.Text;
using BlendIn.Game;

namespace BlendIn.Connection.Responses
{
    public class TickResponse : BaseMessage
    {
        public List<Player> userlist { get; set; }
    }
}
