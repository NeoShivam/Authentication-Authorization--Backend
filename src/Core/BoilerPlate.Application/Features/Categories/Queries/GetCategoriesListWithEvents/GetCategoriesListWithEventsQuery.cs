using BoilerPlate.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace BoilerPlate.Application.Features.Categories.Queries.GetCategoriesListWithEvents
{
    public class GetCategoriesListWithEventsQuery: IRequest<Response<IEnumerable<CategoryEventListVm>>>
    {
        public bool IncludeHistory { get; set; }
    }
}
