using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EmailerMember : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid EmailerId { get; set; }

        [Column(Order = 4)]
        public Guid MemberId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar", Order = 5)]
        public string Email { get; set; }

        [MaxLength(200)]
        [Column(TypeName = "varchar", Order = 6)]
        public string Contact { get; set; }

        [Column(Order = 7)]
        public Language Language { get; set; }


        [ForeignKey("EmailerId")]
        public virtual Emailer Emailer { get; set; }
    }
}
