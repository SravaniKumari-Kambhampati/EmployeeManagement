using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger logger;
		public HomeController(IEmployeeRepository employeeRepository, 
                              IWebHostEnvironment hostingEnvironment,
                              ILogger<HomeController> logger) 
        { 
            this.logger = logger;
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        [AllowAnonymous]
        public ViewResult Index() {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }
        [AllowAnonymous]
        public ViewResult Details(int? id) 
        {
            // throw new Exception("Error in details view");

            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
			logger.LogInformation("Information Log");
			logger.LogCritical("Critical Log");
			logger.LogError("Error Log");
			logger.LogWarning("Warning Log");

			Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null) {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);       
            }

            HomeDetailsViewModel viewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle="Employee Details"
            };
            return View(viewModel);
        }
        [HttpGet]
        public ViewResult Create() {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel );
        }

		[HttpPost]
		public IActionResult Edit(EmployeeEditViewModel model)
		{
			if (ModelState.IsValid)
			{

				Employee employee = _employeeRepository.GetEmployee(model.Id);
				employee.Name = model.Name;
				employee.Department = model.Department;
				employee.Email = model.Email;
				
                if (model.Photo != null) {
                    if(model.ExistingPhotoPath != null)
                    {
                       string filePath= Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
				    employee.PhotoPath = ProcessUploadedFile(model);
				}
				_employeeRepository.Update(employee);
				return RedirectToAction("index");
			}
			return View();
		}

		private string ProcessUploadedFile(EmployeeCreateViewModel model)
		{
			string uniqueFileName = null;
			if (model.Photo != null)
			{
				string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
				model.Photo.CopyTo(fileStream);
			}

			return uniqueFileName;
		}

		[HttpPost]
        public IActionResult  Create(EmployeeCreateViewModel model)
        {
            if(ModelState.IsValid) {
                string uniqueFileName = ProcessUploadedFile(model);
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
				return RedirectToAction("details", new { id = newEmployee.Id });
			}
            return View();
        }

    }
}
