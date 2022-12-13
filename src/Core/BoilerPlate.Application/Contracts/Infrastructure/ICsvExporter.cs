using BoilerPlate.Application.Features.Events.Queries.GetEventsExport;
using System.Collections.Generic;

namespace BoilerPlate.Application.Contracts.Infrastructure
{
    public interface ICsvExporter
    {
        byte[] ExportEventsToCsv(List<EventExportDto> eventExportDtos);
    }
}
