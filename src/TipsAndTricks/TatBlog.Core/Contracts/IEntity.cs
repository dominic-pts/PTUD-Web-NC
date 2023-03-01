using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.Contracts;

    public interface IEntity
    {
        int Id { get; set; }
    }
   
