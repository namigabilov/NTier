namespace DataAccess.Exceptions.Global
{
    public class DataNullException : Exception
    {
        public DataNullException(string argument, string cultre) : base(message: Modify(argument, cultre))
        {

        }

        static string Modify(string message, string cultre)
        {
            switch (cultre)
            {
                case "az":
                    return $"Göndərilən arqumentə uyğun data tapılmadı {message}";
                case "tr":
                    return $"Gönderilen arqumentə eşdeğer data bulunamadı {message}";
                default:
                    return $"Gönderilen arqumentə eşdeğer data bulunamadı {message}";
            }

        }
    }
}