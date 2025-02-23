using StackOverFlowProject.CustomFilters;
using StackOverFlowProject.ServiceLayer;
using StackOverFlowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverFlowProject.Controllers
{
    public class QuestionsController : Controller
    {
        IQuestionsService _questionsService;
        IAnswersService _answersService;
        ICategoriesService _categoriesService;
        public QuestionsController(IQuestionsService questionsService, IAnswersService answersService, ICategoriesService categoriesService) 
        {
            _answersService = answersService;
            _questionsService = questionsService;
            _categoriesService = categoriesService;
        }
        // GET: Questions
        public ActionResult View(int Id)
        {
            _questionsService.UpdateQuestionViewsCount(Id, 1);
            int uId = Convert.ToInt32(Session["CurrentUserId"]);
            QuestionViewModel qvm = _questionsService.GetQuestionByQuestionId(Id, uId);
            return View(qvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult AddAnswer(NewAnsweViewModel avm)
        {
            avm.UserId = Convert.ToInt32(Session["CurrentUserId"]);
            avm.AnswerDateAndTime = DateTime.Now;
            avm.VotesCount = 0;
            if(ModelState.IsValid)
            {
                _answersService.InsertAnswer(avm);
                return RedirectToAction("View","Questions", new {Id = avm.QuestionId});
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Date");
                QuestionViewModel qvm = _questionsService.GetQuestionByQuestionId(avm.QuestionId, avm.UserId);
                return View("View", qvm);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult EditAnswer(EditAnswerViewModel avm)
        {
            avm.UserId = Convert.ToInt32(Session["CurrentUserId"]);
            avm.AnswerDateAndTime= DateTime.Now;
            if (ModelState.IsValid)
            {
                _answersService.UpdateAnswer(avm);
                return RedirectToAction("View", new { Id = avm.QuestionId });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return RedirectToAction("View", new { Id = avm.QuestionId });
            }
        }
        [UserAuthorizationFilter]
        public ActionResult Create()
        {
            List<CategoryViewModel> cvm = _categoriesService.GetCategories();
            ViewBag.categories = cvm;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult Create(NewQuestionViewModel qvm)
        {
            if (ModelState.IsValid)
            {
                qvm.AnswerCount = 0;
                qvm.VotesCount = 0;
                qvm.ViewsCount = 0;
                qvm.QuestionDateAndTime = DateTime.Now;
                qvm.UserId = Convert.ToInt32(Session["CurrentUserId"]);
                _questionsService.InsertQuestion(qvm);
                return RedirectToAction("Questions", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View();
            }
        } 
    }
}