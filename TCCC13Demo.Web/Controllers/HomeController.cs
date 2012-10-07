using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserService.Messages.Commands;

namespace TCCC13Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return Json("This is the homepage. Go do something!");
        }

		public ActionResult NewUser(string email, string name)
		{
			ServiceBus.Bus.Send(new CreateUserCmd
			{
				Email = email,
				Name = name
			});

			return Json(new { name = name, email = email, status = "User Created" });
		}

		public ActionResult LatestUsers()
		{
			return Json(RecentUsers.Recent.ToArray());
		}
































		protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
		}

    }
}
