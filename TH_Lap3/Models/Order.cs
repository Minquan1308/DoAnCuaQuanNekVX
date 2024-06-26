﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace TH_Lap3.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        // Thuộc tính mới để lưu trữ tổng số lượng đã bán của sản phẩm trong đơn hàng
      //  [NotMapped] // Đánh dấu thuộc tính này không được ánh xạ vào cột trong cơ sở dữ liệu
        public int TotalQuantitySold { get; set; }
    }
}
