using StackOverFlowProject.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowProject.Repositories
{
    public interface ICategoriesRepository
    {
        void InsertCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int cid);
        List<Category> GetCategories();
        Category GetCategoryByCategoryId(int CategoryId);
    }
    public class CategoriesRepository : ICategoriesRepository
    {
        StackOverFlowDatabaseDbContext db;
        public CategoriesRepository()
        {
            db = new StackOverFlowDatabaseDbContext();
        }
        public void DeleteCategory(int cid)
        {
            Category category = db.Categories.Where(temp=> temp.CategoryId == cid).FirstOrDefault();
            db.Categories.Remove(category);
            db.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = db.Categories.ToList();
            return categories;
        }

        public Category GetCategoryByCategoryId(int CategoryId)
        {
            Category category = db.Categories.Where(temp=>temp.CategoryId == CategoryId).FirstOrDefault();
            return category;
        }

        public void InsertCategory(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            Category ct = db.Categories.Where(temp=>temp.CategoryId==category.CategoryId).FirstOrDefault();
            if (ct != null)
            {
                ct.CategoryName = category.CategoryName;
                db.SaveChanges();
            }
        }
    }
}
