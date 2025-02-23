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
    public interface ICategoriesService
    {
        void InsertCategory(CategoryViewModel cvm);
        void UpdateCategory(CategoryViewModel cvm);
        void DeleteCategory(int cId);
        List<CategoryViewModel> GetCategories();
        CategoryViewModel GetCategoryByCategoryId(int cId);
    }
    public class CategoriesService: ICategoriesService
    {
        ICategoriesRepository _categories;
        public CategoriesService()
        {
            _categories = new CategoriesRepository();
        }

        public void DeleteCategory(int cId)
        {
           _categories.DeleteCategory(cId);
        }

        public List<CategoryViewModel> GetCategories()
        {
            List<Category> c = _categories.GetCategories();
            List<CategoryViewModel> cvm = null;
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            cvm = mapper.Map<List<Category>, List<CategoryViewModel>>(c);
            return cvm;
        }

        public CategoryViewModel GetCategoryByCategoryId(int cId)
        {
            Category  c= _categories.GetCategoryByCategoryId(cId);
            CategoryViewModel cvm = null;
            if (c != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                cvm = mapper.Map<Category, CategoryViewModel>(c);
            }
            return cvm;
        }

        public void InsertCategory(CategoryViewModel cvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Category category = mapper.Map<CategoryViewModel, Category>(cvm);
            _categories.InsertCategory(category);
        }

        public void UpdateCategory(CategoryViewModel cvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Category category = mapper.Map<CategoryViewModel, Category>(cvm);
            _categories.UpdateCategory(category);
        }
    }
}
