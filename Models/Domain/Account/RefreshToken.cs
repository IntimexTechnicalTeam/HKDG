using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Account
{
    public class RefreshToken
    {
        public string token { get; set; }

        public string CurrencyCode { get; set; }

        public Language Lang { get; set; }
    }
}
