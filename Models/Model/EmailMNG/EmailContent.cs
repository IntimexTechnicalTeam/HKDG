using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EmailContent : BaseEntity<Guid>
    {
        public string Content { get; set; }
    }
}
