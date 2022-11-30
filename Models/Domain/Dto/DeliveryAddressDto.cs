using System.ComponentModel;

namespace Domain
{
    public class DeliveryAddressDto : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>       
        public Guid Id { get; set; } = Guid.Empty;

        public string Remark { get; set; }
        public bool Default { get; set; }

        public Guid MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Address1 { get; set; }


        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

        public string City { get; set; }
        public int CityId { get; set; }

        public string PostalCode { get; set; }

        public string Phone { get; set; }
        public string Mobile { get; set; }
        public bool? Gender { get; set; }
        public string Email { get; set; }

        public string ProvinceName { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }
    }
}
