using System.ComponentModel;

namespace Model
{
    public class ProductCommentImageDto 
    {
        
        public Guid CommentId { get; set; }
 
        public string ImageS { get; set; }
        
        public string ImageB { get; set; }

        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }

   
        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }

        public string ImageName { get; set; }
    }
}
