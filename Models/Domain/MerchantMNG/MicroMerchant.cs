namespace Domain
{
    public  class MicroMerchant
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 商家编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string Name { get; set; }

        public string ImagePath { get; set; }

        public bool IsFavorite { get; set; }

        public decimal Score { get; set; }

        public DateTime CreateDate { get; set; }    

        public string LogoB { get; set; }

        public string LogoS { get; set; }
    }

    
}
