using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Restaurant_Data_Access.DTOs.OrderDetailsDTOs;
using System.Data;
using System.Reflection;
using Util;

namespace Restaurant_Data_Access
{
    public class clsOrderDetailsData
    {
        private static string? ConnectionString = clsDataSettings._connectionString;

        private static string? SourceName = Assembly.GetExecutingAssembly().GetName().Name;

        public static bool AddNewOrderDetails(OrderDetailsDTO newOrderDetails)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsAdded = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewOrderDetails", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", newOrderDetails.OrderID);
                cmd.Parameters.AddWithValue("@MenuItemID", newOrderDetails.MenuItemID);
                cmd.Parameters.AddWithValue("@Quantity", newOrderDetails.Quantity);
                cmd.Parameters.AddWithValue("@Price", newOrderDetails.Price);

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    IsAdded = true;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewOrderDetails: {ex.Message}", clsUtil.enEventType.Error);
                    IsAdded = false;
                }
            }

            return IsAdded;
        }

        public static async Task<bool> AddNewOrderDetailsAsync(OrderDetailsDTO newOrderDetails)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsAdded = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewOrderDetails", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", newOrderDetails.OrderID);
                cmd.Parameters.AddWithValue("@MenuItemID", newOrderDetails.MenuItemID);
                cmd.Parameters.AddWithValue("@Quantity", newOrderDetails.Quantity);
                cmd.Parameters.AddWithValue("@Price", newOrderDetails.Price);

                try
                {
                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    IsAdded = true;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewOrderDetails: {ex.Message}", clsUtil.enEventType.Error);
                    IsAdded = false;
                }
            }

            return IsAdded;
        }

        public static async Task<bool> AddNewListOrderDetailsAsync(List<OrderDetailsDTO> newOrderDetailsList)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsAdded = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();

                foreach (var newOrderDetails in newOrderDetailsList)
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddNewOrderDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@OrderID", newOrderDetails.OrderID);
                        cmd.Parameters.AddWithValue("@MenuItemID", newOrderDetails.MenuItemID);
                        cmd.Parameters.AddWithValue("@Quantity", newOrderDetails.Quantity);
                        cmd.Parameters.AddWithValue("@Price", newOrderDetails.Price);

                        try
                        {
                            await cmd.ExecuteNonQueryAsync();

                            IsAdded = true;
                        }
                        catch (Exception ex) when (ex is SqlException || ex is Exception)
                        {
                            clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewOrderDetails: {ex.Message}", clsUtil.enEventType.Error);
                            IsAdded = false;
                        }
                    }
                }
            }

            return IsAdded;
        }

        public static OrderDetailsDTO? GetOrderDetailsByOrderID(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || menuItemId < 1) return null; // check OrderID Or MenuItemID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetOrderDetailsByOrderIDAndMenuItemID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@MenuItemID", menuItemId);

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new OrderDetailsDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                reader.GetInt32(reader.GetOrdinal("Quantity")),
                                reader.GetDecimal(reader.GetOrdinal("Price"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetOrderDetailsByOrderID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static async Task<OrderDetailsDTO?> GetOrderDetailsByOrderIDAsync(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || menuItemId < 1) return null; // check OrderID Or MenuItemID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetOrderDetailsByOrderIDAndMenuItemID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@MenuItemID", menuItemId);
                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        if (reader.Read())
                        {
                            return new OrderDetailsDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                reader.GetInt32(reader.GetOrdinal("Quantity")),
                                reader.GetDecimal(reader.GetOrdinal("Price"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetOrderDetailsByOrderID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static List<OrderDetailsDTO?> GetListOrderDetailsByOrderID(int? orderId)
        {
            if (orderId < 0) return null!; // check OrderID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var orderDetails = new List<OrderDetailsDTO>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetOrderDetailsByOrderID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", orderId); // OrderID parameter.

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            orderDetails.Add(new OrderDetailsDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                reader.GetInt32(reader.GetOrdinal("Quantity")),
                                reader.GetDecimal(reader.GetOrdinal("Price"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetOrderDetailsByOrderID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return orderDetails!;
        }

        public static async Task<List<OrderDetailsDTO?>> GetListOrderDetailsByOrderIDAsync(int? orderId)
        {
            if (orderId < 0) return null!; // check OrderID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var orderDetails = new List<OrderDetailsDTO>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetOrderDetailsByOrderID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", orderId); // OrderID parameter.

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        while (reader.Read())
                        {
                            orderDetails.Add(new OrderDetailsDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                reader.GetInt32(reader.GetOrdinal("Quantity")),
                                reader.GetDecimal(reader.GetOrdinal("Price"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetOrderDetailsByOrderID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return orderDetails!;
        }

        public static bool UpdateOrderDetails(OrderDetailsDTO uOrderDetails)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateOrderDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", uOrderDetails.OrderID);
                cmd.Parameters.AddWithValue("@MenuItemID", uOrderDetails.MenuItemID);
                cmd.Parameters.AddWithValue("@Quantity", uOrderDetails.Quantity);
                cmd.Parameters.AddWithValue("@Price", uOrderDetails.Price);

                try
                {
                    conn.Open();

                    IsUpdated = cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateOrderDetails: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static async Task<bool> UpdateOrderDetailsAsync(OrderDetailsDTO uOrderDetails)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateOrderDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", uOrderDetails.OrderID);
                cmd.Parameters.AddWithValue("@MenuItemID", uOrderDetails.MenuItemID);
                cmd.Parameters.AddWithValue("@Quantity", uOrderDetails.Quantity);
                cmd.Parameters.AddWithValue("@Price", uOrderDetails.Price);

                try
                {
                    await conn.OpenAsync();

                    IsUpdated = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateOrderDetails: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static bool IsOrderDetailsExists(int? orderId, int? menuItemId)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsOrderDetailsExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@MenuItemID", menuItemId);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsOrderDetailsExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static async Task<bool> IsOrderDetailsExistsAsync(int? orderId, int? menuItemId)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsOrderDetailsExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@MenuItemID", menuItemId);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsOrderDetailsExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static bool DeleteOrderItem(int? orderId, int? menuItemId)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteOrderItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@MenuItemID", menuItemId);

                try
                {
                    conn.Open();

                    IsDeleted = (cmd.ExecuteNonQuery() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteOrderItem: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static async Task<bool> DeleteOrderItemAsync(int? orderId, int? menuItemId)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteOrderItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@MenuItemID", menuItemId);

                try
                {
                    await conn.OpenAsync();

                    IsDeleted = (await cmd.ExecuteNonQueryAsync() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteOrderItem: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static List<OrderDetailsDTO?> GetAllOrderDetails()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var OrderDetailsList = new List<OrderDetailsDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllOrderDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderDetailsList.Add(new OrderDetailsDTO(

                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                 reader.GetInt32(reader.GetOrdinal("Quantity")),
                                 reader.GetDecimal(reader.GetOrdinal("Price"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllOrderDetails: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return OrderDetailsList;
        }

        public static async Task<List<OrderDetailsDTO?>> GetAllOrderDetailsAsync()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var OrderDetailsList = new List<OrderDetailsDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllOrderDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderDetailsList.Add(new OrderDetailsDTO(
                                 reader.GetInt32(reader.GetOrdinal("OrderID")),
                                 reader.GetInt32(reader.GetOrdinal("MenuItemID")),
                                 reader.GetInt32(reader.GetOrdinal("Quantity")),
                                 reader.GetDecimal(reader.GetOrdinal("Price"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllOrderDetails: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return OrderDetailsList;
        }

    }
}
