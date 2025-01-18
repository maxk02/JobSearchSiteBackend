namespace Core.Domains._Shared.Email;

public interface IEmailModel
{
    public string Subject { get; }
    public string Content { get; }
}