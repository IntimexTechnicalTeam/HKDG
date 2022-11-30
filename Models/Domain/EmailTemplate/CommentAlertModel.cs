namespace Domain
{
    public class CommentAlertModel
    {
        public string OrderNO { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        public string MerchantName { get; set; }
        public string Reply { get; set; }
        public string MerchantCode { get; set; }
    }
}