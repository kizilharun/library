namespace Library.Models
{
    public class BorrowBookModel
    {
        public int book_id { get; set; }
        public string book_name { get; set; }
        public string book_author { get; set; }
        public string book_image_path { get; set; }
        public bool at_library { get; set; }
        public int borrower_id { get; set; }
        public string borrower_name { get; set; }
        public string borrowing_date { get; set; }
        public string return_date { get; set; }
    }
}
