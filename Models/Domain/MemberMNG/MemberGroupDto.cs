namespace Domain
{
    public class MemberGroupDto
    {

        public MemberGroupDto()
        {
            CreateDate = DateTime.Now;
            CreateBy = Guid.Empty;
            UpdateDate = DateTime.Now;
            UpdateBy = Guid.Empty;
            IsActive = true;
            IsDeleted = false;
        }
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        /// <summary>
        /// 多语言ID
        /// </summary> 
        public Guid NameTransId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 享受折扣,占百分比
        /// </summary>
        public decimal? Discount { get; set; }


        public Guid ParentId { get; set; }


        public List<MemberGroupDto> SubGroup { get; set; }

        /// <summary>
        /// 名称，默认语言保存
        /// </summary>
        //[Required]
        //[StringLength(20)]
        public string Name { get; set; }

        public List<MutiLanguage> Names { get; set; }


        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }


        public DateTime CreateDate { get; set; }


        public DateTime? UpdateDate { get; set; }


        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
