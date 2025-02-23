using StackOverFlowProject.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowProject.Repositories
{
    public interface IUsersRepository
    {
        void InsertUser(User u);
        void UpdateUserDetails(User u);
        void ChangePassword(User u);
        void DeleteUser(int uid);
        List<User> GetUsers();
        List<User> GetUsersByEmailAndPassword(string Email, string Password);
        List<User> GetUsersByEmail(string Email);
        List<User> GetUsersByUserId(int UserId);
        int GetUserId();
    }
    public class UsersRepository:IUsersRepository
    {
        StackOverFlowDatabaseDbContext db;
        public UsersRepository()
        {
            db = new StackOverFlowDatabaseDbContext();
        }
        public void InsertUser(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();
        }
        public void UpdateUserDetails(User u)
        {
            User us = db.Users.Where(temp=>temp.UserId == u.UserId).FirstOrDefault();
            if (us != null)
            {
                us.Mobile = u.Mobile;
                us.Name = u.Name;
                db.SaveChanges();
            }
        }
        public void ChangePassword(User u)
        {
            User us = db.Users.Where(temp => temp.UserId == u.UserId).FirstOrDefault();
            if (us != null)
            {
                us.PasswordHash = u.PasswordHash;
                db.SaveChanges();
            }
        }
        public void DeleteUser(int uid)
        {
            User us = db.Users.Where(temp => temp.UserId == uid).FirstOrDefault();
            if (us != null)
            {
                db.Users.Remove(us);
                db.SaveChanges();
            }
        }
        public List<User> GetUsers()
        {
            List<User> users = db.Users.Where(temp=>temp.IsAdmin==false).OrderBy(temp=>temp.Name).ToList();
            return users;
        }
        public List<User>GetUsersByEmailAndPassword(string Email, string Password)
        {
            List<User> users = db.Users.Where(temp => temp.Email == Email && temp.PasswordHash==Password).ToList();
            return users;
        }
        public List<User> GetUsersByEmail(string Email)
        {
            List<User> users = db.Users.Where(temp => temp.Email == Email).ToList();
            return users;
        }
        public List<User> GetUsersByUserId(int UserId)
        {
            List<User> users = db.Users.Where(temp => temp.UserId == UserId).ToList();
            return users;
        }
        public int GetUserId()
        {
            int userId = db.Users.Select(temp => temp.UserId).Max();
            return userId;
        }
    }
}
