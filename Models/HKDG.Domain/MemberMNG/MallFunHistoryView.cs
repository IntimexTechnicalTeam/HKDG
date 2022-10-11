namespace HKDG.Domain
{
    public class MallFunHistoryView
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set
            {

            }
        }

        public string Email { get; set; }

        public decimal Fun { get; set; }

        public InOut Type { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateString { get; set; }
    }
}
