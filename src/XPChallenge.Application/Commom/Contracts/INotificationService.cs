namespace XPChallenge.Application.Commom.Contracts;
/// <summary>
/// This interface represents the contract for sending messages  email.
/// </summary>
public interface INotificationService
{
    #region Public Methods

    /// <summary>
    /// Sends an email message asynchronously.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="htmlMessage">The HTML body of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(string toEmail, string subject, string htmlMessage);

    #endregion Public Methods
}
