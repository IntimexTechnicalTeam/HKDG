namespace Domain
{
    public class FunCond : PageInfo
    {
        public FunCond()
        {
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Type = -1;

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public int Type { get; set; }
    }
}
