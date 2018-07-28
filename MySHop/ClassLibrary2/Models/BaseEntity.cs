using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopCore.Models
{
     public abstract  class BaseEntity
    {
        public string id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public BaseEntity()
        {
            this.id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }
    }
}
