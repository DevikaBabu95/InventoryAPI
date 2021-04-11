using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Product
    {

        public int ItemId         { get; set;}
        public string Name           { get; set;}
        public string Description    { get; set;}
        public decimal Price { get; set; }
        public string Action { get; set; }
    }
    public class ProductDetails
    {
        public string Output { get; set; }
        public List<Product> ProInfo { get; set; }
    }
}