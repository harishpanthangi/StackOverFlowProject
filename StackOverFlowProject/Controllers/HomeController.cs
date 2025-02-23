using StackOverFlowProject.ServiceLayer;
using StackOverFlowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverFlowProject.Controllers
{
    public class HomeController : Controller
    {
        IQuestionsService qs;
        ICategoriesService _categories;
        public HomeController(IQuestionsService qs, ICategoriesService categories)
        {
            this.qs = qs;
            _categories = categories;
        }

        // GET: Home
        public ActionResult Index()
        {
            List<QuestionViewModel>qvm = qs.GetAllQuestions();
            return View(qvm);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Categories()
        {
            List<CategoryViewModel> categories = _categories.GetCategories();
            return View(categories);
        }
        [Route("allquestions")]  //attribute routing
        public ActionResult Questions()
        {
            List<QuestionViewModel> questions = qs.GetAllQuestions();
            return View(questions);
        }
        public ActionResult Search(string str)
        {
            List<QuestionViewModel> questions = qs.GetAllQuestions().Where(temp=>temp.QuestionName.ToLower().Contains(str.ToLower()) || 
            temp.Category.CategoryName.ToLower().Contains(str.ToLower())).ToList();
            ViewBag.str = str;
            return View(questions);
        }
    }
}