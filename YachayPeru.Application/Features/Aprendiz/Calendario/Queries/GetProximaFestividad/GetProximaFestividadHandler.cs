using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.Aprendiz.Calendario.Queries.GetProximaFestividad
{
    public class GetProximaFestividadHandler : IRequestHandler<GetProximaFestividadQuery, AprendizProximaFestividadItem?>
    {
        private readonly IFestividadRepository festividadRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IFestividadReminderRepository reminderRepository;

        public GetProximaFestividadHandler(
            IFestividadRepository _festividadRepository,
            ICourseRepository _courseRepository,
            IFestividadReminderRepository _reminderRepository)
        {
            festividadRepository = _festividadRepository;
            courseRepository = _courseRepository;
            reminderRepository = _reminderRepository;
        }

        public async Task<AprendizProximaFestividadItem?> Handle(GetProximaFestividadQuery request, CancellationToken ct)
        {
            var festividades = await festividadRepository.ListAsync(f => f.IsActive, ct);
            if (festividades.Count == 0) return null;

            var today = DateTime.UtcNow.Date;

            var proxima = festividades
                .Select(f => new { Festividad = f, NextDate = NextOccurrence(today, f.Month, f.Day) })
                .OrderBy(x => x.NextDate)
                .ThenBy(x => x.Festividad.Id)
                .First();

            var region = await courseRepository.GetByIdAsync(proxima.Festividad.CourseId, ct);
            var reminders = await reminderRepository.GetByUserAsync(request.UserId, ct);
            var isReminderOn = reminders.Any(r => r.FestividadId == proxima.Festividad.Id && r.Enabled);

            return new AprendizProximaFestividadItem
            {
                Id = proxima.Festividad.Id,
                Name = proxima.Festividad.Name,
                Description = proxima.Festividad.Description,
                RegionId = proxima.Festividad.CourseId,
                RegionTitle = region?.Title ?? string.Empty,
                Month = proxima.Festividad.Month,
                Day = proxima.Festividad.Day,
                DaysUntil = (proxima.NextDate - today).Days,
                IsReminderOn = isReminderOn
            };
        }

        /// <summary>Próxima fecha en que cae Month/Day desde hoy (este año si no pasó, si no el que viene).</summary>
        private static DateTime NextOccurrence(DateTime today, int month, int day)
        {
            var daysInMonth = DateTime.DaysInMonth(today.Year, month);
            var safeDay = Math.Min(day, daysInMonth);
            var candidate = new DateTime(today.Year, month, safeDay);

            if (candidate < today)
            {
                daysInMonth = DateTime.DaysInMonth(today.Year + 1, month);
                safeDay = Math.Min(day, daysInMonth);
                candidate = new DateTime(today.Year + 1, month, safeDay);
            }

            return candidate;
        }
    }
}
