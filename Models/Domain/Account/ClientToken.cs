
namespace Domain
{
    public class ClientToken
    {
        public ClientToken()
        {
            
        }
        public string Id { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime ExpireDate { get; private set; }

        private int _expireSecond;
        public int ExpireSecond
        {
            get
            {
                return _expireSecond;
            }
            set
            {
                _expireSecond = value;
                ExpireDate = DateTime.Now.AddSeconds(value - 60);// 提前60秒过期
            }
        }




    }
}
