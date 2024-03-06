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
                MainModel model = new MainModel(_db); //model olu�tur 
                var books = model.GetBooks(); // t�m kitaplar� getir
                var borrowers = model.GetBorrowers(); // t�m �d�n� alanlar� getir

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
                                          }).OrderBy(book => book.book_name).ToList(); //e�er kitab� �d�n� alan varsa �d�n� alan son ki�iyi getir

                return View(borrowed_book_data); //modeli viewa yolla 
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Index metodu hatas�: {ErrorMessage}", ex.Message); //hatay� logla

                return RedirectToAction("Error", new { error = "Index sayfas�nda bir hata olu�tu." }); // error sayfas�na y�nlendir
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
                Log.Error(ex, "AddBook metodu hatas�: {ErrorMessage}", ex.Message); //hatay� logla

                return RedirectToAction("Error", new { error = "AddBook sayfas�nda bir hata olu�tu." }); //error sayfas�na y�nlendir
            }
        }
        public IActionResult BorrowBook(int book_id)
        {
            try
            {
                MainModel model = new MainModel(_db); //modeli tan�mla
                var borrowed_book = model.GetBooksWithId(book_id); // �d�n� al�nan kitab� getir
                ViewBag.book_name = borrowed_book.book_name; //�d�n� al�nacak kitap ad�
                ViewBag.book_id = borrowed_book.book_id; // �d�n� al�ncak kitap id
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddBook metodu hatas�: {ErrorMessage}", ex.Message); // hatay� logla

                return RedirectToAction("Error", new { error = "BorrowBook sayfas�nda bir hata olu�tu." }); //hata sayfas�na y�nlendir

            }
        }
        public IActionResult SaveNewBook(book formData, IFormFile book_image)
        {
            try
            {
                if (book_image != null && book_image.Length > 0) //image y�klendiyse
                {
                    string web_root_path = _webHostEnvironment.WebRootPath; //wwwroot yolu

                    string image_path = Path.Combine(web_root_path, "media"); //mediay� yola ekle

                    if (!Directory.Exists(image_path)) //media klas�r� yoksa olu�tur
                    {
                        Directory.CreateDirectory(image_path);
                    }

                    string unique_file_name = Guid.NewGuid().ToString() + "_" + book_image.FileName; //her resim i�in uniqe bir isim olustur

                    using (var stream = new FileStream(Path.Combine(image_path, unique_file_name), FileMode.Create)) //resmi belirlenen yola kaydet
                    {
                        book_image.CopyTo(stream); 
                    }

                    formData.book_image_path = Path.Combine("media", unique_file_name); // dbye dosya yolunu kaydet
                }
                _db.book.Add(new book() //kitab� kaydet
                {
                    book_name = formData.book_name.Trim(),
                    book_author = formData.book_author.Trim(),
                    at_library = true,
                    book_image_path = formData.book_image_path?.Trim() ?? "" // E�er null veya bo� ise "" kullan 
                });
                // resimler possttan sonra yok + karakter k�s�t� ya da tamam�n� g�stermeme + loglama

                _db.SaveChanges();

                return Redirect("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveNewBook metodunda hata: {ex.Message}"); //hatay� logla

                return RedirectToAction("Error", new { error = "Kitap kaydedilirken bir hata olu�tu." }); //hata sayfas�na y�nlendir
            }
            
        }
        public IActionResult SaveNewBorrower(borrower formData)
        {
            try
            {
                MainModel model = new MainModel(_db);
                var barrowed_book = model.GetBooksWithId(formData.borrowed_book_id); // �d�n� verilen kitap
                barrowed_book.at_library = false; //�d�n� al�nan kitab� k�t�phaneden ��kar

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
                Console.WriteLine($"SaveNewBorrower metodunda hata: {ex.Message}"); //hatay� logla

                return RedirectToAction("Error", new { error = "Okur kaydedilirken bir hata olu�tu." }); // error sayfas�na y�nlendir
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
