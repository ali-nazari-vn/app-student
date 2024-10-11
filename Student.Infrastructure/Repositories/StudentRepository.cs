using Dapper;
using StudentApp.Core;
using StudentApp.Core.Context;
using StudentApp.Core.IRepositories;
using StudentApp.Infrastructure.Utility;


namespace StudentApp.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        #region Dependency Injection

        private readonly StudentAppContext studentAppContext;
        private readonly DapperUtility dapperUtility;

        public StudentRepository(StudentAppContext studentAppContext, DapperUtility dapperUtility)
        {
            this.studentAppContext = studentAppContext;
            this.dapperUtility = dapperUtility;
        }

        #endregion

        #region methods ef

        public async Task<Student> GetByIdAsync(int id) => await studentAppContext.Set<Student>().FindAsync(id);
        public void UpdateStudent(Student student) => studentAppContext.Students.Update(student);

        public async Task<int> InsertStudentAsync(Student student)
        {
            await studentAppContext.Set<Student>().AddAsync(student);
            return student.Id;
        }

        public void DeleteStudent(Student student)
        {
            student.IsDeleted = true;
            student.DeleteDate = DateTime.Now;

            studentAppContext.Set<Student>().Update(student);
        }

        #endregion

        #region methods dapper

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var query = @"SELECT * FROM Students";

            using var connectiossn = dapperUtility.GetConnection();
            var result = await connectiossn.QueryAsync<Student>(query);

            var exerciseClassForSelectList = result.ToList();

            return exerciseClassForSelectList;
        }
            
        #endregion
    }
}
