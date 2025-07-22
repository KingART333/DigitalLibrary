namespace DigitalLibraryConsole.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }

        public User() { }
        public User(int age, string name, string surname, string phoneNumber)
        {
            Age = age;
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
        }

    }
}
