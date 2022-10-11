namespace HKDG.Domain
{
    public class MbrGroupSelect
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        public double? Discount { get; set; }

        public Guid? Parent_id { get; set; }

        public List<MbrGroupSelect> Children { get; set; }
    }
}
