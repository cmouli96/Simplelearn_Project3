using System;
using System.Collections.Generic;

#nullable disable

namespace e_commerce.Models
{
    public partial class TblOrder
    {
        public string Uname { get; set; }
        public int Pid { get; set; }
        public string Pname { get; set; }
        public string Sname { get; set; }
        public int Qty { get; set; }
        public int Cost { get; set; }

        public virtual TblProduct PidNavigation { get; set; }
    }
}
