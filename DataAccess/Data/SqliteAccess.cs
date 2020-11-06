using DataAccess.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataAccessLibrary.Data
{
    public class SqliteAccess
    {
        #region Properties,Initialize

        private static string _dbpath { get; set; }

        public static async Task InitializeSqlite(string databaseName)
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(databaseName, CreationCollisionOption.OpenIfExists);
            _dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, databaseName);
            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "CREATE TABLE IF NOT EXISTS Customers(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, CREATED DATETIME NOT NULL); CREATE TABLE IF NOT EXISTS Problems(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, CustomerId INTEGER NOT NULL, Status TEXT NOT NULL, Category TEXT NOT NULL, Description TEXT NOT NULL, CREATED DATETIME NOT NULL, FOREIGN KEY (CustomerId) REFERENCES Customers(Id)); CREATE TABLE IF NOT EXISTS Comments(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ProblemId INTEGER NOT NULL, Description TEXT NOT NULL, CREATED DATETIME NOT NULL, FOREIGN KEY (ProblemId) REFERENCES Problems(Id));";

                var cmd = new SqliteCommand(query, db);

                await cmd.ExecuteNonQueryAsync();
                db.Close();

            }
        }

      
        #endregion



        #region Create Method
        public static async Task<long> CreateCustomerAsync(Customer customer)
        {
            long id = 0;
            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "INSERT INTO Customers VALUES(null, @Name, @Created)";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "SELECT last_insert_rowid()";
                id = (long)await cmd.ExecuteScalarAsync();
                db.Close();
            }
            return id;
        }

        public static async Task<long> CreateProblemAsync(Problem problem)
        {
            long id = 0;
            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "INSERT INTO Problems VALUES(null, @CustomerId, @Status, @Category, @Description, @Created) ";
                var cmd = new SqliteCommand(query, db);
                cmd.Parameters.AddWithValue("@CustomerId", problem.CustomerId);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@Category", problem.Category);
                cmd.Parameters.AddWithValue("@Description", problem.Description);
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "SELECT last_insert_rowid()";
                id = (long)await cmd.ExecuteScalarAsync();
                db.Close();
            }
            return id;

        }

        public static async Task CreateCommentAsync(Comment comment)
        {
          
            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "INSERT INTO Comments VALUES(null, @ProblemId, @Description, @Created) ";
                var cmd = new SqliteCommand(query, db);
                cmd.Parameters.AddWithValue("@ProblemId", comment.ProblemId);
                cmd.Parameters.AddWithValue("@Description", comment.Description);
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);

                await cmd.ExecuteNonQueryAsync();

                db.Close();
            }
       
            
        }


        #endregion

        #region Get  

        public static async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var customers = new List<Customer>();

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "SELECT * FROM Customers";
                var cmd = new SqliteCommand(query, db);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        customers.Add(new Customer(result.GetInt32(0), result.GetString(1), result.GetDateTime(3)));

                    }
                }
                db.Close();
            }

            return customers;
        }

        public static async Task<Customer> GetCustomerById(int id)
        {
            var customer = new Customer();

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "SELECT * FROM Customers WHERE Id = @Id";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Id", id);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        customer = new Customer(result.GetInt32(0), result.GetString(1), result.GetDateTime(2));

                    }
                }
                db.Close();
            }

            return customer;
        }

        public static async Task<long> GetCustomerIdByName(string name)
        {
            long customerid = 0;

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();

                var query = "SELECT Id FROM Customers WHERE Name = @Name";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Name", name);
                customerid = (long)await cmd.ExecuteScalarAsync();

                db.Close();
            }

            return customerid;
        }

        public static async Task<Customer> GetCustomerByName(string name)
        {
            var customer = new Customer();

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "SELECT * FROM Customers WHERE Name = @Name";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Name", name);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        customer = new Customer(result.GetInt32(0), result.GetString(1), result.GetDateTime(3));

                    }
                }
                db.Close();
            }

            return customer;
        }

        public static async Task<IEnumerable<string>> GetCustomerNames()
        {
            var customernames = new List<string>();

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();

                var query = "SELECT Name FROM Customers";
                var cmd = new SqliteCommand(query, db);

                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        customernames.Add(result.GetString(0));
                    }
                }

                db.Close();
            }

            return customernames;
        }


        public static async Task<Comment> GetCommentsByProblemId(int problemid)
        {
            var comments = new Comment();

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "SELECT * FROM Comments WHERE ProblemId = @ProblemId";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@ProblemId", problemid);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                       comments = new Comment(result.GetInt32(0), result.GetInt32(1), result.GetString(2), result.GetDateTime(3));

                    }
                }
                db.Close();
            }

            return comments;
        }


        public static async Task<IEnumerable<Problem>> GetAllProblems()
        {
            var problems = new List<Problem>();

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();
                var query = "SELECT * FROM Problems";
                var cmd = new SqliteCommand(query, db);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        var problem = new Problem(result.GetInt32(0), result.GetInt32(1), result.GetString(2), result.GetString(3), result.GetString(4), result.GetDateTime(5));
                        problem.Customer = await GetCustomerById(result.GetInt32(1));
                        problem.Comments = await GetCommentsByProblemId(result.GetInt32(0));

                        problems.Add(problem);
                    }
                }
                db.Close();
            }

            return problems;
        }


        public static async Task<Problem> UpdateProblemStatus(int id, string status)
        {
            var problem = new Problem();

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                

                db.Open();
                var query = "UPDATE Problems SET Status = @Status WHERE Id = @Id";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Status", status);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        problem = new Problem(result.GetInt32(0), result.GetInt32(1), result.GetString(2), result.GetString(3), result.GetString(4), result.GetDateTime(5));

                    }
                }
                db.Close();
            }

            return problem;
        }


        public static async Task<IEnumerable<long>> GetAllProblemsId()
        {
            var problemsId = new List<long>();

            using (SqliteConnection db = new SqliteConnection($"FileName = {_dbpath}"))
            {
                db.Open();

                var query = "SELECT Id FROM Problems";
                var cmd = new SqliteCommand(query, db);

                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        problemsId.Add(result.GetInt32(0));
                    }
                }

                db.Close();
            }

            return problemsId;
        }

    }

    


        #endregion









    
}
