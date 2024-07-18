using Core.Interfaces;
using Core.Time;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Extensions
{
    public static class ApplicationsServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            services.AddEndpointsApiExplorer();

            services.AddMemoryCache();
            //services.AddSingleton<ITimeProvider, SystemTimeProvider>();
            services.AddSingleton<ITimeProvider, TestTimeProvider>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBaggageRepository, BaggageRepository>();
            services.AddScoped<IFlightRepository, FlightRepository>();
            services.AddScoped<IOtherFlightRepository, OtherFlightRepository>();
            services.AddScoped<IBaseFlightRepository, BaseFlightRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<IInfantRepository, InfantRepository>();
            services.AddScoped<IDestinationRepository, DestinationRepository>();
            services.AddScoped<ISSRCodeRepository, SSRCodeRepository>();
            services.AddScoped<ISpecialServiceRequestRepository, SpecialServiceRequestRepository>();
            services.AddScoped<IAPISDataRepository, APISDataRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPredefinedCommentRepository, PredefinedCommentRepository>();
            services.AddScoped<IPassengerBookingDetailsRepository, PassengerBookingDetailsRepository>();
            services.AddScoped<IBasePassengerOrItemRepository, BasePassengerOrItemRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IPassengerFlightRepository, PassengerFlightRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IActionHistoryRepository, ActionHistoryRepository>();
            
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPassengerHistoryService, PassengerHistoryService>();
            services.AddScoped<IPassengerDtoMappingService, PassengerDtoMappingService>();
            services.AddScoped<ITokenService, TokenService>();
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value is { Errors.Count: > 0 })
                        .SelectMany(x =>
                        {
                            if (x.Value != null) return x.Value.Errors;
                            throw new InvalidOperationException();
                        })
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse(errors);

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}
