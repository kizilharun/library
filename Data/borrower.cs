using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    public class borrower
    {
        [Key]
        public int borrower_id { get; set; }
        public string borrower_name { get; set; }
        public DateTime borrowing_date { get; set; }
        public DateTime return_date { get; set; }
        public int borrowed_book_id { get; set; }
    }
}
