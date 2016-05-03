using System.Web.Mvc;

namespace L.S.Home.Areas.admin.Controllers
{
    using L.S.Home.Models;
    using L.S.Interface;
    
    public class SysController : LsBaseController
    {
        public IUserService userService;
        public SysController(IUserService _userService)
        {
            userService = _userService;
        }

        [LSAuthorize("SysManage")]
        public ActionResult Index()
        {
            return View();
        }
    }
}