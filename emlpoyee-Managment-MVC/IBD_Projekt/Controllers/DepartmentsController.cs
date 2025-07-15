using DatabaseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace IBD_Projekt.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ILogger<DepartmentsController> _logger;
        private readonly DatabaseContext _dbContext;

        public DepartmentsController(ILogger<DepartmentsController> logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public IActionResult Index(string search, string sortField = "ID", string sortOrder = "asc", int page = 1)
        {
            int pageSize = 5;
            var departmentsQuery = _dbContext.Departments.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                departmentsQuery = departmentsQuery.Where(d => d.Name.Contains(search));
            }

            departmentsQuery = sortField switch
            {
                "Name" => (sortOrder == "asc" ? departmentsQuery.OrderBy(d => d.Name) : departmentsQuery.OrderByDescending(d => d.Name)),
                _ => (sortOrder == "asc" ? departmentsQuery.OrderBy(d => d.ID) : departmentsQuery.OrderByDescending(d => d.ID)),
            };

            int totalDepartments = departmentsQuery.Count();

            var departments = departmentsQuery
            .Include(d => d.Users)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();


            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalDepartments / (double)pageSize);
            ViewBag.Search = search;
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }
                

            _dbContext.Departments.Add(department);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = _dbContext.Departments.Include(d => d.Users).FirstOrDefault(d => d.ID == id);
            if (department == null)
                return NotFound();

            if (department.Users.Any())
            {
                TempData["DeleteError"] = "Nie można usunąć działu, który ma przypisanych pracowników.";
                return RedirectToAction("Index");
            }

            _dbContext.Departments.Remove(department);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
