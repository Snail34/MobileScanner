using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Models.Arguements
{
    public class CurrentCategoryPageArguements
    {
        public Guid CategoryId { get; set; }

        public bool IsMainCategory { get; set; }
    }
}
