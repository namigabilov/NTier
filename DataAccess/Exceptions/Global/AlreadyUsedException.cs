namespace DataAccess.Exceptions.Global
{
    public class AlreadyUsedException : Exception
    {
        public AlreadyUsedException(string entity) : base(message: Modify(entity))
        {
        }
        private static string Modify(string entity)
        {
            return $"{entity} already used in context";
        }
    }
}