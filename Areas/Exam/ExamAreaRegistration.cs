using System.Web.Mvc;

namespace Question_Bank.Areas.Exam
{
    public class ExamAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Exam";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Exam_default",
                "Exam/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}