using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.DataLayer
{
    public class SchemaAttribute : TableAttribute
    {
        public SchemaAttribute(string schema) : base(nameof(Schema))
        {
            Schema = schema;
        }
    }
}
