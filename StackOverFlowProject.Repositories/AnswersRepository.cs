using StackOverFlowProject.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowProject.Repositories
{
    public interface IAnswersRepository
    {
        void InsertAnswer(Answer ans);
        void UpdateAnswer(Answer ans);
        void UpdateAnswerVotesCount(int ansId, int uId, int value);
        void DeleteAnswer(int ansId);
        List<Answer>GetAnswersByQuestionId(int questionId);
        List<Answer>GetAnswerByAnswerId(int answerId);
        List<Answer> GetAllAnswers();
    }
    public class AnswersRepository: IAnswersRepository
    {
        StackOverFlowDatabaseDbContext db;
        IQuestionsRepository qr;
        IVotesRepository vr;
        public AnswersRepository()
        {
            db = new StackOverFlowDatabaseDbContext();
            qr = new QuestionsRepository();
            vr = new VotesRepository();
        }

        public void DeleteAnswer(int ansId)
        {
            Answer ans = db.Answers.Where(temp=>temp.AnswerId==ansId).FirstOrDefault();
            db.Answers.Remove(ans);
            db.SaveChanges();
            qr.UpdateQuetionAnswersCount(ans.QuestionId, -1);
        }

        public List<Answer> GetAllAnswers()
        {
            List<Answer> ans = db.Answers.ToList();
            return ans;
        }

        public List<Answer> GetAnswerByAnswerId(int answerId)
        {
            List<Answer> ans = db.Answers.Where(temp => temp.AnswerId == answerId).ToList();
            return ans;
        }

        public List<Answer> GetAnswersByQuestionId(int questionId)
        {
            List<Answer> ans = db.Answers.Where(temp => temp.QuestionId == questionId).OrderByDescending(temp => temp.AnswerDateAndTime).ToList();
            return ans;
        }

        public void InsertAnswer(Answer ans)
        {
            db.Answers.Add(ans);
            db.SaveChanges();
            qr.UpdateQuetionAnswersCount(ans.QuestionId, 1);
        }

        public void UpdateAnswer(Answer ans)
        {
            Answer answer = db.Answers.Where(temp => temp.AnswerId == ans.AnswerId).FirstOrDefault();
            if (answer != null)
            {
                answer.AnswerText = ans.AnswerText;
                answer.AnswerDateAndTime = ans.AnswerDateAndTime;
                db.SaveChanges();
            }
        }

        public void UpdateAnswerVotesCount(int ansId, int uId, int value)
        {
            Answer ans = db.Answers.Where(temp => temp.AnswerId == ansId).FirstOrDefault();
            if (ans != null)
            {
                ans.VotesCount += value;
                db.SaveChanges();
                qr.UpdateQuestionVotesCount(ans.QuestionId, value);
                vr.UpdateVote(ansId, uId,value);
            }
        }
    }
}
