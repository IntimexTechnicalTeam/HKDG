using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SearchCommentsInfo : PageInfo
    {
        /// <summary>
        /// 產品ID
        /// </summary>
        public Guid? ProductId { get; set; } = Guid.Empty;

        /// <summary>
        /// 買家ID
        /// </summary>
        public Guid? ShopperId { get; set; } = Guid.Empty;



    }
}