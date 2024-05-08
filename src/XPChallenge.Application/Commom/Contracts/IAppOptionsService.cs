using XPChallenge.Application.Commom.Models.ApplicationMailSenderOptions;

namespace XPChallenge.Application.Commom.Contracts;
/// <summary>
/// This interface represents the contract for accessing to the global configuration settings for the application stored in appsettings.json.
/// </summary>
public interface IAppOptionsService
{
    #region Public Methods

    /// <summary>
    /// Retrieves the options related to the Simple Mail Transfer Protocol (SMTP) configuration.
    /// </summary>
    /// <returns>An instance of <see cref="AppMailSenderOptions"/> containing SMTP configurations.</returns>
    AppMailSenderOptions GetAppMailSenderOptions();

    #endregion Public Methods
}
