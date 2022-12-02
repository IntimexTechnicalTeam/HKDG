using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ReturnOrderImageView
    {
        /// <summary>
        /// 原圖鏈接
        /// </summary>
        public string ImageB { get; set; }
        /// <summary>
        /// 原圖名稱
        /// </summary>
        public string ImageBName { get; set; }
        /// <summary>
        /// 縮略圖鏈接
        /// </summary>
        public string ImageS { get; set; }
        /// <summary>
        /// 縮略圖名稱
        /// </summary>
        public string ImageSName { get; set; }
    }
}

