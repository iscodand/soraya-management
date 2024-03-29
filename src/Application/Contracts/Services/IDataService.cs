using Application.Dtos.Data;
using Application.Responses;

namespace Application.Contracts.Services
{
    public interface IDataService
    {
        public Task<BaseResponse<GetDataDto>> GetDataAsync(int companyId, DateTime initialDate, DateTime finalDate);
    }
}