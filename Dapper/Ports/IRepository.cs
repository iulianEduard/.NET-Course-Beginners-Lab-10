using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Dapper.Ports
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        Task<T> GetAsync(int id);

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        void Add(T entity);

        Task<int> AddAsync(T entity);

        void Delete(T entity);

        Task DeleteAsync(T entity);

        void Update(T entity);

        Task UpdateAsync(T entity);

        IEnumerable<R> ExecuteQuery<R>(string name, bool? usingTransaction = null, int? timeout = null);

        Task<IEnumerable<R>> ExecuteQueryAsync<R>(string name, bool? usingTransaction = null, int? timeout = null);

        IEnumerable<R> SingleSetStoredProcedure<R>(string name, DynamicParameters parameters = null, bool? usingTransaction = null, int? timeout = null);

        Task<IEnumerable<R>> SingleSetStoredProcedureAsync<R>(string name, DynamicParameters parameters = null, bool? usingTransaction = null, int? timeout = null);

        R MultipleSetStoredProcedure<R>(string name, Func<GridReader, R> func, DynamicParameters parameters = null, bool? usingTransaction = null, int? timeout = null);

        Task<R> MultipleSetStoredProcedureAsync<R>(string name, Func<GridReader, R> func, DynamicParameters parameters = null, bool? usingTransaction = null, int? timeout = null);
    }
}
