namespace Model
{
    public class ProductCommentImage : BaseEntity<Guid>
    {
        [Required]
        public Guid CommentId { get; set; }
        [Column(TypeName = "varchar")]
        [StringLength(200)]
        public string ImageS { get; set; }
        [Column(TypeName = "varchar")]
        [StringLength(200)]
        public string ImageB { get; set; }

    }
}
