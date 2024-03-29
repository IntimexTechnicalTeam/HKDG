﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ShoppingCartItemDetailView
    {


        public Guid ProductId { get; set; }

        public string ProductCode { get; set; }

        public Guid MerchantId { get; set; }

        public Guid SkuId { get; set; }


        public Guid AttrId1 { get; set; }

        public Guid AttrId2 { get; set; }

        public Guid AttrId3 { get; set; }

        public Guid AttrValue1 { get; set; }

        public Guid AttrValue2 { get; set; }

        public Guid AttrValue3 { get; set; }

        public Guid AttrName1 { get; set; }

        public Guid AttrName2 { get; set; }

        public Guid AttrName3 { get; set; }

        public Guid AttrValueName1 { get; set; }

        public Guid AttrValueName2 { get; set; }

        public Guid AttrValueName3 { get; set; }

        public decimal AttrValue1Price { get; set; }

        public decimal AttrValue2Price { get; set; }

        public decimal AttrValue3Price { get; set; }
    }
}
