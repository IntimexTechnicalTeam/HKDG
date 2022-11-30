using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ArrivalNotifyCond
    {
        public string ProductCode { get; set; }

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid AttrVal1 { get; set; } = Guid.Empty;

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid AttrVal2 { get; set; } = Guid.Empty;

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid AttrVal3 { get; set; } = Guid.Empty;

        public string Email { get; set; }
    }
}
