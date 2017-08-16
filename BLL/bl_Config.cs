using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiMiao.BLL
{
    public class bl_Config
    {
        public static List<string> noFrozen
        {
            get
            {
                string str = System.Configuration.ConfigurationManager.AppSettings["NOFROZEN"];
                return str?.Split(',').ToList<string>();
            }
        }
        public static string YueBingGoodsID
        {
            get
            {
                var str = System.Configuration.ConfigurationManager.AppSettings["YueBingID"];
                if (string.IsNullOrEmpty(str))
                {
                    str = "16111c0f-7f4a-11e7-88e6-000c2943fa30";
                }
                return str;
            }
        }
    }
}
