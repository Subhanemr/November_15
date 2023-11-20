﻿namespace _15_11_23.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CountId { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductTag> ProductTags { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductColor> ProductColors { get; set; }


    }
}