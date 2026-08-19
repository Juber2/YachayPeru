using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.Aprendiz.Calendario.Queries.GetFestividades
{
    public class GetFestividadesHandler : IRequestHandler<GetFestividadesQuery, IReadOnlyList<AprendizFestividadListItem>>
    {
        private readonly IFestividadRepository festividadRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IFestividadReminderRepository reminderRepository;

        public GetFestividadesHandler(
            IFestividadRepository _festividadRepository,
            ICourseRepository _courseRepository,
            IFestividadReminderRepository _reminderRepository)
        {
            festividadRepository = _festividadRepository;
            courseRepository = _courseRepository;
            reminderRepository = _reminderRepository;
        }

        public async Task<IReadOnlyList<AprendizFestividadListItem>> Handle(GetFestividadesQuery request, CancellationToken ct)
        {
            var festividades = await festividadRepository.ListAsync(f => f.IsActive, ct);
            var reminders = await reminderRepository.GetByUserAsync(request.UserId, ct);
            var enabledIds = reminders.Where(r => r.Enabled).Select(r => r.FestividadId).ToHashSet();

            var items = new List<AprendizFestividadListItem>();
            foreach (var festividad in festividades)
            {
                var region = await courseRepository.GetByIdAsync(festividad.CourseId, ct);

                items.Add(new AprendizFestividadListItem
                {
                    Id = festividad.Id,
                    Name = festividad.Name,
                    Description = festividad.Description,
                    RegionId = festividad.CourseId,
                    RegionTitle = region?.Title ?? string.Empty,
                    Month = festividad.Month,
                    Day = festividad.Day,
                    IsReminderOn = enabledIds.Contains(festividad.Id)
                });
            }

            return items;
        }
    }
}
