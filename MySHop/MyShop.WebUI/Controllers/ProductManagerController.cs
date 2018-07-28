using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShopCore.Models;
using MyShop.DataAccess.InMemory;
using MyShopCore.ViewModels;
using MyShopCore.Contracts;
using System.IO;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> ProductCategories; 
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            ProductCategories = productCategoryContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel ViewModel = new ProductManagerViewModel();
            ViewModel.Product = new Product();
            ViewModel.ProductCategories = ProductCategories.Collection();
            return View(ViewModel);
        }
        [HttpPost]

        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    product.Image = product.id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
                context.Insert(product);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {

                ProductManagerViewModel ViewModel = new ProductManagerViewModel();
                ViewModel.Product = product;
                ViewModel.ProductCategories = ProductCategories.Collection();
                return View(ViewModel);
            }

        }

        [HttpPost]
        public ActionResult Edit(Product product, string id, HttpPostedFileBase file)
        {
            Product ProductToEdit = context.Find(id);

            if (ProductToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if(!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    if (file != null)
                    {
                        ProductToEdit.Image = product.id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//ProductImages//") + ProductToEdit.Image);
                    }
                    ProductToEdit.Category = product.Category;
                    ProductToEdit.Description = product.Description;
                 
                    ProductToEdit.Name = product.Name;
                    ProductToEdit.Price = product.Price;

                    context.Commit();
                    return RedirectToAction("Index");

                }
            }
        }
        public ActionResult Delete(string Id)
        {
            Product ProductToDelete = context.Find(Id); 
            if (ProductToDelete == null)
            {
                return HttpNotFound(); 
            }
            else
            {
                return View(ProductToDelete); 
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product ProductToDelete = context.Find(Id);
            if (ProductToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}