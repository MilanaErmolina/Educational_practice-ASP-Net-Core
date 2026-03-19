using HelloRazorPage.Models;
using HelloRazorPage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelloRazorPage.Pages.Users
{
    public class UsersPageModel : PageModel
    {
        private readonly UserService _userService;

        [BindProperty]
        public User CurrentUser { get; set; } = new();
        public List<User> Users => _userService.GetAll();

        public UsersPageModel(UserService userService)
        {
            _userService = userService;
        }

        public void OnGet() { }

        public JsonResult OnGetUser(int id)
        {
            var user = _userService.GetById(id);
            return new JsonResult(user);
        }

        public IActionResult OnPostSave()
        {
            bool isResult;

            if (CurrentUser.Id == 0)
                isResult = (CurrentUser = _userService.Add(CurrentUser)).Id != 0 ? true : false;
            else
                isResult = _userService.Update(CurrentUser);

            return RedirectToPage();
        }

        //public JsonResult OnPostDelete(int id)
        //{
        //    bool isRersult = _userService.DeleteById(id);
        //    return new JsonResult(new { success = true });
        //}

        public IActionResult OnPostDelete(int id)
        {
            bool isDeleted = _userService.DeleteById(id);
            if (isDeleted)
                return new JsonResult(new { success = true });
            else
                return NotFound();
        }
    }
}