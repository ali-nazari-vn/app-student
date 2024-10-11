using StudentApp.Core.Entities;

namespace StudentApp.Core
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhonNumber { get; set; }
    }
}
