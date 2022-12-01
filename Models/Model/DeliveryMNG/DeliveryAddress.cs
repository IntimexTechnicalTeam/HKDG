namespace Model
{
    public class DeliveryAddress : BaseEntity<Guid>
    {
        public string Remark { get; set; } = string.Empty;
        public bool Default { get; set; } = false;

        [Required]
        public Guid MemberId { get; set; }

        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }=string.Empty;

        [Required]
        [StringLength(200)]
        public string LastName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;


        [StringLength(200)]
        public string Address1 { get; set; } = string.Empty;


        [StringLength(200)]
        public string Address2 { get; set; } = string.Empty;

        [StringLength(200)]
        public string Address3 { get; set; } = string.Empty;

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        [NotMapped]
        public int CityId { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty;

        [StringLength(200)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Mobile { get; set; } = string.Empty;
        public bool? Gender { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar")]

        public string Email { get; set; } = string.Empty;
    }
}