using BoilerPlate.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace BoilerPlate.Application.Features.Categories.Queries.GetCategoriesList
{
    public class GetCategoriesListQuery : IRequest<Response<IEnumerable<CategoryListVm>>>
    {
    }
}
