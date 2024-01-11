using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Microservice.Models
{
    [Table("Tbl_Product")]
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime Create_Date { get; set; }
        public bool IsDeleted { get; set; }
    }
}
