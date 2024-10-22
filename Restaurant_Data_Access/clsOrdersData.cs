using Microsoft.Data.SqlClient;
using Restaurant_Data_Access.DTOs.OrderDTOs;
using System.Data;
using System.Reflection;
using Util;

namespace Restaurant_Data_Access
{
    public class clsOrdersData
    {
        private static string? ConnectionString = clsDataSettings._connectionString;

        private static string? SourceName = Assembly.GetExecutingAssembly().GetName().Name;


        public static int AddNewOrder(OrdersDTO newOrder)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int newID = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewOrder", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                if (newOrder.UserID == null || newOrder.UserID < 1) // allow null
                    cmd.Parameters.AddWithValue("@UserID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@UserID", newOrder.UserID);
                cmd.Parameters.AddWithValue("@OrderDate", newOrder.OrderDate);
                cmd.Parameters.AddWithValue("@TotalAmount", newOrder.TotalAmount);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewOrderID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    newID = (int)cmd.Parameters["@NewOrderID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewOrder: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static async Task<int> AddNewOrderAsync(OrdersDTO newOrder)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int newID = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewOrder", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                if (newOrder.UserID == null || newOrder.UserID < 1) // allow null
                    cmd.Parameters.AddWithValue("@UserID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@UserID", newOrder.UserID);
                cmd.Parameters.AddWithValue("@OrderDate", newOrder.OrderDate);
                cmd.Parameters.AddWithValue("@TotalAmount", newOrder.TotalAmount);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewOrderID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    newID = (int)cmd.Parameters["@NewOrderID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewOrder: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static OrdersDTO? GetOrderByID(int id)
        {
            if (id < 0) return null; // check PersonID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetOrderByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", id); // OrderID parameter.

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new OrdersDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.IsDBNull(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                reader.GetDecimal(reader.GetOrdinal("AppliedTaxRate"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetOrderByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static async Task<OrdersDTO?> GetOrderByIDAsync(int id)
        {
            if (id < 0) return null; // check OrderID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetOrderByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", id); // OrderID parameter.

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        if (reader.Read())
                        {
                            return new OrdersDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                await reader.IsDBNullAsync(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                reader.GetDecimal(reader.GetOrdinal("AppliedTaxRate"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetOrderByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static bool UpdateOrder(OrdersDTO uOrder)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", uOrder.OrderID);
                if (uOrder.UserID == null || uOrder.UserID < 1) // allow null
                    cmd.Parameters.AddWithValue("@UserID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@UserID", uOrder.UserID);
                cmd.Parameters.AddWithValue("@OrderDate", uOrder.OrderDate);
                cmd.Parameters.AddWithValue("@TotalAmount", uOrder.TotalAmount);

                try
                {
                    conn.Open();

                    IsUpdated = cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateOrder: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static async Task<bool> UpdateOrderAsync(OrdersDTO uOrder)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", uOrder.OrderID);
                if (uOrder.UserID == null || uOrder.UserID < 1) // allow null
                    cmd.Parameters.AddWithValue("@UserID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@UserID", uOrder.UserID);
                cmd.Parameters.AddWithValue("@OrderDate", uOrder.OrderDate);
                cmd.Parameters.AddWithValue("@TotalAmount", uOrder.TotalAmount);

                try
                {
                    await conn.OpenAsync();

                    IsUpdated = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateOrder: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static bool IsOrderExists(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsOrderExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", id);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsOrderExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static async Task<bool> IsOrderExistsAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsOrderExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", id);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsOrderExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static bool DeleteOrder(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", id);

                try
                {
                    conn.Open();

                    IsDeleted = (cmd.ExecuteNonQuery() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteOrder: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static async Task<bool> DeleteOrderAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", id);

                try
                {
                    await conn.OpenAsync();

                    IsDeleted = (await cmd.ExecuteNonQueryAsync() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteOrder: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static IEnumerable<OrdersDTO?> GetAllOrders()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var OrdersList = new List<OrdersDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllOrders", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdersList.Add(new OrdersDTO(

                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.IsDBNull(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
                                 reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                 reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                 reader.GetDecimal(reader.GetOrdinal("AppliedTaxRate"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllOrders: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return OrdersList;
        }

        public static async Task<IEnumerable<OrdersDTO?>> GetAllOrdersAsync()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var OrdersList = new List<OrdersDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllOrders", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdersList.Add(new OrdersDTO(
                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 await reader.IsDBNullAsync(reader.GetOrdinal("UserID")) ? null : reader.GetInt32(reader.GetOrdinal("UserID")),
                                 reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                 reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
								 await reader.IsDBNullAsync(reader.GetOrdinal("AppliedTaxRate")) ? null : reader.GetDecimal(reader.GetOrdinal("AppliedTaxRate"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllOrders: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return OrdersList;
        }

        public static InvoiceDTO? GetInvoice(int? orderId)
        {
            if (orderId < 0 || !orderId.HasValue || orderId > int.MaxValue) return null; // check OrderId maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            InvoiceDTO invoice = null!;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SP_GetInvoiceHeader", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderID", orderId); // OrderID parameter.

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 1 ---> Get Invoice Header.
                                invoice = new InvoiceDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    reader.GetString(reader.GetOrdinal("CreatedByUser")),
                                    reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                    reader.GetDecimal(reader.GetOrdinal("AppliedTaxRate"))
                                );
                            }
                        }
                    }
                    catch (Exception ex) when (ex is SqlException || ex is Exception)
                    {
                        clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetInvoiceHeader: {ex.Message}", clsUtil.enEventType.Error);
                    }
                }


                // 2 ---> Get Invoice Details.
                invoice.OrderDetails = GetInvoiceDetails(conn, orderId);
            }

            return invoice;
        }
        
        public static async Task<InvoiceDTO?> GetInvoiceAsync(int? orderId)
        {
            if (orderId < 0 || !orderId.HasValue || orderId > int.MaxValue) return null; // check OrderId maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            InvoiceDTO invoice = null!;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("SP_GetInvoiceHeader", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderID", orderId); // OrderID parameter.

                    try
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // 1 ---> Get Invoice Header.
                                invoice = new InvoiceDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    reader.GetString(reader.GetOrdinal("CreatedByUser")),
                                    reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                    reader.GetDecimal(reader.GetOrdinal("AppliedTaxRate"))
                                );
                            }
                        }
                    }
                    catch (Exception ex) when (ex is SqlException || ex is Exception)
                    {
                        clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetInvoiceHeader: {ex.Message}", clsUtil.enEventType.Error);
                    }
                }


                // 2 ---> Get Invoice Details.
                invoice.OrderDetails = GetInvoiceDetails(conn, orderId);
            }

            return invoice;
        }

        private static List<InvoiceDTO.OrderDetail> GetInvoiceDetails(SqlConnection conn, int? orderId)
        {
            var list = new List<InvoiceDTO.OrderDetail>();

            // 1 ==> Get Invoice Header.
            using (SqlCommand cmd = new SqlCommand("SP_GetInvoiceDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderID", orderId); // OrderID parameter.

                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new InvoiceDTO.OrderDetail
                            (
                                reader.GetString(reader.GetOrdinal("ItemName")),
                                reader.GetString(reader.GetOrdinal("CategoryName")),
                                reader.GetInt32(reader.GetOrdinal("Quantity")),
                                reader.GetDecimal(reader.GetOrdinal("Price"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetInvoiceHeader: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return list;
        }
        private static async Task<List<InvoiceDTO.OrderDetail>> GetInvoiceDetailsAsync(SqlConnection conn, int? orderId)
        {
            var list = new List<InvoiceDTO.OrderDetail>();

            // 1 ==> Get Invoice Header.
            using (SqlCommand cmd = new SqlCommand("SP_GetInvoiceDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderID", orderId); // OrderID parameter.

                try
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new InvoiceDTO.OrderDetail
                            (
                                reader.GetString(reader.GetOrdinal("ItemName")),
                                reader.GetString(reader.GetOrdinal("CategoryName")),
                                reader.GetInt32(reader.GetOrdinal("Quantity")),
                                reader.GetDecimal(reader.GetOrdinal("Price"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetInvoiceHeader: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return list;
        }
    }
}
