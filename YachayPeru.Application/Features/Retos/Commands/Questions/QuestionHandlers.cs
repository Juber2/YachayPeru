using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.Questions
{
    public class AddRetoQuestionHandler : IRequestHandler<AddRetoQuestionCommand, Result<int>>
    {
        private readonly RetoActions retoActions;
        public AddRetoQuestionHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result<int>> Handle(AddRetoQuestionCommand request, CancellationToken ct)
            => retoActions.AddQuestion(new AddRetoQuestionInput
            {
                RetoId = request.RetoId,
                QuestionTypeCode = request.QuestionTypeCode,
                QuestionText = request.QuestionText,
                Points = request.Points,
                Choices = request.Choices.Select(c => new QuestionChoiceInput { Text = c.Text, IsCorrect = c.IsCorrect, OrderIndex = c.OrderIndex }).ToList(),
                Blanks = request.Blanks.Select(b => new QuestionBlankInput { BlankIndex = b.BlankIndex, CorrectAnswer = b.CorrectAnswer, OrderIndex = b.OrderIndex }).ToList()
            }, ct);
    }

    public class EditRetoQuestionHandler : IRequestHandler<EditRetoQuestionCommand, Result<int>>
    {
        private readonly RetoActions retoActions;
        public EditRetoQuestionHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result<int>> Handle(EditRetoQuestionCommand request, CancellationToken ct)
            => retoActions.EditQuestion(new EditRetoQuestionInput
            {
                QuestionId = request.QuestionId,
                QuestionText = request.QuestionText,
                Points = request.Points,
                Choices = request.Choices.Select(c => new QuestionChoiceInput { Text = c.Text, IsCorrect = c.IsCorrect, OrderIndex = c.OrderIndex }).ToList(),
                Blanks = request.Blanks.Select(b => new QuestionBlankInput { BlankIndex = b.BlankIndex, CorrectAnswer = b.CorrectAnswer, OrderIndex = b.OrderIndex }).ToList()
            }, ct);
    }

    public class DeleteRetoQuestionHandler : IRequestHandler<DeleteRetoQuestionCommand, Result>
    {
        private readonly RetoActions retoActions;
        public DeleteRetoQuestionHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result> Handle(DeleteRetoQuestionCommand request, CancellationToken ct)
            => retoActions.DeleteQuestion(request.QuestionId, ct);
    }

    public class ReorderRetoQuestionsHandler : IRequestHandler<ReorderRetoQuestionsCommand, Result>
    {
        private readonly RetoActions retoActions;
        public ReorderRetoQuestionsHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result> Handle(ReorderRetoQuestionsCommand request, CancellationToken ct)
            => retoActions.ReorderQuestions(new ReorderQuestionsInput
            {
                RetoId = request.RetoId,
                Items = request.Items.Select(i => new QuestionOrderItem { QuestionId = i.QuestionId, OrderIndex = i.OrderIndex }).ToList()
            }, ct);
    }
}
