using Application.Contracts.Services;
using Application.Dtos.Meal;
using Application.Dtos.Order;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Data.Repositories;

namespace Application.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;

        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<BaseResponse<GetMealDto>> GetMealByIdAsync(int mealId, int userCompanyId)
        {
            Meal meal = await _mealRepository.GetByIdAsync(mealId);

            if (meal == null)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Sabor não encontrado",
                    IsSuccess = false
                };
            }

            if (meal.CompanyId != userCompanyId)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Esse sabor não pertence a sua empresa",
                    IsSuccess = false
                };
            }

            GetMealDto getMealDto = new()
            {
                Id = meal.Id,
                Description = meal.Description,
                Accompaniments = meal.Accompaniments,
                CreatedBy = meal.UserId
            };

            return new BaseResponse<GetMealDto>()
            {
                Message = "Sabor encontrado com sucesso",
                IsSuccess = true,
                Data = getMealDto
            };
        }

        public async Task<BaseResponse<DetailMealDto>> DetailMealAsync(int mealId, int userCompanyId)
        {
            Meal meal = await _mealRepository.DetailMealAsync(mealId);

            if (meal == null)
            {
                return new BaseResponse<DetailMealDto>()
                {
                    Message = "Sabor não encontrado",
                    IsSuccess = false
                };
            }

            if (meal.CompanyId != userCompanyId)
            {
                return new BaseResponse<DetailMealDto>()
                {
                    Message = "Esse sabor não pertence a sua empresa",
                    IsSuccess = false
                };
            }

            List<GetOrderDto> getOrderDtoCollection = new();
            foreach (Order order in meal.Orders)
            {
                GetOrderDto getOrderDto = new()
                {
                    Id = order.Id,
                    Description = order.Description,
                    Price = order.Price,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    PaymentType = order.PaymentType.Description,
                    Meal = order.Meal.Description,
                    Customer = order.Customer.Name,
                    CreatedAt = order.CreatedAt
                };

                getOrderDtoCollection.Add(getOrderDto);
            }

            DetailMealDto detailMealDto = new()
            {
                Id = meal.Id,
                Description = meal.Description,
                Accompaniments = meal.Accompaniments,
                Orders = getOrderDtoCollection,
                CreatedBy = meal.User.Name
            };

            return new BaseResponse<DetailMealDto>()
            {
                Message = "Sabor encontrado com sucesso",
                IsSuccess = true,
                Data = detailMealDto
            };
        }

        public async Task<BaseResponse<CreateMealDto>> CreateMealAsync(CreateMealDto createMealDto)
        {
            if (createMealDto == null)
            {
                return new BaseResponse<CreateMealDto>()
                {
                    Message = "Cliente não pode ser nulo.",
                    IsSuccess = false
                };
            }

            Meal meal = Meal.Create(
                createMealDto.Description,
                createMealDto.Accompaniments,
                createMealDto.CompanyId,
                createMealDto.UserId
            );

            await _mealRepository.CreateAsync(meal);

            return new BaseResponse<CreateMealDto>()
            {
                Message = "Sabor salvo com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetMealDto>> GetMealsByCompanyAsync(int companyId)
        {
            if (companyId <= 0)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente",
                    IsSuccess = false
                };
            }

            ICollection<Meal> meals = await _mealRepository.GetMealsByCompanyAsync(companyId);

            List<GetMealDto> getMealDtoCollection = new();
            foreach (Meal meal in meals)
            {
                GetMealDto getMealDto = new()
                {
                    Id = meal.Id,
                    Description = meal.Description,
                    Accompaniments = meal.Accompaniments,
                    CreatedBy = meal.User.Name,
                    OrdersCount = meal.Orders.Count
                };

                getMealDtoCollection.Add(getMealDto);
            }

            return new BaseResponse<GetMealDto>()
            {
                Message = "Sabores encontrados com sucesso",
                IsSuccess = true,
                DataCollection = getMealDtoCollection
            };
        }

        public async Task<BaseResponse<GetMealDto>> GetMealsByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate)
        {
            ICollection<Meal> meals = await _mealRepository.GetMealsByDateRangeAsync(userCompanyId, initialDate.Date, finalDate.Date);

            List<GetMealDto> getMealDtoCollection = new();
            foreach (Meal meal in meals)
            {
                GetMealDto getMealDto = new()
                {
                    Id = meal.Id,
                    Description = meal.Description,
                    Accompaniments = meal.Accompaniments,
                    OrdersCount = meal.Orders.Count
                };

                getMealDtoCollection.Add(getMealDto);
            }

            return new BaseResponse<GetMealDto>()
            {
                Message = "Sabores encontrados com sucesso",
                IsSuccess = true,
                DataCollection = getMealDtoCollection
            };
        }

        public async Task<BaseResponse<GetMealDto>> UpdateMealAsync(UpdateMealDto updateMealDto)
        {
            if (updateMealDto == null)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Sabor não pode ser nulo",
                    IsSuccess = false
                };
            }

            Meal meal = await _mealRepository.GetByIdAsync(updateMealDto.Id);

            if (meal == null)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Sabor não encontrado",
                    IsSuccess = false
                };
            }

            bool mealExists = await _mealRepository.MealExistsByDescriptionAsync(updateMealDto.Description, updateMealDto.UserCompanyId);

            if (mealExists)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Um sabor já foi cadastrado com essa Descrição. Verifique e tente novamente",
                    IsSuccess = false
                };
            }

            if (meal.CompanyId != updateMealDto.UserCompanyId)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Esse sabor não pertence a sua empresa",
                    IsSuccess = false
                };
            }

            meal.Update(updateMealDto.Description, updateMealDto.Accompaniments);

            await _mealRepository.SaveAsync();

            return new BaseResponse<GetMealDto>()
            {
                Message = "Sabor atualizado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetMealDto>> DeleteMealAsync(DeleteMealDto deleteMealDto)
        {
            if (deleteMealDto == null)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Sabor não pode ser nulo",
                    IsSuccess = false
                };
            }

            Meal meal = await _mealRepository.DetailMealAsync(deleteMealDto.Id);

            if (meal == null)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Sabor não encontrado",
                    IsSuccess = false
                };
            }

            if (meal.Orders.Count > 0)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = $"Você não pode deletar esse sabor, pois ele está vinculado a {meal.Orders.Count} pedido(s)",
                    IsSuccess = false
                };
            }

            if (meal.CompanyId != deleteMealDto.UserCompanyId)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Esse sabor não pertence a sua empresa",
                    IsSuccess = false
                };
            }

            await _mealRepository.DeleteAsync(meal);

            return new BaseResponse<GetMealDto>()
            {
                Message = "Sabor deletado com sucesso",
                IsSuccess = true
            };
        }
    }
}