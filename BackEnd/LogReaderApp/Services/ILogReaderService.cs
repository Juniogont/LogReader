using LogReaderApp.Models;
using LogReaderApp.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogReaderApp.Services
{
    public interface ILogReaderService
    {
        IEnumerable<Log> GetAll();
        Task<Log> Get(int id);
        Task<IEnumerable<Log>> GetFiltered(FilterInput input);
        Task CreateBatch(Lote input);
        Task<int> Create(Log input);
        Task<bool> Update(int id, Log input);
        Task Delete(int id);
    }
}
