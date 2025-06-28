namespace EndPoint.Repositories
{
    public interface IMessageSender
    {
        public Task sendEmailAsync(string Toemail, string subject, string message, bool isMessageHtml = false);
    }
}
