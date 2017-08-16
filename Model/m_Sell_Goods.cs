using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiMiao.Model
{
    public class m_Sell_Goods
    {
        public string GoodsID { set; get; }
        public int? OrgID { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
        public int? Amount { set; get; }
        public int? Balance { set; get; }
        public int? Frozen { set; get; }
        public decimal? Price { set; get; }
        public DateTime? CreateTime { set; get; }
        public int? OrderIndex { set; get; }
        public int? StateCode { set; get; }
        public int isSell { set; get; }
    }
}
