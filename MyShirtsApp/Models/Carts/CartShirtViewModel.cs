﻿namespace MyShirtsApp.Models.Carts
{
    public class CartShirtViewModel
    {
        public int ShirtId { get; set; }

        public string UserId { get; set; }

        public string SizeName { get; set; }

        public decimal Price { get; set; }
        
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int Count { get; set; }
    }
}
