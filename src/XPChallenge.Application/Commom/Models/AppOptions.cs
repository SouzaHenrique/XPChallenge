using XPChallenge.Application.Commom.Models.ApplicationMailSenderOptions;

namespace XPChallenge.Application.Commom.Models;
public class AppOptions
{


    /// <summary>
    /// Gets the name of the configuration section where these options can be found.
    /// </summary>
    public const string Section = "AppOptions";

    /// <summary>
    /// Gets or sets the mail sender options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppMailSenderOptions"/> containing mail sender options.</returns>
    public AppMailSenderOptions AppMailSenderOptions { get; set; }
}
