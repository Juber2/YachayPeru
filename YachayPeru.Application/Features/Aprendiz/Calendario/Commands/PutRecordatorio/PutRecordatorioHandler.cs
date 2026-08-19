using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Aprendiz;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Calendario.Commands.PutRecordatorio
{
    public class PutRecordatorioHandler : IRequestHandler<PutRecordatorioCommand, Result>
    {
        private readonly IFestividadRepository festividadRepository;
        private readonly IFestividadReminderRepository reminderRepository;
        private readonly IUnitOfWork unitOfWork;

        public PutRecordatorioHandler(
            IFestividadRepository _festividadRepository,
            IFestividadReminderRepository _reminderRepository,
            IUnitOfWork _unitOfWork)
        {
            festividadRepository = _festividadRepository;
            reminderRepository = _reminderRepository;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result> Handle(PutRecordatorioCommand request, CancellationToken ct)
        {
            var festividad = await festividadRepository.GetByIdAsync(request.FestividadId, ct);
            if (festividad is null)
                return Result.Failure("Festividad no encontrada.", NotFound);

            var reminder = await reminderRepository.GetByUserAndFestividadAsync(request.UserId, request.FestividadId, ct);
            if (reminder is null)
            {
                reminder = new FestividadReminder
                {
                    UserId = request.UserId,
                    FestividadId = request.FestividadId,
                    Enabled = request.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId
                };
                await reminderRepository.AddAsync(reminder, ct);
            }
            else
            {
                reminder.Enabled = request.Enabled;
                reminder.UpdatedAt = DateTime.UtcNow;
                reminder.UpdatedBy = request.UserId;
                reminderRepository.Update(reminder);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
