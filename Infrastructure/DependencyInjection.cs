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
using Quartz;
using Microsoft.EntityFrameworkCore.Internal;
using Infrastructure.Repository;
using Infrastructure.Auth;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        //configuring http client to python service
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

        //configuring background service with Quartz
        builder.Services.AddQuartz(q =>
        {
            var jobKey = new JobKey("BackgroundCreatingReportService");
            q.AddJob<BackgroundCreatingReportService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("BackgroundCreatingReportService-timeframe-1h")
                .UsingJobData("timeframe", "1h")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever()));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("GenerateReports_1hour")
                .UsingJobData("timeframe", "1d")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever()));
        });

        //configuring database
        var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        Guard.Against.Null(connectionString, message: "Connection string 'ConnectionString' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString); 
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        //configuring Identity
        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 4;

            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        //builder.Services.AddAuthorization(options =>
        //    options.AddPolicy(Policies.CanViewReports, policy => policy.RequireRole(Roles.Administrator)));

        //configuring auth services
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        var jwtOptions = builder.Configuration.GetSection("JwtConfig").Get<JwtOptions>()
                ?? throw new InvalidOperationException("JwtConfig section is missing.");
        builder.Services.AddSingleton(jwtOptions);

        //configuring email sender
        builder.Services.AddTransient<IEmailSender, EmailSender>();
        builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("AuthMessageSenderOptions"));
    }
}
