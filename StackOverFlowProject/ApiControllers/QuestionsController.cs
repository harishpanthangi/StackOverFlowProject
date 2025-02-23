using StackOverFlowProject.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StackOverFlowProject.ApiControllers
{
    public class QuestionsController : ApiController
    {
        IAnswersService _answersService;
        public QuestionsController(IAnswersService answersService)
        {
            _answersService = answersService;
        }
        public void UpdateAnswerVotes(int aId, int uId, int value)
        {
            _answersService.UpdateAnswerVotesCount(aId, uId, value);
        }
    }
}
