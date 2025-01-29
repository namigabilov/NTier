using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Exceptions.Global
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity) : base(message: Modify(entity))
        {
        }

        private static string Modify(string entity)
        {
            return $"{entity} Not found in context";
        }
    }
}