using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public bool PublishedOnly { get; set; }
        public string KeyWord { get; set; }
    }
}
