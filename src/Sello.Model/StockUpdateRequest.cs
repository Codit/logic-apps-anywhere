using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sello.Model
{
    public class StockUpdateRequest
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }
}
