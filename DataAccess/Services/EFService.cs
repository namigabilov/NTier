namespace DataAccess.Services
{
    public static class EFService
    {
        public static string MapUrl(double latitude, double longitude)
        {
            return $"https://www.google.com/maps?q={latitude},{longitude}";
        }
    }
}