using Microsoft.Data.SqlClient;
using Restaurant_Data_Access.DTOs.EmployeeDTOs;
using System.Data;
using System.Reflection;
using Util;


namespace Restaurant_Data_Access
{
    public class clsEmployeesData
    {
        private static string? ConnectionString = clsDataSettings._connectionString;

        private static string? SourceName = Assembly.GetExecutingAssembly().GetName().Name;

        public static int? AddNewEmployee(EmployeesDTO newEmployee)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int? newID = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewEmployee", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", newEmployee.Name);
                cmd.Parameters.AddWithValue("@Position", newEmployee.Position);
                cmd.Parameters.AddWithValue("@HireDate", newEmployee.HireDate);
                if (newEmployee.Salary == null || newEmployee.Salary < 0) // allow null
                    cmd.Parameters.AddWithValue("@Salary", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Salary", newEmployee.Salary);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewEmployeeID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    newID = (int?)cmd.Parameters["@NewEmployeeID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewEmployee: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static async Task<int?> AddNewEmployeeAsync(EmployeesDTO newEmployee)
        {

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            int? newID = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddNewEmployee", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", newEmployee.Name);
                cmd.Parameters.AddWithValue("@Position", newEmployee.Position);
                cmd.Parameters.AddWithValue("@HireDate", newEmployee.HireDate);
                if (newEmployee.Salary == null || newEmployee.Salary < 0) // allow null
                    cmd.Parameters.AddWithValue("@Salary", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Salary", newEmployee.Salary);

                // output parameter.
                SqlParameter outputParameter = new SqlParameter("@NewEmployeeID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                try
                {
                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    newID = (int?)cmd.Parameters["@NewEmployeeID"].Value;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_AddNewEmployee: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return newID;
        }

        public static EmployeesDTO? GetEmployeeByID(int? id)
        {
            if (id < 0) return null; // check PersonID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetEmployeeByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", id); // EmployeeID parameter.

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new EmployeesDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Position")),
                                reader.GetDateTime(reader.GetOrdinal("HireDate")),
                                reader.IsDBNull(reader.GetOrdinal("Salary")) ? null : reader.GetDecimal(reader.GetOrdinal("Salary"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetEmployeeByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static async Task<EmployeesDTO?> GetEmployeeByIDAsync(int? id)
        {
            if (id < 0) return null; // check EmployeeID maybe data is not correct.

            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetEmployeeByID", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", id); // EmployeeID parameter.

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        if (reader.Read())
                        {
                            return new EmployeesDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Position")),
                                reader.GetDateTime(reader.GetOrdinal("HireDate")),
                                await reader.IsDBNullAsync(reader.GetOrdinal("Salary")) ? null : reader.GetDecimal(reader.GetOrdinal("Salary"))
                            );
                        }
                        else
                            return null;
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetEmployeeByID: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return null;
        }

        public static bool UpdateEmployee(EmployeesDTO uEmployee)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateEmployee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", uEmployee.EmployeeID);
                cmd.Parameters.AddWithValue("@Name", uEmployee.Name);
                cmd.Parameters.AddWithValue("@Position", uEmployee.Position);
                cmd.Parameters.AddWithValue("@HireDate", uEmployee.HireDate);
                if (uEmployee.Salary == null || uEmployee.Salary < 0) // allow null
                    cmd.Parameters.AddWithValue("@Salary", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Salary", uEmployee.Salary);

                try
                {
                    conn.Open();

                    IsUpdated = cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateEmployee: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static async Task<bool> UpdateEmployeeAsync(EmployeesDTO uEmployee)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }
            bool IsUpdated = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateEmployee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", uEmployee.EmployeeID);
                cmd.Parameters.AddWithValue("@Name", uEmployee.Name);
                cmd.Parameters.AddWithValue("@Position", uEmployee.Position);
                cmd.Parameters.AddWithValue("@HireDate", uEmployee.HireDate);
                if (uEmployee.Salary == null || uEmployee.Salary < 0) // allow null
                    cmd.Parameters.AddWithValue("@Salary", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Salary", uEmployee.Salary);

                try
                {
                    await conn.OpenAsync();

                    IsUpdated = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_UpdateEmployee: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsUpdated;
        }

        public static bool IsEmployeeExists(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsEmployeeExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", id);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsEmployeeExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static async Task<bool> IsEmployeeExistsAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsExists = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_IsEmployeeExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", id);

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
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_IsEmployeeExists: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsExists;
        }

        public static bool DeleteEmployee(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteEmployee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", id);

                try
                {
                    conn.Open();

                    IsDeleted = (cmd.ExecuteNonQuery() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteEmployee: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static async Task<bool> DeleteEmployeeAsync(int? id)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            bool IsDeleted = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteEmployee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", id);

                try
                {
                    await conn.OpenAsync();

                    IsDeleted = (await cmd.ExecuteNonQueryAsync() > 0);
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_DeleteEmployee: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return IsDeleted;
        }

        public static List<EmployeesDTO?> GetAllEmployees()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var EmployeesList = new List<EmployeesDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllEmployees", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeesList.Add(new EmployeesDTO(

                                 reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                 reader.GetString(reader.GetOrdinal("Name")),
                                 reader.GetString(reader.GetOrdinal("Position")),
                                 reader.GetDateTime(reader.GetOrdinal("HireDate")),
                                 reader.IsDBNull(reader.GetOrdinal("Salary")) ? null : reader.GetDecimal(reader.GetOrdinal("Salary"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllEmployees: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return EmployeesList;
        }

        public static async Task<List<EmployeesDTO?>> GetAllEmployeesAsync()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                clsUtil.StoreEventInEventLogs(SourceName, $"Connection string is not set.", clsUtil.enEventType.Error);
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            var EmployeesList = new List<EmployeesDTO?>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllEmployees", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeesList.Add(new EmployeesDTO(
                                 reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                 reader.GetString(reader.GetOrdinal("Name")),
                                 reader.GetString(reader.GetOrdinal("Position")),
                                 reader.GetDateTime(reader.GetOrdinal("HireDate")),
                                 await reader.IsDBNullAsync(reader.GetOrdinal("Salary")) ? null : reader.GetDecimal(reader.GetOrdinal("Salary"))
                            ));
                        }
                    }
                }
                catch (Exception ex) when (ex is SqlException || ex is Exception)
                {
                    clsUtil.StoreEventInEventLogs(SourceName, $"Error SP_GetAllEmployees: {ex.Message}", clsUtil.enEventType.Error);
                }
            }

            return EmployeesList;
        }
    }
}
