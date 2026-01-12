using System;

namespace Doanweb.Models
{
    public class CartItem
    {
      
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = "Sản phẩm";


        public string Image { get; set; } = "default.png";

        public int Quantity { get; set; } = 1;


        public decimal Price { get; set; }

        public decimal Total => Price * Quantity;

        public CartItem() { }

        public CartItem(int cartId, int productId, string productName, string image, int quantity, decimal price)
        {
            CartId = cartId;
            ProductId = productId;
            ProductName = productName;
            Image = image;
            Quantity = quantity;
            Price = price;
        }
    }
}