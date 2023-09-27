using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class QueryExecutionService
    {
        private readonly IConfiguration _configuration;

        public QueryExecutionService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public void ExecuteQueryWithParameters(string query, Func<NpgsqlParameter[]> parametersFactory)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var cmd = new NpgsqlCommand(query, connection);
                NpgsqlParameter[] parameters = parametersFactory.Invoke();
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }
        
        public NpgsqlParameter[] CreateParameters(params (string Name, object Value)[] paramValues)
        {
            var parameters = new List<NpgsqlParameter>();

            foreach (var paramValue in paramValues)
            {
                parameters.Add(new NpgsqlParameter(paramValue.Name, paramValue.Value));
            }

            return parameters.ToArray();
        }
    }
}
