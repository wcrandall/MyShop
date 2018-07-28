using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShopCore.Models;
using MyShopCore.Contracts;
namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache Cache = MemoryCache.Default;
        List<T> Items;
        string ClassName; 

        public InMemoryRepository()
        {
            ClassName = typeof(T).Name;
            Items = Cache[ClassName] as List<T>; 
            if (Items == null)
            {
                Items = new List<T>();
            }

        }
        public void commit()
        {
            Cache[ClassName] = Items; 
        }

        public void Insert(T t)
        {
            Items.Add(t); 

        }
        public void update(T t)
        {
            T TToUpdate = Items.Find(i => i.id == t.id); 
            if(TToUpdate != null)
            {
                TToUpdate = t; 
            }
            else
            {
                throw new Exception(ClassName + "Not Found"); 
                    
            }
        }
        public void Commit()
        {
            Cache[ClassName] = Items; 
        }
        public T Find (string Id)
        {
            T t = Items.Find(i => i.id == Id);
            if (t!=null)
            {
                return t;
            }
            else
            {
                throw new Exception(ClassName + "Not Found");
            }
        }
        public IQueryable<T> Collection()
        {
            return Items.AsQueryable();
        }
        public void Delete(string Id)
        {
            T TToDelete = Items.Find(i => i.id == Id);
            if (TToDelete != null)
            {
                Items.Remove(TToDelete);
            }
            else
            {
                throw new Exception(ClassName + "Not Found");

            }
        }
    }
}
