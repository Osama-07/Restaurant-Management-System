using Microsoft.Data.SqlClient;
using Restaurant_Data_Access.DTOs.MenuItemsDTOs;
using System.Data;
using System.Reflection;
using Util;

namespace Restaurant_Data_Access
{
    public class clsMenuItemsData
    {
        private static string? ConnectionString = clsDataSettings._connectionString;

        private static string? SourceName = Assembly.GetExecutingAssembly().GetName().Name;

        public static int? AddNewMenuItem(MenuItemsDTO newMenuItem)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int? newID = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewMenuItem", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", newMenuItem.Name);
                cmd.Parameters.AddWithValue("@Price", newMenuItem.Price);
                if (string.IsNullOrEmpty(newMenuItem.Description)) // allow null
                    cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Description", newMenuItem.Description);
                cmd.Parameters.AddWithValue("@CategoryID", newMenuItem.CategoryID);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewMenuItemID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    newID = (int?)cmd.Parameters["@NewMenuItemID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewMenuItem: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static async Task<int?> AddNewMenuItemAsync(MenuItemsDTO newMenuItem)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int? newID = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewMenuItem", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", newMenuItem.Name);
                cmd.Parameters.AddWithValue("@Price", newMenuItem.Price);
                if (string.IsNullOrEmpty(newMenuItem.Description)) // allow null
                    cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Description", newMenuItem.Description);
                cmd.Parameters.AddWithValue("@CategoryID", newMenuItem.CategoryID);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewMenuItemID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    newID = (int?)cmd.Parameters["@NewMenuItemID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewMenuItem: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static MenuItemsDTO? GetMenuItemByID(int? id)
        {
            if (id < 0) return null; // check PersonID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetMenuItemByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuItemID", id); // MenuItemID parameter.

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new MenuItemsDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                reader.GetInt32(reader.GetOrdinal("CategoryID"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetMenuItemByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static async Task<MenuItemsDTO?> GetMenuItemByIDAsync(int? id)
        {
            if (id < 0) return null; // check MenuItemID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetMenuItemByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuItemID", id); // MenuItemID parameter.

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        if (reader.Read())
                        {
                            return new MenuItemsDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                await reader.IsDBNullAsync(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                reader.GetInt32(reader.GetOrdinal("CategoryID"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetMenuItemByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static bool UpdateMenuItem(MenuItemsDTO uMenuItem)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateMenuItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuItemID", uMenuItem.MenuItemID);
                cmd.Parameters.AddWithValue("@Name", uMenuItem.Name);
                cmd.Parameters.AddWithValue("@Price", uMenuItem.Price);
                if (string.IsNullOrEmpty(uMenuItem.Description)) // allow null
                    cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Description", uMenuItem.Description);
                cmd.Parameters.AddWithValue("@CategoryID", uMenuItem.CategoryID);

                try
                {
                    conn.Open();

                    IsUpdated = cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateMenuItem: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static async Task<bool> UpdateMenuItemAsync(MenuItemsDTO uMenuItem)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateMenuItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuItemID", uMenuItem.MenuItemID);
                cmd.Parameters.AddWithValue("@Name", uMenuItem.Name);
                cmd.Parameters.AddWithValue("@Price", uMenuItem.Price);
                if (string.IsNullOrEmpty(uMenuItem.Description)) // allow null
                    cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Description", uMenuItem.Description);
                cmd.Parameters.AddWithValue("@CategoryID", uMenuItem.CategoryID);

                try
                {
                    await conn.OpenAsync();

                    IsUpdated = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateMenuItem: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static bool IsMenuItemExists(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsMenuItemExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuItemID", id);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsMenuItemExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static async Task<bool> IsMenuItemExistsAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsMenuItemExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuItemID", id);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsMenuItemExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static bool DeleteMenuItem(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteMenuItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuItemID", id);

                try
                {
                    conn.Open();

                    IsDeleted = (cmd.ExecuteNonQuery() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteMenuItem: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static async Task<bool> DeleteMenuItemAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteMenuItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuItemID", id);

                try
                {
                    await conn.OpenAsync();

                    IsDeleted = (await cmd.ExecuteNonQueryAsync() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteMenuItem: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static List<MenuItemsDTO?> GetAllMenuItems()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var MenuItemsList = new List<MenuItemsDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllMenuItems", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MenuItemsList.Add(new MenuItemsDTO(

                                 reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                 reader.GetString(reader.GetOrdinal("Name")),
                                 reader.GetDecimal(reader.GetOrdinal("Price")),
                                 reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                 reader.GetInt32(reader.GetOrdinal("CategoryID"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllMenuItems: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return MenuItemsList;
        }

        public static async Task<List<MenuItemsDTO?>> GetAllMenuItemsAsync()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var MenuItemsList = new List<MenuItemsDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllMenuItems", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MenuItemsList.Add(new MenuItemsDTO(
                                 reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                 reader.GetString(reader.GetOrdinal("Name")),
                                 reader.GetDecimal(reader.GetOrdinal("Price")),
                                 await reader.IsDBNullAsync(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                 reader.GetInt32(reader.GetOrdinal("CategoryID"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllMenuItems: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return MenuItemsList;
        }

    }
}
