using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MerchantFreeChargeCond
    {

        /// <summary>
        /// 商家主檔記錄ID
        /// </summary>
        public Guid Id { get; set; }

        public List<string> ShipCodes { get; set; }
    }
}
