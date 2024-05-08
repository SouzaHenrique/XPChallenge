using Microsoft.Extensions.Options;
using XPChallenge.Application.Commom.Contracts;
using XPChallenge.Application.Commom.Models;
using XPChallenge.Application.Commom.Models.ApplicationMailSenderOptions;

namespace XPChallenge.Infrastructure.Services;
public class AppOptionsService(IOptionsSnapshot<AppOptions> appOptionsSnapshot) : IAppOptionsService
{
    private readonly AppOptions _appOptionsSnapshot = appOptionsSnapshot.Value;

    public AppMailSenderOptions GetAppMailSenderOptions()
    {
        return _appOptionsSnapshot.AppMailSenderOptions;
    }
}
