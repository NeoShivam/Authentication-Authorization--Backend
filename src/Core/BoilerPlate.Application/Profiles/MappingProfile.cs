using AutoMapper;
using BoilerPlate.Application.Features.Categories.Commands.CreateCategory;
using BoilerPlate.Application.Features.Categories.Commands.StoredProcedure;
using BoilerPlate.Application.Features.Categories.Queries.GetCategoriesList;
using BoilerPlate.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using BoilerPlate.Application.Features.Events.Commands.CreateEvent;
using BoilerPlate.Application.Features.Events.Commands.Transaction;
using BoilerPlate.Application.Features.Events.Commands.UpdateEvent;
using BoilerPlate.Application.Features.Events.Queries.GetEventDetail;
using BoilerPlate.Application.Features.Events.Queries.GetEventsExport;
using BoilerPlate.Application.Features.Events.Queries.GetEventsList;
using BoilerPlate.Application.Features.Orders.GetOrdersForMonth;
using BoilerPlate.Domain.Entities;

namespace BoilerPlate.Application.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {          
            CreateMap<Event, CreateEventCommand>().ReverseMap();
            CreateMap<Event, TransactionCommand>().ReverseMap();
            CreateMap<Event, UpdateEventCommand>().ReverseMap();
            CreateMap<Event, EventDetailVm>().ReverseMap();
            CreateMap<Event, CategoryEventDto>().ReverseMap();
            CreateMap<Event, EventExportDto>().ReverseMap();

            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryListVm>();
            CreateMap<Category, CategoryEventListVm>();
            CreateMap<Category, CreateCategoryCommand>();
            CreateMap<Category, CreateCategoryDto>();
            CreateMap<Category, StoredProcedureCommand>();
            CreateMap<Category, StoredProcedureDto>();

            CreateMap<Order, OrdersForMonthDto>();

            CreateMap<Event, EventListVm>().ConvertUsing<EventVmCustomMapper>();
        }
    }
}
