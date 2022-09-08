using Microsoft.AspNetCore.Mvc;

namespace Users.API.Contracts.v1;

[ApiVersion("1", Deprecated = false)]
public static class Routes
{
    private const string Root = "user-api";

    private const string Version = "v{version:apiVersion}";

    private const string Base = $"{Root}/{Version}";

    public static class Vehicles
    {
        public const string Controller = $"{Base}/user";
    }
}