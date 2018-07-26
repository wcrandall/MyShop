using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShopCore.Models;
using MyShop.DataAccess.InMemory;
namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository context; 

        public ProductManagerController()
        {
            context = new ProductRepository(); 
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }
        [HttpPost]

        public ActionResult Create(Product product)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
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
                return View(product);
            }

        }

        [HttpPost]
        public ActionResult Edit(Product product, string id)
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
                    ProductToEdit.Category = product.Category;
                    ProductToEdit.Description = product.Description;
                    ProductToEdit.Image = product.Image;
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