using DatabaseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace IBD_Projekt.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly DatabaseContext _dbContext;

        public UsersController(ILogger<UsersController> logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index(string search, string sortField = "ID", string sortOrder = "asc", int page = 1)
        {
            int pageSize = 5;

            var usersQuery = _dbContext.Users
                .Include(u => u.Departments)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = usersQuery.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search) ||
                    u.Email.Contains(search) ||
                    u.Salary.ToString().Contains(search) ||
                    u.EmploymentDate.ToString().Contains(search) ||
                    u.Departments.Any(d => d.Name.Contains(search))
                );
            }


            usersQuery = sortField switch
            {
                "FirstName" => (sortOrder == "asc" ? usersQuery.OrderBy(u => u.FirstName) : usersQuery.OrderByDescending(u => u.FirstName)),
                "LastName" => (sortOrder == "asc" ? usersQuery.OrderBy(u => u.LastName) : usersQuery.OrderByDescending(u => u.LastName)),
                "Email" => (sortOrder == "asc" ? usersQuery.OrderBy(u => u.Email) : usersQuery.OrderByDescending(u => u.Email)),
                "Salary" => (sortOrder == "asc" ? usersQuery.OrderBy(u => u.Salary) : usersQuery.OrderByDescending(u => u.Salary)),
                "EmploymentDate" => (sortOrder == "asc" ? usersQuery.OrderBy(u => u.EmploymentDate) : usersQuery.OrderByDescending(u => u.EmploymentDate)),
                _ => (sortOrder == "asc" ? usersQuery.OrderBy(u => u.ID) : usersQuery.OrderByDescending(u => u.ID)),
            };

            int totalUsers = usersQuery.Count();

            var users = usersQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
            ViewBag.Search = search;
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;

            return View(users);
        }


        public IActionResult Create()
        {
            ViewBag.Departments = _dbContext.Departments.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _dbContext.Departments.ToList();
                return View(user);
            }

            if (user.SelectedDepartmentIds == null || !user.SelectedDepartmentIds.Any())
            {
                ModelState.AddModelError("SelectedDepartmentIds", "Wybierz co najmniej jeden dział.");
                ViewBag.Departments = _dbContext.Departments.ToList();
                return View(user);
            }

            var selectedDepartments = _dbContext.Departments
                .Where(d => user.SelectedDepartmentIds.Contains(d.ID))
                .ToList();

            User newuser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmploymentDate = user.EmploymentDate,
                Salary = user.Salary,
                Departments = selectedDepartments
            };

            _dbContext.Users.Add(newuser);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _dbContext.Users
                .Include(u => u.Departments)
                .FirstOrDefault(u => u.ID == id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Departments = _dbContext.Departments.ToList();
            user.SelectedDepartmentIds = user.Departments.Select(d => d.ID).ToList();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _dbContext.Departments.ToList();
                return View(user);
            }

            if (user.SelectedDepartmentIds == null || !user.SelectedDepartmentIds.Any())
            {
                ModelState.AddModelError("SelectedDepartmentIds", "Wybierz co najmniej jeden dział.");
                ViewBag.Departments = _dbContext.Departments.ToList();
                return View(user);
            }

            var existingUser = _dbContext.Users
                .Include(u => u.Departments)
                .FirstOrDefault(u => u.ID == user.ID);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.EmploymentDate = user.EmploymentDate;
            existingUser.Salary = user.Salary;

            var selectedDepartments = _dbContext.Departments
                .Where(d => user.SelectedDepartmentIds.Contains(d.ID))
                .ToList();

            existingUser.Departments.Clear();
            foreach (var dept in selectedDepartments)
            {
                existingUser.Departments.Add(dept);
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
