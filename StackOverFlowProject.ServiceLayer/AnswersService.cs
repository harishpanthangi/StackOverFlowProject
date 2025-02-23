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
    public interface IAnswersService
    {
        void InsertAnswer(NewAnsweViewModel avm);
        void UpdateAnswer(EditAnswerViewModel avm);
        void UpdateAnswerVotesCount(int Id, int uId, int value);
        void DeleteAnswer(int Id);
        List<AnswerViewModel> GetAnswersByQuestionId(int qId);
        AnswerViewModel GetAnswerByAnswerId(int aId);

    }
    public class AnswersService: IAnswersService
    {
        IAnswersRepository _answersRepository;
        public AnswersService() 
        { 
            _answersRepository = new AnswersRepository();
        }

        public void DeleteAnswer(int Id)
        {
            _answersRepository.DeleteAnswer(Id);
        }

        public AnswerViewModel GetAnswerByAnswerId(int aId)
        {
            Answer a = _answersRepository.GetAnswerByAnswerId(aId).FirstOrDefault();
            AnswerViewModel avm = null;
            if (a != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Answer, AnswerViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                avm = mapper.Map<Answer, AnswerViewModel>(a);
            }
            
            return avm;
        }

        public List<AnswerViewModel> GetAnswersByQuestionId(int qId)
        {
            List<Answer> a = _answersRepository.GetAnswersByQuestionId(qId);
            List<AnswerViewModel> avm = null;
            if (a != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Answer, AnswerViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                avm = mapper.Map<List<Answer>, List<AnswerViewModel>>(a);
            }

            return avm;
        }

        public void InsertAnswer(NewAnsweViewModel avm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<NewAnsweViewModel, Answer>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Answer ans = mapper.Map<NewAnsweViewModel, Answer>(avm);
            _answersRepository.InsertAnswer(ans);
        }

        public void UpdateAnswer(EditAnswerViewModel avm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditAnswerViewModel, Answer>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Answer ans = mapper.Map<EditAnswerViewModel, Answer>(avm);
            _answersRepository.UpdateAnswer(ans);
        }

        public void UpdateAnswerVotesCount(int Id, int uId, int value)
        {
            _answersRepository.UpdateAnswerVotesCount(Id, uId, value);
        }
    }
}
