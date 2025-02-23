using AutoMapper;
using StackOverFlowProject.DomainModels;
using StackOverFlowProject.Repositories;
using StackOverFlowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowProject.ServiceLayer
{
    public interface IQuestionsService
    {
        void InsertQuestion(NewQuestionViewModel qvm);
        void UpdateQuestionDetails(EditQuestionViewModel eqvm);
        void UpdateQuestionVotesCount(int qId, int value);
        void UpdateQuestionAnswersCount(int qId, int value);
        void UpdateQuestionViewsCount(int qId, int value);
        void DeleteQuestion(int qId);
        List<QuestionViewModel> GetAllQuestions();
        QuestionViewModel GetQuestionByQuestionId(int qId, int UserId);
    }
    public class QuestionsService: IQuestionsService
    {
        IQuestionsRepository _questionsRepository;
        public QuestionsService()
        {
            _questionsRepository = new QuestionsRepository();
        }

        public void DeleteQuestion(int qId)
        {
            _questionsRepository.DeleteQuestion(qId);
        }

        public List<QuestionViewModel> GetAllQuestions()
        {
            List<Question> q = _questionsRepository.GetQuestions();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Question, QuestionViewModel>();
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<Category, CategoryViewModel>();
                cfg.CreateMap<Answer, AnswerViewModel>();
                cfg.CreateMap<Vote, VoteViewModel>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            List<QuestionViewModel> qvm = mapper.Map<List<Question>, List<QuestionViewModel>>(q);
            return qvm;
        }

        public QuestionViewModel GetQuestionByQuestionId(int qId, int UserId)
        {
            Question q = _questionsRepository.GetQuestionByQuetionId(qId);
            QuestionViewModel qvm = null;
            if (q != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Question, QuestionViewModel>();
                    cfg.CreateMap<User, UserViewModel>();
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.CreateMap<Answer, AnswerViewModel>();
                    cfg.CreateMap<Vote, VoteViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                qvm = mapper.Map<Question, QuestionViewModel>(q);
                if(qvm.Answers != null)
                {
                    foreach (var item in qvm.Answers)
                    {
                        item.CurrentUserVoteType = 0;
                        VoteViewModel vote = item.Votes.Where(temp => temp.UserId == UserId).FirstOrDefault();
                        if (vote != null)
                        {
                            item.CurrentUserVoteType = vote.VoteValue;
                        }
                    }
                }
                
            }
            return qvm;
        }

        public void InsertQuestion(NewQuestionViewModel qvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<NewQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question q = mapper.Map<NewQuestionViewModel, Question>(qvm);
            _questionsRepository.InsertQuestion(q);
        }

        public void UpdateQuestionAnswersCount(int qId, int value)
        {
            _questionsRepository.UpdateQuetionAnswersCount(qId, value);
        }

        public void UpdateQuestionDetails(EditQuestionViewModel eqvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question q = mapper.Map<EditQuestionViewModel, Question>(eqvm);
            _questionsRepository.UpdateQuestionDetails(q);
        }

        public void UpdateQuestionViewsCount(int qId, int value)
        {
            _questionsRepository.UpdateQuestionViewCount(qId, value);
        }

        public void UpdateQuestionVotesCount(int qId, int value)
        {
            _questionsRepository.UpdateQuestionVotesCount(qId, value);
        }
    }
}
