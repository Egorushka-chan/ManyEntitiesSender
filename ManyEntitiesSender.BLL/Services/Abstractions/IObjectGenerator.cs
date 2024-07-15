using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManyEntitiesSender.DAL.Entities;

namespace ManyEntitiesSender.DAL.Interfaces
{
    public interface IObjectGenerator
    {
        Task EnsurePackageCount();
    }
}
