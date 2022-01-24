using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDataManager.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];
            bool IsValid = decimal.TryParse(rateText, out decimal output);
            if (IsValid == false)
            {
                throw new ConfigurationErrorsException("Tax rate is not set up propery in App Config");
            }
            return output;
        }
    }

}
