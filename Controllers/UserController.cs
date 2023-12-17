#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffTracking.Data.DbContext;
using StaffTracking.Data.Entities;
using StaffTracking.Models;

namespace StaffTracking.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IConfiguration _config;
        private readonly IUserEmailStore<User> _emailStore;
        
        public UserController(ApplicationDbContext context, IUserStore<User> userStore, UserManager<User> userManager, IConfiguration config)
        {
            _context = context;
            _userStore = userStore;
            _userManager = userManager;
            _config = config;
            _emailStore = GetEmailStore();
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(x => x.Employee).ThenInclude(x => x.EmployeeProfile)
                .Include(x => x.AccessLogs)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    LastLogIn = x.AccessLogs.OrderBy(y => y.Id).LastOrDefault().LogInTime,
                    LastLogOut = x.AccessLogs.OrderBy(y => y.Id).LastOrDefault().LogOutTime,
                    FirstName = x.Employee.EmployeeProfile.FirstName,
                    LastName = x.Employee.EmployeeProfile.LastName,
                    MiddleName = x.Employee.EmployeeProfile.MiddleName
                }).ToListAsync();
            
            return View(users);
        }


        // GET: Transaction/AddOrEdit
        public async Task<IActionResult> AddOrEdit(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new UserDto());
            var user = await _context.Users.Include(x => x.Employee).ThenInclude(x => x.EmployeeProfile)
                .Include(x => x.AccessLogs).SingleAsync(x => x.Id == id);
            return View(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                LastLogIn = user.AccessLogs.MaxBy(y => y.Id)?.LogInTime,
                LastLogOut = user.AccessLogs.MaxBy(y => y.Id)?.LogOutTime,
                FirstName = user.Employee.EmployeeProfile.FirstName,
                LastName = user.Employee.EmployeeProfile.LastName,
                MiddleName = user.Employee.EmployeeProfile.MiddleName
            });
        }

        // POST: Transaction/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,UserName,Email,FirstName,LastName,MiddleName")] UserDto userDto)
        {
            /*if (!ModelState.IsValid) 
                return View(userDto);*/
            
            if (string.IsNullOrEmpty(userDto.Id))
            {
                var user = new User
                {
                    Employee = new Employee
                    {
                        Inn = Guid.NewGuid().ToString(),
                        EmployeeProfile = new EmployeeProfile
                        {
                            FirstName = userDto.FirstName,
                            LastName = userDto.LastName,
                            MiddleName = userDto.MiddleName
                        }
                    },
                    AccessLogs = new List<AccessLog>
                    {
                        new ()
                        {
                            LogInTime = DateTime.UtcNow
                        }
                    }
                };

                await _userStore.SetUserNameAsync(user, userDto.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, userDto.Email, CancellationToken.None);
                await _userManager.CreateAsync(user, _config.GetSection("DefaultPassword").Get<string>());
            }
            else
            {
                var user = await _context.Users.Include(x => x.Employee).ThenInclude(x => x.EmployeeProfile)
                    .Include(x => x.AccessLogs).SingleAsync(x => x.Id == userDto.Id);

                user.UserName = userDto.UserName;
                user.Employee.EmployeeProfile.FirstName = userDto.FirstName;
                user.Employee.EmployeeProfile.LastName = userDto.LastName;
                user.Employee.EmployeeProfile.MiddleName = userDto.MiddleName;
                user.Email = userDto.Email;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
                
            return RedirectToAction(nameof(Index));
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.SingleAsync(x => x.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
