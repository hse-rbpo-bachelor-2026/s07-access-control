namespace TrackerApi;

// Доменная модель учебного "Внутреннего трекера обращений".
// Хранилище в памяти: объекту С07 не нужна БД, дефекты - в логике доступа,
// а не в данных. Аутентификация не моделируется: текущий пользователь задается
// заголовком X-User, а его роль и подразделение берутся из справочника Users
// по логину (упрощение объекта, не дефект; аутентификация - тема Л07,
// авторизация - предмет этой лабораторной).

public enum Role { Requester, Resolver, Manager }

public record User(string Login, Role Role, string Department);

public record Attachment(int Id, int IssueId, string FileName, string Content);

public class Issue
{
    public int Id { get; init; }
    public required string Author { get; init; }        // логин заявителя
    public required string Department { get; init; }    // подразделение обращения
    public required string Title { get; set; }
    public required string Body { get; set; }
    public string Status { get; set; } = "open";        // open | closed
}

// Справочник пользователей и начальные данные.
public static class Seed
{
    public static readonly Dictionary<string, User> Users = new()
    {
        ["alice"] = new("alice", Role.Requester, "sales"),
        ["bob"]   = new("bob",   Role.Requester, "sales"),
        ["carol"] = new("carol", Role.Resolver,  "sales"),
        ["dave"]  = new("dave",  Role.Resolver,  "logistics"),
        ["mgr"]   = new("mgr",   Role.Manager,   "sales"),
    };

    public static (List<Issue>, List<Attachment>) Build()
    {
        var issues = new List<Issue>
        {
            new() { Id = 1, Author = "alice", Department = "sales",     Title = "Laptop broken",   Body = "screen flicker" },
            new() { Id = 2, Author = "bob",   Department = "sales",     Title = "VPN access",      Body = "cannot connect" },
            new() { Id = 3, Author = "alice", Department = "sales",     Title = "Salary question", Body = "sensitive: pay slip mismatch" },
            new() { Id = 4, Author = "dave",  Department = "logistics", Title = "Scanner issue",   Body = "logistics only" },
        };
        var attachments = new List<Attachment>
        {
            new(10, 3, "payslip.txt", "CONFIDENTIAL payslip content for alice"),
            new(11, 2, "vpn-log.txt", "vpn connection log for bob"),
        };
        return (issues, attachments);
    }
}

public sealed class TrackerStore
{
    public List<Issue> Issues { get; }
    public List<Attachment> Attachments { get; }

    public TrackerStore()
    {
        (Issues, Attachments) = Seed.Build();
    }

    public Issue? FindIssue(int id) => Issues.FirstOrDefault(i => i.Id == id);
    public Attachment? FindAttachment(int id) => Attachments.FirstOrDefault(a => a.Id == id);
    public User? FindUser(string login) => Seed.Users.GetValueOrDefault(login);
}
