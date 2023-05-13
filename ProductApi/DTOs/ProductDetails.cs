﻿namespace ProductApi.DTOs
{
    public class ProductDetails
    {
        public string? ProductName { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Sku { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string? Icon { get; set; }
        public string? Image { get; set; }
    }
}
