using System;

namespace TaskManager.DataLayer.Tasks
{
    public class GuidFactory : IGuidFactory
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }

        public Guid Parse(string literal)
        {
            return Guid.Parse(literal);
        }
    }
}