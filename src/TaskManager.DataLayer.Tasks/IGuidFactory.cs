using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.DataLayer.Tasks
{
    public interface IGuidFactory
    {
        Guid Create();
        Guid Parse(string literal);
    }
}
