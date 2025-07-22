using DigitalLibraryConsole.Models;

namespace DigitalLibraryConsole.Interfaces
{
    public interface ILibraryService
    {
        void AddBook(Book book);
        void DeleteBook(int bookId);
        void RegisterUser(User user);
        void DeleteUser(int userId);
        User? GetUserById(int id);
        User? GetUserByName(string name);
        User? GetUserBySurname(string surname);
        User? GetUserByPhoneNumber(string phoneNumber);
        bool BorrowBook(int userId, int bookId);
        bool ReturnBook(int recordId);
        List<Book> GetAvailableBooks(int pageNumber = 1, int pageSize = 10);
        List<User> GetAllUsers(int pageNumber = 1, int pageSize = 10);
        List<LendingRecord> GetAllLendingRecords(int pageNumber = 1, int pageSize = 10);
        List<LendingRecord> GetOverdueRecords(int pageNumber = 1, int pageSize = 10);
        List<LendingRecord> GetOverdueRecordsByUser(int userId, int pageNumber, int pageSize);
        List<LendingRecord> GetOverdueRecordsByBook(int bookId, int pageNumber, int pageSize);
        void DeleteLendingRecord(int id);
        List<LendingRecord> GetLendingRecordsByUser(int userId, int pageNumber, int pageSize);
        List<LendingRecord> GetLendingRecordsByBook(int bookId, int pageNumber, int pageSize);
        LendingRecord? GetLendingRecordById(int id);
        Book? GetBookByTitle(string title);
        List<Book> GetBooksByAuthor(string author, int pageNumber = 1, int pageSize = 10);
        Book? GetBookByISBN(string isbn);
    }
}
