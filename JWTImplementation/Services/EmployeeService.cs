using JWTImplementation.Context;
using JWTImplementation.Interfaces;
using JWTImplementation.Models;

namespace JWTImplementation.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly JwtContext _jwtContext;
        public EmployeeService(JwtContext jwtContext)
        {
            _jwtContext = jwtContext;
        }

        public Employee AddEmployee(Employee employee)
        {
            var emp = _jwtContext.Employees.Add(employee);
            _jwtContext.SaveChanges();
            return emp.Entity;
        }

        public bool DeleteEmployee(int id)
        {
            try
            {
                var emp = _jwtContext.Employees.SingleOrDefault(x => x.Id == id);
                if (emp == null)
                {
                    throw new Exception("User not found");
                }
                else
                {
                    _jwtContext.Employees.Remove(emp);
                    _jwtContext.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<Employee> GetEmployeeDetails()
        {
            return _jwtContext.Employees.ToList();
        }

        public Employee GetEmployeeDetails(int id)
        {
            var emp = _jwtContext.Employees.SingleOrDefault(x => x.Id == id);
            return emp;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var updated = _jwtContext.Employees.Update(employee);
            _jwtContext.SaveChanges();
            return updated.Entity;
        }
    }
}
