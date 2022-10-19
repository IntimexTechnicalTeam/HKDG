namespace Model
{
    public class IspProvider : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public string Name { get; set; }

        [Column(Order = 4)]
        public string IspType { get; set; } = "HKDG";    //香港设计馆

        [Column(Order = 5)]
        public string AdminUrl { get; set; } = string.Empty;

        [Column(Order = 6)]
        public string WebUrl { get; set; } = string.Empty;

        [Column(Order = 7)]
        public string DbAddress { get; set; } = string.Empty;

        [Column(Order = 8)]
        public string Remark { get; set; } = string.Empty;

    }
}
