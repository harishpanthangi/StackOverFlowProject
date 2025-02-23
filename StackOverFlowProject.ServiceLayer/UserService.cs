using AutoMapper;
using StackOverFlowProject.DomainModels;
using StackOverFlowProject.Repositories;
using StackOverFlowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowProject.ServiceLayer
{
    public interface IUserService
    {
        int InsertUser(RegisterViewModel rvm);
        void UpdateUserDetails(EditUserDetailsViewModel eudvm);
        void UpdateUserPassword(EditUserPasswordViewModel eupvm);
        void DeleteUser(int id);
        List<UserViewModel>GetUsers();
        UserViewModel GetUsersByEmailAndPassword(string Email, string Password);
        UserViewModel GetUsersByEmail(string Email);
        UserViewModel GetUserByUserId(int userId);
    }
    public class UserService:IUserService
    {
        IUsersRepository _usersRepository;
        public UserService()
        {
            _usersRepository = new UsersRepository();
        }

        public void DeleteUser(int id)
        {
            _usersRepository.DeleteUser(id);
        }

        public UserViewModel GetUserByUserId(int userId)
        {
            User user = _usersRepository.GetUsersByUserId(userId).FirstOrDefault();
            UserViewModel uvm = null;
            if (user != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(user);
            }
            return uvm;
        }

        public List<UserViewModel> GetUsers()
        {
            List<User> users = _usersRepository.GetUsers();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List<UserViewModel> uvm = mapper.Map<List<User>, List<UserViewModel>>(users);
            return uvm;
        }

        public UserViewModel GetUsersByEmail(string Email)
        {
            User user = _usersRepository.GetUsersByEmail(Email).FirstOrDefault();
            UserViewModel uvm = null;
            if (user != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(user);
            }
            return uvm;
        }

        public UserViewModel GetUsersByEmailAndPassword(string Email, string Password)
        {
            User users = _usersRepository.GetUsersByEmailAndPassword(Email, SHA256HashGenerator.GenerateHash(Password)).FirstOrDefault();
            UserViewModel uvm=null;
            if (users != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(users);
            }
            return uvm;
        }

        public int InsertUser(RegisterViewModel rvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<RegisterViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User user = mapper.Map<RegisterViewModel,User>(rvm);
            user.PasswordHash=SHA256HashGenerator.GenerateHash(rvm.Password);
            _usersRepository.InsertUser(user);
            int uId = _usersRepository.GetUserId();
            return uId;
        }

        public void UpdateUserDetails(EditUserDetailsViewModel eudvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditUserDetailsViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User user = mapper.Map<EditUserDetailsViewModel, User>(eudvm);
            _usersRepository.UpdateUserDetails(user);
        }

        public void UpdateUserPassword(EditUserPasswordViewModel eupvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditUserPasswordViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User user = mapper.Map<EditUserPasswordViewModel, User>(eupvm);
            user.PasswordHash = SHA256HashGenerator.GenerateHash(eupvm.Password);
            _usersRepository.ChangePassword(user);
        }
    }
}
