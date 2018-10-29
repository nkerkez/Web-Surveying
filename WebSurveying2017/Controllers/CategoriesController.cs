using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using WebSurveying2017.BindingModels;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.ViewModels;
using WebSurveyingS2017.Service.Service;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        private ICategoryService categoryService;
        private IUnitOfWork unitOfWork;
        

        public CategoriesController(IUnitOfWork unitOfWork, ICategoryService categoryService)
        {
            this.unitOfWork = unitOfWork;
            this.categoryService = categoryService;
        }

        // GET: api/Categories
        [AllowAnonymous]
        [Route("sub/{id}")]
        [HttpGet]
        public IHttpActionResult GetSubCategories(int id)
        {
            return Ok(categoryService.GetAllSub(id));
        }
        
        [AllowAnonymous]
        public IHttpActionResult GetCategories()
        {
            var categories = categoryService.GetRoots().ToList();
            try
            {
                var categoriesVM = AutoMapper.Mapper.Map<List<Category>, List<CategoriesViewModel>>(categories);
                return Ok(categoriesVM);
            }catch(Exception)
            {
                return BadRequest();
            }
        }
        
        // GET: api/Categories/5
        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = categoryService.GetManyAsNoTracking(c => c.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }
            try
            {
                CategoryViewModel categoryVM = AutoMapper.Mapper.Map<Category, CategoryViewModel>(category);
                return Ok(categoryVM);
            }
            catch(Exception)
            {
                return BadRequest();
            }
     
        }

        [Authorize(Roles = "Admin, Moderator")]
        // PUT: api/Categories/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult PutCategory(int id, CategoryBindingModel categoryBM)
        {

            try
            {
                var subs = categoryService.GetAllSub(id);
                if (categoryBM.CategoryId != null && (categoryBM.CategoryId == categoryBM.Id
                    || subs.Contains((int)categoryBM.CategoryId)))
                {
                    ModelState.AddModelError("", "Pogrešno postavljena nadkategorija");
                    return BadRequest(ModelState);
                }
                var category = AutoMapper.Mapper.Map<CategoryBindingModel, Category>(categoryBM);
                categoryService.Update(category);
                unitOfWork.Commit();
            }
            catch (Exception)
            {

                return BadRequest();
            }
            return Ok();
        }
        
        
        [Authorize(Roles = "Admin, Moderator")]
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(CategoryBindingModel categoryBM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
               
                Category category = AutoMapper.Mapper.Map<CategoryBindingModel, Category>(categoryBM);
                categoryService.Create(category);
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Postoji kategorija sa unetim imenom");
                return BadRequest(ModelState);
            }
            return Ok();
        }
        
        [Authorize(Roles = "Admin,Moderator")]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteCategory(int id)
        {
            
            try{
                
                categoryService.DeleteCategory(id);
                
                unitOfWork.Commit();
                 return Ok();
            }catch(Exception)
            {
                  return BadRequest(ModelState);
            }
        }
        

    }
}