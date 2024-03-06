-- TR --
# Kütüphane Yönetim Sistemi #
Proje basit bir kütüphane yönetim sistemini simgeler, içeirinde kitap eklenme, ödünç verme gibi temel kütüphane işlemleri bulunmaktadır.

.NET Core 8 kullanılarak geliştirilmiş bir MVC uygulamasıdır. Veritabanı işlemleri için Entity Framework Core Code First kullanılmış, MSSQL veritabanıyla entegre edilmiştir.

Kütüphane bulunan kitapları kaydetmek amacıyla book , ödünç alan okurları kaydetmek adına borrower tabloları oluşturulmuş, tablolar book_id'lerden birbirleri ile ilişkilendirilmiştir.

Eklenilen kitaplar kütüphane anasayfasında gösterilmektedir, kitaplar eğer kütüphanede bulunuyor ise background rengi yeşil durumu "kütüphanede",
eğer ödünç alınmış ve okurda bulunuyor ise background rengi kırmızı durumu "okurda" yapılmıştır.

Kitap eklerken image yüklemesi yapılabilmektedir, resim yüklenilmeyen kitaplara default kitap resmi konulmuştur.

Anasayfada kitap adına göre alfabetik listelenen kitaplar, ödünç ver butonu yardımıyla okurlara ödünç verilebilmektedir. Bir kitap birden çok okur tarafından ödünç alındığı takdirde,
anasayfada bulunan kitap listesinde ödünç alan son okurun adı&soyadı gösterilmektedir.

Uygulama kullanıcı dostu, birbirleri arasında kolayca geçiş sağlanabilen bir tasarıma sahiptir. 

Tüm fonksiyonlarında hata yönetimi bulunmaktadır, olası hatalar logs dosyasında kayıt altına alınır. 

-- EN -- 
 # Library Management System #

The project represents a simple library management system, featuring basic library operations such as adding books and lending.

It is an MVC application developed using .NET Core 8. Entity Framework Core Code First is employed for database operations, integrated with an MSSQL database.

To store the library's collection of books, tables for books and borrowers have been created. These tables are linked through book_id relationships.

The added books are displayed on the library homepage. If a book is available in the library, it is marked with a green background indicating its status as "available." If it has been borrowed and is currently with a reader, it is marked with a red background, indicating its status as "checked out."

When adding a book, an image can be uploaded. Books without an uploaded image are assigned a default book image.

On the homepage, the list of books is alphabetically organized by the book's name. Books can be lent to readers through the "Borrow" button. If a book has been borrowed by multiple readers, the last borrower's name is displayed on the homepage.

The application boasts a user-friendly design, allowing easy navigation between different functionalities.

All functions include error handling, and potential errors are logged for record-keeping.


