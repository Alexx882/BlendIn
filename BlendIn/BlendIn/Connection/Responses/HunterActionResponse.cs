using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Responses
{
    public class HunterActionResponse : BaseMessage
    {
        public int? distance { get; set; }
        public int? duration { get; set; }
    }
}
