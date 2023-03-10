using Question_Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Question_Bank.Helper
{
    public partial class BaseController : Controller
    {
        Question_Bank_2022Entities db = new Question_Bank_2022Entities();
        Common common = new Common();


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                Login_Model login_model = common.Get_Login_Details();

                if (filterContext.ActionDescriptor.ActionName == "Home")
                {
                    filterContext.Result = View("Home");
                }
                else if (filterContext.ActionDescriptor.ActionName == "Login" && filterContext.ActionParameters.Count == 0)
                {
                    filterContext.Result = View("Login");
                }
                else if (login_model == null && filterContext.ActionParameters.Count == 0 && filterContext.ActionDescriptor.ActionName != "Login" && filterContext.ActionDescriptor.ActionName != "Home")
                {
                    filterContext.Result = new RedirectResult("https://localhost:44336/");/*View("Login");*/
                }
            }
            catch (Exception)
            {
                filterContext.Result = View("Home");
            }
        }
    }
}
