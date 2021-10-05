using System;
using System.Collections.Generic;

#nullable disable

namespace e_commerce.Models
{
    public partial class TblProduct
    {
        public TblProduct()
        {
            TblOrders = new HashSet<TblOrder>();
        }

        public int Pid { get; set; }
        public string Pname { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public string Sname { get; set; }

        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
