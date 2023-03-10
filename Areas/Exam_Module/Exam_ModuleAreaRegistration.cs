using System.Web.Mvc;

namespace Question_Bank.Areas.Exam_Module
{
    public class Exam_ModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Exam_Module";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Exam_Module_default",
                "Exam_Module/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}