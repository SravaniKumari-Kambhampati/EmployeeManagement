namespace EmployeeManagement.Models
{
	public class SQLEmployeeRepository : IEmployeeRepository
	{
		private readonly AppDbContext context;
		private readonly ILogger<SQLEmployeeRepository> logger;
		public SQLEmployeeRepository(AppDbContext context,
									 ILogger<SQLEmployeeRepository> logger) 
		{
			this.context = context;
			this.logger = logger;
		}
		
		
		public Employee Add(Employee employee)
		{
			context.Employees.Add(employee);
			context.SaveChanges();
			return employee;
		}

		public Employee Delete(int id)
		{
			Employee employee = context.Employees.Find(id);
			if(employee != null) { 
				context.Employees.Remove(employee);
				context.SaveChanges() ;
			}
			return employee;
		}

		public IEnumerable<Employee> GetAllEmployees()
		{
			return context.Employees;
		}

		public Employee GetEmployee(int id)
		{

			logger.LogTrace("Trace Log");
			logger.LogDebug("Debug Log");
			logger.LogInformation("Information Log");
			logger.LogCritical("Critical Log");
			logger.LogError("Error Log");
			logger.LogWarning("Warning Log");

			return context.Employees.Find(id);
		}

		public Employee Update(Employee employeeChanges)
		{
			var employee = context.Employees.Attach(employeeChanges);
			employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			context.SaveChanges();
			return employeeChanges;
		}
	}
}
