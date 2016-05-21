using System.Collections.Generic;

namespace OrderTaker.SharedObjects
{
    public class Order
    {
        public List<OrderItem> OrderItems { get; set; }

        public Person Customer { get; set; }

        public int OrderDiscount { get; set; }
    }
}
