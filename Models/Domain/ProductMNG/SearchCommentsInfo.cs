using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid ProductId { get; set; } = Guid.Empty;

        ///// <summary>
        ///// 買家ID
        ///// </summary>
        //[DefaultValue("00000000-0000-0000-0000-000000000000")]
        //public Guid? ShopperId { get; set; } = Guid.Empty;



    }
}