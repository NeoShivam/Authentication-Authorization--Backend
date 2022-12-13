using BoilerPlate.Application.Responses;
using MediatR;

namespace BoilerPlate.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand: IRequest<Response<CreateCategoryDto>>
    {
        public string Name { get; set; }
    }
}
