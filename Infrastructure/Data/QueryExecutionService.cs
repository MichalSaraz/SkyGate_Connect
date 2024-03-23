using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data
{
    public class QueryExecutionService
    {
        private readonly IConfiguration _configuration;

        public QueryExecutionService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Executes a query with parameters.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <param name="parametersFactory">A function that returns an array of NpgsqlParameter objects representing
        /// the query parameters.</param>
        public void ExecuteQueryWithParameters(string query, Func<NpgsqlParameter[]> parametersFactory)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(query, connection);
            var parameters = parametersFactory.Invoke();
            cmd.Parameters.AddRange(parameters);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates an array of NpgsqlParameter objects based on the given parameter values.
        /// </summary>
        /// <param name="paramValues">An array of tuples containing the parameter name and value.</param>
        /// <returns>An array of NpgsqlParameter objects.</returns>
        public NpgsqlParameter[] CreateParameters(params (string Name, object Value)[] paramValues)
        {
            return paramValues.Select(paramValue => new NpgsqlParameter(paramValue.Name, paramValue.Value)).ToArray();
        }
    }
}
