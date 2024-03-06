using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    public class book
    {
        [Key]
        public int book_id { get; set; }
        public string book_name { get; set; }
        public string book_author { get; set; }
        public bool at_library { get; set; }
        public string book_image_path { get; set; }

    }
}
