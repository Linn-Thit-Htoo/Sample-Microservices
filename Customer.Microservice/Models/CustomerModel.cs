using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer.Microservice.Models
{
    [Table("Tbl_Customer")]
    public class CustomerModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Age { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
