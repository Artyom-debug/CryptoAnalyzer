using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Domain.Constants;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<IReportApiClient, ReportApiClient>((HttpClient client) =>
        {
            client.BaseAddress = new Uri("");
            client.Timeout = TimeSpan.FromSeconds(10);
        }).AddTransientHttpErrorPolicy(policy =>
            policy.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromSeconds(1)
            })
        );

        var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        Guard.Against.Null(connectionString, message: "Connection string 'ConnectionString' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString); 
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>()); //регистрация сервиса бд (используем лямбда-выражение, чтобы получить всю конфигурацию бд)

        builder.Services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanViewReports, policy => policy.RequireRole(Roles.Administrator)));
    }
}
