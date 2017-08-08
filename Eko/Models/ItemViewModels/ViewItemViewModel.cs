using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models.ItemViewModels
{
    public class ViewItemViewModel
    {
        public Item Item { get; set; }

        public List<Guid> ImageIds { get; set; }

        public int Watchers { get; set; }
    }
}
