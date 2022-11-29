using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ArrivalNotifyCond
    {
        public string ProductCode { get; set; }

        public Guid? AttrVal1 { get; set; } = Guid.Empty;

        public Guid AttrVal2 { get; set; } = Guid.Empty;

        public Guid AttrVal3 { get; set; } = Guid.Empty;

        public string Email { get; set; }
    }
}
