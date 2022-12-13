using BoilerPlate.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace BoilerPlate.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListQuery: IRequest<Response<IEnumerable<EventListVm>>>
    {

    }
}
