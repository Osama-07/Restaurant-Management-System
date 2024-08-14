using Microsoft.Data.SqlClient;
using Restaurant_Data_Access.DTOs.InventoryDTOs;
using System.Data;
using System.Reflection;
using Util;

namespace Restaurant_Data_Access
{
    public class clsInventoryData
    {
        private static string? ConnectionString = clsDataSettings._connectionString;

        private static string? SourceName = Assembly.GetExecutingAssembly().GetName().Name;

        public static int? AddNewInventory(InventoryDTO newInventor)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int? newID = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewInventory", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemName", newInventor.ItemName);
                cmd.Parameters.AddWithValue("@Quantity", newInventor.Quantity);
                cmd.Parameters.AddWithValue("@Unit", newInventor.Unit);
                if (newInventor.ReorderLevel == null || newInventor.ReorderLevel < 0) // allow null
                    cmd.Parameters.AddWithValue("@ReorderLevel", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ReorderLevel", newInventor.ReorderLevel);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewInventoryID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    newID = (int?)cmd.Parameters["@NewInventoryID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewInventor: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static async Task<int?> AddNewInventoryAsync(InventoryDTO newInventor)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int? newID = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewInventory", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemName", newInventor.ItemName);
                cmd.Parameters.AddWithValue("@Quantity", newInventor.Quantity);
                cmd.Parameters.AddWithValue("@Unit", newInventor.Unit);
                if (newInventor.ReorderLevel == null || newInventor.ReorderLevel < 0) // allow null
                    cmd.Parameters.AddWithValue("@ReorderLevel", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ReorderLevel", newInventor.ReorderLevel);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewInventoryID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    newID = (int?)cmd.Parameters["@NewInventoryID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewInventor: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static InventoryDTO? GetInventoryByID(int? id)
        {
            if (id < 0) return null; // check PersonID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetInventoryByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InventoryID", id); // InventoryID parameter.

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new InventoryDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("InventoryID")),
                                reader.GetString(reader.GetOrdinal("ItemName")),
                                reader.GetDecimal(reader.GetOrdinal("Quantity")),
                                reader.GetString(reader.GetOrdinal("Unit")),
                                reader.IsDBNull(reader.GetOrdinal("ReorderLevel")) ? null : reader.GetDecimal(reader.GetOrdinal("ReorderLevel"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetInventorByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static async Task<InventoryDTO?> GetInventoryByIDAsync(int? id)
        {
            if (id < 0) return null; // check InventoryID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetInventoryByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InventoryID", id); // InventoryID parameter.

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        if (reader.Read())
                        {
                            return new InventoryDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("InventoryID")),
                                reader.GetString(reader.GetOrdinal("ItemName")),
                                reader.GetDecimal(reader.GetOrdinal("Quantity")),
                                reader.GetString(reader.GetOrdinal("Unit")),
                                await reader.IsDBNullAsync(reader.GetOrdinal("ReorderLevel")) ? null : reader.GetDecimal(reader.GetOrdinal("ReorderLevel"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetInventorByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static bool UpdateInventory(InventoryDTO uInventor)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateInventory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InventoryID", uInventor.InventoryID);
                cmd.Parameters.AddWithValue("@ItemName", uInventor.ItemName);
                cmd.Parameters.AddWithValue("@Quantity", uInventor.Quantity);
                cmd.Parameters.AddWithValue("@Unit", uInventor.Unit);
                if (uInventor.ReorderLevel == null || uInventor.ReorderLevel < 0) // allow null
                    cmd.Parameters.AddWithValue("@ReorderLevel", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ReorderLevel", uInventor.ReorderLevel);

                try
                {
                    conn.Open();

                    IsUpdated = cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateInventor: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static async Task<bool> UpdateInventoryAsync(InventoryDTO uInventor)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateInventory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InventoryID", uInventor.InventoryID);
                cmd.Parameters.AddWithValue("@ItemName", uInventor.ItemName);
                cmd.Parameters.AddWithValue("@Quantity", uInventor.Quantity);
                cmd.Parameters.AddWithValue("@Unit", uInventor.Unit);
                if (uInventor.ReorderLevel == null || uInventor.ReorderLevel < 0) // allow null
                    cmd.Parameters.AddWithValue("@ReorderLevel", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ReorderLevel", uInventor.ReorderLevel);

                try
                {
                    await conn.OpenAsync();

                    IsUpdated = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateInventor: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static bool IsInventoryExists(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsInventoryExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InventoryID", id);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsInventorExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static async Task<bool> IsInventoryExistsAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsInventoryExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InventoryID", id);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsInventorExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static bool DeleteInventory(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteInventory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InventoryID", id);

                try
                {
                    conn.Open();

                    IsDeleted = (cmd.ExecuteNonQuery() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteInventor: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static async Task<bool> DeleteInventoryAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteInventory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InventoryID", id);

                try
                {
                    await conn.OpenAsync();

                    IsDeleted = (await cmd.ExecuteNonQueryAsync() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteInventor: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static List<InventoryDTO?> GetAllInventory()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var InventoryList = new List<InventoryDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllInventory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InventoryList.Add(new InventoryDTO(

                                 reader.GetInt32(reader.GetOrdinal("InventoryID")),
                                 reader.GetString(reader.GetOrdinal("ItemName")),
                                 reader.GetDecimal(reader.GetOrdinal("Quantity")),
                                 reader.GetString(reader.GetOrdinal("Unit")),
                                 reader.IsDBNull(reader.GetOrdinal("ReorderLevel")) ? null : reader.GetDecimal(reader.GetOrdinal("ReorderLevel"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllInventory: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return InventoryList;
        }

        public static async Task<List<InventoryDTO?>> GetAllInventoryAsync()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var InventoryList = new List<InventoryDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllInventory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InventoryList.Add(new InventoryDTO(
                                 reader.GetInt32(reader.GetOrdinal("InventoryID")),
                                 reader.GetString(reader.GetOrdinal("ItemName")),
                                 reader.GetDecimal(reader.GetOrdinal("Quantity")),
                                 reader.GetString(reader.GetOrdinal("Unit")),
                                 await reader.IsDBNullAsync(reader.GetOrdinal("ReorderLevel")) ? null : reader.GetDecimal(reader.GetOrdinal("ReorderLevel"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllInventory: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return InventoryList;
        }
    }
}
