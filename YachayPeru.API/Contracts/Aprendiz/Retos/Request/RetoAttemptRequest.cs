namespace YachayPeru.API.Contracts.Aprendiz.Retos.Request
{
    public class RetoAttemptRequest
    {
        public List<RetoAnswerRequest> Answers { get; set; } = [];
    }

    public class RetoAnswerRequest
    {
        public int QuestionId { get; set; }
        public List<int>? SelectedChoiceIds { get; set; }
        public List<string>? BlankAnswers { get; set; }
    }
}
