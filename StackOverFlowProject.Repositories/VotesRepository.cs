using StackOverFlowProject.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowProject.Repositories
{
    public interface IVotesRepository
    {
        void UpdateVote(int ansId, int uId, int value);
    }
    public class VotesRepository:IVotesRepository
    {
        StackOverFlowDatabaseDbContext db;
        public VotesRepository() 
        { 
            db = new StackOverFlowDatabaseDbContext();
        }
        public void UpdateVote(int ansId, int uId,int value)
        {
            int updateValue;
            if (value > 0) updateValue=1;
            else if (value < 0) updateValue = -1;
            else updateValue = 0;
            Vote vote = db.Votes.Where(temp=>temp.AnswerId == ansId && temp.UserId==uId).FirstOrDefault();
            if(vote != null)
            {
                vote.VoteValue = updateValue;
            }
            else
            {
                Vote newVote = new Vote() { AnswerId = ansId, UserId=uId,VoteValue=value};
                db.Votes.Add(newVote);
            }
            db.SaveChanges();
        }
    }
}
