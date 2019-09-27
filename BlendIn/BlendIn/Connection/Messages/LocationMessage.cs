using System;
using System.Collections.Generic;
using System.Text;

namespace BlendIn.Connection.Messages
{
    public class LocationMessage : BaseMessage
    {
        public string lobby { get; set; }
        public string username { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
