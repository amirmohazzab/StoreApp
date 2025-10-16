using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Order
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered(
            int ProductItemId, 
            string ProductName, 
            string ProductBrandName, 
            string ProductTypeName, 
            string PictureUrl)
        {
            this.ProductItemId = ProductItemId;
            this.ProductName = ProductName;
            this.ProductBrandName = ProductBrandName;
            this.ProductTypeName = ProductTypeName;
            this.PictureUrl = PictureUrl;
        }

        public ProductItemOrdered() {}

        public int ProductItemId { get; set; }

        public string ProductName { get; set; }

        public string ProductBrandName { get; set; }

        public string ProductTypeName { get; set; }

        public string PictureUrl { get; set; }
    }
}
