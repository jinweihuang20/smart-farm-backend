using System;
using System.Collections.Generic;

namespace Smart_farm.Model
{
    public partial class D1wifi
    {
        public DateTime Datetime { get; set; }
        public double? Humidity { get; set; }
        public int? Raw { get; set; }
        public bool? Relayon { get; set; }
    }
}
