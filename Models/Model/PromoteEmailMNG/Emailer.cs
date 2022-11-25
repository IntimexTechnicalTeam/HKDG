using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Emailer : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid SubjectTransId { get; set; }

        [Column(Order = 4)]
        public Guid TopMessageTransId { get; set; }
        [Column(Order = 5)]
        public Guid BottomMessageTransId { get; set; }

        [Column(Order = 7)]
        public Guid ContactTransId { get; set; }
        [Column(Order = 8)]
        public Guid TitleTransId { get; set; }
        [Column(Order = 9)]
        public Guid ContentSubjectTransId { get; set; }
        [Column(Order = 10)]
        public ImageSizeType ImageSizeType { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "varchar", Order = 11)]
        public string FromEmail { get; set; }

        [Column(Order = 12)]
        public bool Finished { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 13)]
        public string Language { get; set; }

        [Column(Order = 14)]
        public EmailerStatus Status { get; set; }

        [Column(Order = 15)]
        public DateTime? SendDate { get; set; }

        [Column(Order = 16)]
        public DateTime? CompleteDate { get; set; }

        [Column(Order = 17)]
        public int SendTotal { get; set; }

        [Column(Order = 18)]
        public int SuccessTotal { get; set; }

        [Column(Order = 19)]
        public DateTime? ExpectSendDate { get; set; }

        /// <summary>
        /// 是否选择所有会员
        /// </summary>
        [Column(Order = 20)]
        public bool AllMembers { get; set; }

        /// <summary>
        /// 是否包含不接受推广的会员
        /// </summary>
        [Column(Order = 21)]
        public bool IncludeOutPromotion { get; set; }


        //public virtual ICollection<EmailerProduct> EmailerProducts { get; set; }

        //public virtual ICollection<EmailerMember> EmailerMembers { get; set; }

    }
}
