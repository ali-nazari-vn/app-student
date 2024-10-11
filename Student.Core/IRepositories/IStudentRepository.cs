namespace StudentApp.Core.IRepositories
{
    public interface IStudentRepository
    {
        Task<Student> GetByIdAsync(int id);
        Task<List<Student>> GetAllStudentsAsync();
        Task<int> InsertStudentAsync(Student student);
        void DeleteStudent(Student student);
        void UpdateStudent(Student student);
    }
}
