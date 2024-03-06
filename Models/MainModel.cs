using Azure.Core;
using Library.Data;
using Serilog;
using static System.Collections.Specialized.BitVector32;

namespace Library.Models
{
    public class MainModel
    {
        public SysDataContext db { get; set; }
        public List<book> book { get; set; }
        public List<borrower> borrower { get; set; }


        public MainModel(SysDataContext _db)
        {
            db = _db;
        }
        public List<book> GetBooks()
        {
            try
            {
                return db.book.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetBooks metodu hatası: {ErrorMessage}", ex.Message); //hatayı logla

                return new List<book>(); //boş liste dön
            }
        }
        public book GetBooksWithId(int id)
        {
            try
            {
                return db.book.FirstOrDefault(a => a.book_id == id);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetBooksWithId metodu hatası: {ErrorMessage}", ex.Message); //hatayı logla

                return null; //boş dön
            }
        }
        public List<borrower> GetBorrowers()
        {
            try
            {
                return db.borrower.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetBorrowers metodu hatası: {ErrorMessage}", ex.Message); //hatayı logla

                return new List<borrower>(); //boş dön
            }
        }
    }
}
