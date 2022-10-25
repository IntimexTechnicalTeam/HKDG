namespace Domain
{
    public class StoreAddressView
    {
        public Guid RelevanceId { get; set; }

        public Guid MerchantId { get; set; }

        public string MerchantName { get; set; }

        public List<MutiLanguage> NameList { get; set; }

        public List<MutiLanguage> AddressList { get; set; }

        public List<MutiLanguage> RemarkList { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }


        public DateTime CreateDate { get; set; }

        public string CreateDateString { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateDateString { get; set; }
    }
}
