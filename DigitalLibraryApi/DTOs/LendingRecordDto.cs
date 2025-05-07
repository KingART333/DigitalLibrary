namespace DigitalLibraryApi.DTOs
{
    public class LendingRecordDto
    {
            public int Id { get; set; }
            public string BookTitle { get; set; }
            public string UserFullName { get; set; }
            public DateTime BorrowDate { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime? ReturnDate { get; set; }
    }
}
