using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MemberPreference : BaseEntity<Guid>
    {
        [Required]
        public Guid MemberId { get; set; }
        [Required]
        public DataSources DataSource { get; set; }
        [Required]
        public PreferenceDataType DataType { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Value { get; set; }
    }
}
