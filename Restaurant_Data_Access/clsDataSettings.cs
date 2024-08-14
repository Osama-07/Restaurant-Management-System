using Microsoft.Extensions.Configuration;

namespace Restaurant_Data_Access
{
    public class clsDataSettings
    {
        public static string? _connectionString;

        public static void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
