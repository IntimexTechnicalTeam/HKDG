
using System.ComponentModel;

namespace Domain
{
    public class AddressInfo 
    {
        [Required]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Id { get; set; }

        [Required]
        public bool Default { get; set; }

        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string ContractAddress { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public string Mobile { get; set; }

        //public string Email { get; set; }

        //public string ProvinceName { get; set; }

        //public string CityName { get; set; }

        //public string CountryName { get; set; }
    }
}
