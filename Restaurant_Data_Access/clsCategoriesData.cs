using Microsoft.Data.SqlClient;
using Restaurant_Data_Access.DTOs.CategoryDTOs;
using System.Data;
using System.Reflection;
using Util;

namespace Restaurant_Data_Access
{
    public class clsCategoriesData
    {
        private static string? ConnectionString = clsDataSettings._connectionString;

        private static string? SourceName = Assembly.GetExecutingAssembly().GetName().Name;

        public static int? AddNewCategory(CategoriesDTO newCategorie)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int? newID = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewCategory", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryName", newCategorie.CategoryName);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewCategoryID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    newID = (int?)cmd.Parameters["@NewCategoryID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewCategory: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static async Task<int?> AddNewCategoryAsync(CategoriesDTO newCategorie)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int? newID = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewCategory", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryName", newCategorie.CategoryName);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewCategoryID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    newID = (int?)cmd.Parameters["@NewCategoryID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewCategory: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static CategoriesDTO? GetCategoryByID(int? id)
        {
            if (id < 0) return null; // check PersonID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetCategoryByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", id); // CategoryID parameter.

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new CategoriesDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                reader.GetString(reader.GetOrdinal("CategoryName"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetCategoryByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static async Task<CategoriesDTO?> GetCategoryByIDAsync(int? id)
        {
            if (id < 0) return null; // check CategoryID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetCategoryByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", id); // CategoryID parameter.

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        if (reader.Read())
                        {
                            return new CategoriesDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                reader.GetString(reader.GetOrdinal("CategoryName"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetCategoryByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static bool UpdateCategory(CategoriesDTO uCategorie)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", uCategorie.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", uCategorie.CategoryName);

                try
                {
                    conn.Open();

                    IsUpdated = cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateCategory: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static async Task<bool> UpdateCategoryAsync(CategoriesDTO uCategorie)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", uCategorie.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", uCategorie.CategoryName);

                try
                {
                    await conn.OpenAsync();

                    IsUpdated = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateCategory: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static bool IsCategoryExists(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsCategoryExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", id);

                SqlParameter returnValue = new SqlParameter
                {
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(returnValue);

                try
                {
                    conn.Open();

                    cmd.ExecuteScalar();

                    IsExists = ((int?)returnValue.Value == 1);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsCategorieExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static async Task<bool> IsCategoryExistsAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsCategoryExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", id);

                SqlParameter returnValue = new SqlParameter
                {
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(returnValue);

                try
                {
                    await conn.OpenAsync();

                    await cmd.ExecuteScalarAsync();

                    IsExists = ((int?)returnValue.Value == 1);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsCategorieExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static bool DeleteCategory(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", id);

                try
                {
                    conn.Open();

                    IsDeleted = (cmd.ExecuteNonQuery() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteCategory: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static async Task<bool> DeleteCategoryAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", id);

                try
                {
                    await conn.OpenAsync();

                    IsDeleted = (await cmd.ExecuteNonQueryAsync() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteCategory: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static List<CategoriesDTO?> GetAllCategories()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var CategoriesList = new List<CategoriesDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllCategories", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoriesList.Add(new CategoriesDTO(

                                 reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                 reader.GetString(reader.GetOrdinal("CategoryName"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllCategories: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return CategoriesList;
        }

        public static async Task<List<CategoriesDTO?>> GetAllCategoriesAsync()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var CategoriesList = new List<CategoriesDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllCategories", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoriesList.Add(new CategoriesDTO(
                                 reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                 reader.GetString(reader.GetOrdinal("CategoryName"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllCategories: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return CategoriesList;
        }

    }
}
