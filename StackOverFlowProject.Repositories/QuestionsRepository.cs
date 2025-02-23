using StackOverFlowProject.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowProject.Repositories
{
    public interface IQuestionsRepository
    {
        void InsertQuestion(Question question);
        void UpdateQuestionDetails(Question question);
        void UpdateQuestionVotesCount(int qid, int value);
        void UpdateQuetionAnswersCount(int qid, int value);
        void UpdateQuestionViewCount(int qid, int value);
        void DeleteQuestion(int qid);
        List<Question> GetQuestions();
        Question GetQuestionByQuetionId(int qid);
    }
    public class QuestionsRepository : IQuestionsRepository
    {
        StackOverFlowDatabaseDbContext db;
        public QuestionsRepository()
        {
            db = new StackOverFlowDatabaseDbContext();
        }
        public void DeleteQuestion(int qid)
        {
            Question q = db.Questions.Where(temp=>temp.QuestionId==qid).FirstOrDefault();
            db.Questions.Remove(q);
            db.SaveChanges();
        }

        public Question GetQuestionByQuetionId(int qid)
        {
            Question question = db.Questions.Where(temp => temp.QuestionId == qid).FirstOrDefault();
            if (question != null)
            {
                db.Entry(question).Collection(q => q.Answers).Load(); // Explicitly load Answers
            }
            return question;
        }

        public List<Question> GetQuestions()
        {
            List<Question> questions = db.Questions.ToList();
            return questions;
        }

        public void InsertQuestion(Question question)
        {
            db.Questions.Add(question);
            db.SaveChanges();
        }

        public void UpdateQuestionDetails(Question question)
        {
            Question q = db.Questions.Where(temp=>temp.QuestionId == question.QuestionId).FirstOrDefault();
            if (q != null)
            {
                q.QuestionName = question.QuestionName;
                q.QuestionDateAndTime = question.QuestionDateAndTime;
                q.CategoryId = question.CategoryId;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionViewCount(int qid, int value)
        {
            Question qt = db.Questions.Where(temp => temp.QuestionId == qid).FirstOrDefault();
            if(qt != null)
            {
                qt.ViewsCount += value;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionVotesCount(int qid, int value)
        {
            Question qt = db.Questions.Where(temp => temp.QuestionId == qid).FirstOrDefault();
            if (qt != null)
            {
                qt.VotesCount += value;
                db.SaveChanges();
            }
        }

        public void UpdateQuetionAnswersCount(int qid, int value)
        {
            Question qt = db.Questions.Where(temp => temp.QuestionId == qid).FirstOrDefault();
            if (qt != null)
            {
                qt.AnswersCount += value;
                db.SaveChanges();
            }
        }
    }
}
