using System.ComponentModel;

namespace Domain
{
    public class DeliveryAddressDto : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>       
        public Guid Id { get; set; } = Guid.Empty;

        public string Remark { get; set; } = string.Empty;
        public bool Default { get; set; } = false;

        public Guid MemberId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Address1 { get; set; } = string.Empty;


        public string Address2 { get; set; } = string.Empty;

        public string Address3 { get; set; } = string.Empty;

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

        public string City { get; set; }
        public int CityId { get; set; }

        public string PostalCode { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public string Email { get; set; } = string.Empty;

        public string ProvinceName { get; set; } = string.Empty;

        public string CityName { get; set; } = string.Empty;

        public string CountryName { get; set; } = string.Empty;
    }
}
