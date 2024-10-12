using Application.Dtos.Data;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface IDataService
    {
        public Task<Response<GetDataDto>> GetDataAsync(int companyId, DateTime initialDate, DateTime finalDate);
    }
}