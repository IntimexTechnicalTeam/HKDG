using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MemberItem
    {
        public string Account { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public Language Language { get; set; } = Language.C;

        public SimpleCurrency Currency { get; set; } = new SimpleCurrency();

        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }

        public bool IsLogin { get; set; } = false;
    }
}
