using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intimex.Utility
{
    public class PwdUtil
    {
        /// <summary>
        /// 構造通用密碼內容
        /// </summary>
        /// <param name="email">郵箱</param>
        /// <param name="pwd">密碼</param>
        public static string GenPwdBase(string email, string pwd)
        {
            string pwdBase = string.Empty;
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pwd))
            {
                pwdBase = email.Trim().ToLower() + ":" + pwd;
            }
            return pwdBase;
        }
    }
}
