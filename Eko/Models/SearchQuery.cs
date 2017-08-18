using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class SearchQuery
    {
        public string Query = null;
        public string Brand = null;
        public string Condition = null;
        public int? YearMin = null;
        public int? YearMax = null;
        public decimal? PriceMin = null;
        public decimal? PriceMax = null;
        public string Sort = null;

        public int? CategoryID = null;
    }
}
