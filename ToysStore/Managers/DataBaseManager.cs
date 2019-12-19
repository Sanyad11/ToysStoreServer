using Dapper;
using ToysStore.DataContracts;
using Serilog;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using ToysStore.Entities;
using System.Collections.Generic;

namespace ToysStore.Managers
{
    /// <summary>
    /// current realization of DManager with all available methods
    /// </summary>
    public class DataBaseManager : IDataBaseManager
    {
        private static string _defaulConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private ILogger _logger;
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="logger"></param>
        public DataBaseManager(ILogger logger)
        {
            _logger = logger;
        }

        private SqlConnection OpenConnection()
        {

            var connection = new SqlConnection(_defaulConnection);
            connection.Open();
            return connection;
        }
        
        /// <summary>
        /// Check is exist user with that Login and Email
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public bool isExistUser(UserData userData)
        {            
            using (var con = OpenConnection())
            {
                string isExistUser = $"SELECT COUNT(Id) FROM Users WHERE Login = '{userData.Login}' AND Email = '{userData.Email}'";
                var result = (int)con.ExecuteScalar(isExistUser);
                if (result == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Add user to data base
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(UserData user)
        {          
            using (var con = OpenConnection())
            {
                string sqlQuery = "INSERT INTO Users (Login, Password, Email) VALUES( @Login, @Password, @Email);";
                con.Execute(sqlQuery, new { user.Login, user.Password, user.Email });
            }
        }

        /// <summary>
        /// Check is login and password correct
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsExistUser(string login, string password)
        {
            using (var con = OpenConnection())
            {
                string sql = $"SELECT * FROM Users WHERE Login = @login AND Password = @password";
                var result = con.Query(sql, new { login, password });
                if (result == null)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check is exist toy with that ID
        /// </summary>
        /// <param name="toyId"></param>
        /// <returns></returns>
        public bool IsExistToy(int toyId)
        {
            using (var con = OpenConnection())
            {
                string isExistToy = $"SELECT COUNT(*) FROM Toys WHERE Id = {toyId}";
                var result = (int)con.ExecuteScalar(isExistToy);
                if (result != 1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns 1 toy with provided ID
        /// </summary>
        /// <param name="toyId"></param>
        public ToysData GetToyById(int toyId)
        {
            using (var con = OpenConnection())
            {
                var sqlQuery = $"SELECT * FROM Toys WHERE Id = {toyId}";
                ToysData toy = con.Query<ToysData>(sqlQuery).First();
                return toy;
            }
        }

        /// <summary>
        /// Returns All Toys
        /// </summary>
        /// <returns></returns>
        public List<AllToysRequest> GetAllToys()
        {
            List<AllToysRequest> allToys = new List<AllToysRequest>();
            using (var con = OpenConnection())
            {
                var sqlQuery = "SELECT * FROM Toys";
                allToys = con.Query<AllToysRequest>(sqlQuery).ToList();
                return allToys;
            }
        }

        /// <summary>
        /// If exist record - updates Count, if no - inserts new record
        /// </summary>
        /// <param name="backetData"></param>
        public void UpdateBacket(BacketData backetData)
        {
                using (var con = OpenConnection())
                {
                string sqlQuery = $"UPDATE Backet SET Count = Count + {backetData.Count} WHERE UserId = {backetData.UserId} AND ToyId = {backetData.ToyId};" +
                    $"INSERT INTO Backet (UserId, ToyId, Count) SELECT {backetData.UserId}, {backetData.ToyId}, {backetData.Count} WHERE NOT EXISTS(SELECT 1 FROM Backet WHERE UserId = {backetData.UserId} AND ToyId = {backetData.ToyId});";
                con.Execute(sqlQuery);
                }
        }

        /// <summary>
        /// Returns all choised toys
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AllToysRequest> GetBucketListBacketData(int userId)
        {
            List<AllToysRequest> backetDatas = new List<AllToysRequest>();
            using (var con = OpenConnection())
            {
                var sqlQuery = $"SELECT ID, Name, Price FROM Backet JOIN Toys ON Toys.Id = Backet.ToyId WHERE UserId = {userId}";
                backetDatas = con.Query<AllToysRequest>(sqlQuery).ToList();
                return backetDatas;
            }
        }

        /// <summary>
        /// Calculate price for all choised toys
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetBucketListPrice(int userId)
        {
            using (var con = OpenConnection())
            {
                var sqlQuery = $"SELECT SUM(Toys.Price * Backet.Count) FROM Backet JOIN Toys ON Toys.Id = Backet.ToyId WHERE UserId = {userId}";
                var result = (int)con.ExecuteScalar(sqlQuery);
                return result;
            }
        }

        /// <summary>
        /// Remove all rows with current userId from DB
        /// </summary>
        /// <param name="userId"></param>
        public void BuyAllToysFromBacket(int userId)
        {
            using (var con = OpenConnection())
            {
                string sqlQuery = $"DELETE FROM Backet WHERE UserId = {userId};";
                con.Execute(sqlQuery);
            }
        }

        /// <summary>
        /// Removes BacketData record
        /// </summary>
        /// <param name="backetData"></param>
        public void RemoveFromBacket(int userId, int toyId)
        {
            using (var con = OpenConnection())
            {
                string sqlQuery = $"DELETE FROM Backet WHERE UserId = {userId} AND ToyId = {toyId};";
                con.Execute(sqlQuery);
            }
        }
    }
}