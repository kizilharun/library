using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SysDataContext _db;

        public HomeController(SysDataContext db, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public IActionResult Index()
        {
            try
            {
                MainModel model = new MainModel(_db); //model oluþtur 
                var books = model.GetBooks(); // tüm kitaplarý getir
                var borrowers = model.GetBorrowers(); // tüm ödünç alanlarý getir

                var borrowed_book_data = (from book in books
                                          join borrower in borrowers on book.book_id equals borrower.borrowed_book_id into br
                                          let lastBorrower = br.OrderByDescending(b => b.borrower_id).FirstOrDefault()
                                          select new BorrowBookModel
                                          {
                                              book_id = book.book_id,
                                              book_name = book.book_name,
                                              book_author = book.book_author,
                                              book_image_path = "\\" + book.book_image_path,
                                              at_library = book.at_library,
                                              borrower_id = lastBorrower?.borrower_id ?? 0,
                                              borrower_name = lastBorrower?.borrower_name ?? " ",
                                              borrowing_date = lastBorrower?.borrowing_date.ToString("MM/dd/yyyy") ?? " ",
                                              return_date = lastBorrower?.return_date.ToString("MM/dd/yyyy") ?? " ",
                                          }).OrderBy(book => book.book_name).ToList(); //eðer kitabý ödünç alan varsa ödünç alan son kiþiyi getir

                return View(borrowed_book_data); //modeli viewa yolla 
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Index metodu hatasý: {ErrorMessage}", ex.Message); //hatayý logla

                return RedirectToAction("Error", new { error = "Index sayfasýnda bir hata oluþtu." }); // error sayfasýna yönlendir
            }
        }

        public IActionResult AddBook()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddBook metodu hatasý: {ErrorMessage}", ex.Message); //hatayý logla

                return RedirectToAction("Error", new { error = "AddBook sayfasýnda bir hata oluþtu." }); //error sayfasýna yönlendir
            }
        }
        public IActionResult BorrowBook(int book_id)
        {
            try
            {
                MainModel model = new MainModel(_db); //modeli tanýmla
                var borrowed_book = model.GetBooksWithId(book_id); // ödünç alýnan kitabý getir
                ViewBag.book_name = borrowed_book.book_name; //ödünç alýnacak kitap adý
                ViewBag.book_id = borrowed_book.book_id; // ödünç alýncak kitap id
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddBook metodu hatasý: {ErrorMessage}", ex.Message); // hatayý logla

                return RedirectToAction("Error", new { error = "BorrowBook sayfasýnda bir hata oluþtu." }); //hata sayfasýna yönlendir

            }
        }
        public IActionResult SaveNewBook(book formData, IFormFile book_image)
        {
            try
            {
                if (book_image != null && book_image.Length > 0) //image yüklendiyse
                {
                    string web_root_path = _webHostEnvironment.WebRootPath; //wwwroot yolu

                    string image_path = Path.Combine(web_root_path, "media"); //mediayý yola ekle

                    if (!Directory.Exists(image_path)) //media klasörü yoksa oluþtur
                    {
                        Directory.CreateDirectory(image_path);
                    }

                    string unique_file_name = Guid.NewGuid().ToString() + "_" + book_image.FileName; //her resim için uniqe bir isim olustur

                    using (var stream = new FileStream(Path.Combine(image_path, unique_file_name), FileMode.Create)) //resmi belirlenen yola kaydet
                    {
                        book_image.CopyTo(stream); 
                    }

                    formData.book_image_path = Path.Combine("media", unique_file_name); // dbye dosya yolunu kaydet
                }
                _db.book.Add(new book() //kitabý kaydet
                {
                    book_name = formData.book_name.Trim(),
                    book_author = formData.book_author.Trim(),
                    at_library = true,
                    book_image_path = formData.book_image_path?.Trim() ?? "" // Eðer null veya boþ ise "" kullan 
                });
                // resimler possttan sonra yok + karakter kýsýtý ya da tamamýný göstermeme + loglama

                _db.SaveChanges();

                return Redirect("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveNewBook metodunda hata: {ex.Message}"); //hatayý logla

                return RedirectToAction("Error", new { error = "Kitap kaydedilirken bir hata oluþtu." }); //hata sayfasýna yönlendir
            }
            
        }
        public IActionResult SaveNewBorrower(borrower formData)
        {
            try
            {
                MainModel model = new MainModel(_db);
                var barrowed_book = model.GetBooksWithId(formData.borrowed_book_id); // ödünç verilen kitap
                barrowed_book.at_library = false; //ödünç alýnan kitabý kütüphaneden çýkar

                _db.borrower.Add(new borrower() //okuru kaydet
                {
                    borrower_name = formData.borrower_name.Trim(),
                    return_date = formData.return_date.Date,
                    borrowed_book_id = formData.borrowed_book_id,
                    borrowing_date = DateTime.Now.Date,
                });

                _db.SaveChanges();

                return Redirect("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveNewBorrower metodunda hata: {ex.Message}"); //hatayý logla

                return RedirectToAction("Error", new { error = "Okur kaydedilirken bir hata oluþtu." }); // error sayfasýna yönlendir
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
