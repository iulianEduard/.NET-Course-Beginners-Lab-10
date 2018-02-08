using Dapper.Contrib.Extensions;
using Dapper.Ports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Dapper
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Attributes

        private IDbTransaction sqlTransaction = null;

        private readonly string connectionString = null;

        #endregion Attributes

        public Repository()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DapperEntities"].ConnectionString;
        }

        #region Public Methods

        public void OpenTransaction(string transactionName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                sqlTransaction = conn.BeginTransaction(IsolationLevel.Serializable, transactionName);
            }
        }

        public void CommitTransaction(string transactionName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (sqlTransaction != null)
                {
                    sqlTransaction.Commit();
                }
            }
        }

        public void CloseTransaction(string transactionName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (sqlTransaction != null)
                {
                    sqlTransaction.Dispose();
                }
            }
        }

        #endregion Public Methods

        #region IRepository implementation

        public void Add(T entity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                conn.Insert(entity);

                conn.Close();
            }
        }

        public async Task<int> AddAsync(T entity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var id = await conn.InsertAsync(entity);

                conn.Close();

                return await Task.FromResult(id);
            }
        }

        public void Delete(T entity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                conn.Delete(entity);

                conn.Close();
            }
        }

        public async Task DeleteAsync(T entity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                await conn.DeleteAsync(entity);

                conn.Close();
            }
        }

        public T Get(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var entity = conn.Get<T>(id);

                conn.Close();

                return entity;
            }
        }

        public async Task<T> GetAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var entity = await conn.GetAsync<T>(id);

                conn.Close();

                return entity;
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var entityList = conn.GetAll<T>();

                conn.Close();

                return entityList;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var entityList = await conn.GetAllAsync<T>();

                conn.Close();

                return await Task.FromResult(entityList);
            }
        }

        public void Update(T entity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                conn.Update<T>(entity);

                conn.Close();
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                bool result = await conn.UpdateAsync(entity);

                conn.Close();
            }
        }

        #endregion IRepository implementation

        #region IQueryRepository implementation

        public IEnumerable<R> ExecuteQuery<R>(string name, bool? usingTransaction = null, int? timeout = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                dynamic result;

                if (usingTransaction.HasValue && usingTransaction.Value == true)
                {
                    if (sqlTransaction == null)
                        throw new Exception("Transaction not set");

                    result = conn.Query<R>(name, null, sqlTransaction, true, timeout, CommandType.Text);
                }
                else
                {
                    result = conn.Query<R>(name, null, null, true, timeout, CommandType.Text);
                }

                conn.Close();
                return result;
            }
        }

        public async Task<IEnumerable<R>> ExecuteQueryAsync<R>(string name, bool? usingTransaction = null, int? timeout = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                dynamic result;

                if (usingTransaction.HasValue && usingTransaction.Value == true)
                {
                    if (sqlTransaction == null)
                        throw new Exception("Transaction not set");

                    result = conn.QueryAsync<R>(name, null, sqlTransaction, timeout, CommandType.Text).Result;
                }
                else
                {
                    result = conn.QueryAsync<R>(name, null, null, timeout, CommandType.Text).Result;
                }

                conn.Close();
                return await Task.FromResult(result);
            }
        }

        public IEnumerable<R> SingleSetStoredProcedure<R>(string name, DynamicParameters parameters = null, bool? usingTransaction = null, int? timeout = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                dynamic result;

                if (usingTransaction.HasValue && usingTransaction.Value == true)
                {
                    if (sqlTransaction == null)
                        throw new Exception("Transaction not set");

                    result = conn.Query<R>(name, parameters, sqlTransaction, true, timeout, CommandType.StoredProcedure);
                }
                else
                {
                    result = conn.Query<R>(name, parameters, null, true, timeout, CommandType.StoredProcedure);
                }

                conn.Close();
                return result;
            }
        }

        public async Task<IEnumerable<R>> SingleSetStoredProcedureAsync<R>(string name, DynamicParameters parameters = null, bool? usingTransaction = null, int? timeout = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                dynamic result;

                if (usingTransaction.HasValue && usingTransaction.Value == true)
                {
                    if (sqlTransaction == null)
                        throw new Exception("Transaction not set");

                    result = conn.QueryAsync<R>(name, parameters, sqlTransaction, timeout, CommandType.StoredProcedure).Result;
                }
                else
                {
                    result = conn.QueryAsync<R>(name, parameters, null, timeout, CommandType.StoredProcedure).Result;
                }

                conn.Close();
                return await Task.FromResult(result);
            }
        }

        public R MultipleSetStoredProcedure<R>(string name, Func<GridReader, R> readerFuncs, DynamicParameters parameters = null, bool? usingTransaction = null, int? timeout = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (usingTransaction.HasValue && usingTransaction.Value == true)
                {
                    if (sqlTransaction == null)
                        throw new Exception("Transaction not set");

                    var gridReader = conn.QueryMultiple(name, parameters, sqlTransaction, timeout, CommandType.StoredProcedure);

                    return readerFuncs(gridReader);
                }
                else
                {
                    var gridReader = conn.QueryMultiple(name, parameters, null, timeout, CommandType.StoredProcedure);

                    return readerFuncs(gridReader);
                }
            }
        }

        public async Task<R> MultipleSetStoredProcedureAsync<R>(string name, Func<GridReader, R> readerFuncs, DynamicParameters parameters = null, bool? usingTransaction = null, int? timeout = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                if (usingTransaction.HasValue && usingTransaction.Value == true)
                {
                    if (sqlTransaction == null)
                        throw new Exception("Transaction not set");

                    var gridReader = await conn.QueryMultipleAsync(name, parameters, sqlTransaction, timeout, CommandType.StoredProcedure);

                    return Task.FromResult(readerFuncs(gridReader)).Result;
                }
                else
                {
                    var gridReader = await conn.QueryMultipleAsync(name, parameters, null, timeout, CommandType.StoredProcedure);

                    return Task.FromResult(readerFuncs(gridReader)).Result;
                }
            }
        }

        #endregion IQueryRepository implementation
    }
}
