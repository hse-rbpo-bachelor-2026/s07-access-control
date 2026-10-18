namespace TrackerApi.Reserve;

// Резервный тренировочный фрагмент (не подключен к приложению).
//
// ВНИМАНИЕ (учебный объект): НАМЕРЕННЫЙ ДЕФЕКТ того же класса, что REVIEW-1 в
// основном объекте, в другом контексте - выдача комментария по его id.
// REVIEW-R1: рассмотрите, что проверяется перед выдачей комментария и как это
// связано с правами на родительское обращение. Пройдите полный цикл, если
// фрагмент вашего проекта еще не готов к переносу.

public record Comment(int Id, int IssueId, string Author, string Text);

public sealed class ReserveCommentStore
{
    private readonly List<Comment> _comments =
    [
        new(100, 1, "alice", "own comment"),
        new(101, 4, "dave",  "logistics-only comment"),
    ];

    // REVIEW-R1: возвращает комментарий по id; рассмотрите проверки прав.
    public Comment? Get(int commentId) =>
        _comments.FirstOrDefault(c => c.Id == commentId);
}
