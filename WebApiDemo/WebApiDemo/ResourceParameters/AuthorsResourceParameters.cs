using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.ResourceParameters
{
    public class AuthorsResourceParameters
    {
        public string MainCategory { get; set; }
        public string SearchQuery { get; set; }
    }
}
