using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.Services.Builders;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Builders.Inferfaces;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;
using YGOProAnalyticsServer.Services.Analyzers;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Converters;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;
using YGOProAnalyticsServer.Services.Updaters;
using YGOProAnalyticsServer.Services.Unzippers;
using YGOProAnalyticsServer.Services.Unzippers.Interfaces;
using YGOProAnalyticsServer.Services.Others.Interfaces;
using YGOProAnalyticsServer.Services.Others;
using System.Reflection;
using MediatR;
using YGOProAnalyticsServer.Jobs;
using YGOProAnalyticsServer.Services.Factories.Interfaces;
using YGOProAnalyticsServer.Services.Factories;
using AutoMapper;
using YGOProAnalyticsServer.Services.Validators.Interfaces;
using YGOProAnalyticsServer.Services.Validators;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using YGOProAnalyticsServer.Middlewares;

namespace YGOProAnalyticsServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMemoryCache();
            services.AddMediatR();
            services.AddSingleton<IAdminConfig, AdminConfig>();
            var adminConfig = services.BuildServiceProvider().GetService<IAdminConfig>();
            services.AddDbContext<YgoProAnalyticsDatabase>(
                options => options
                            .UseSqlServer(YgoProAnalyticsDatabase.ConnectionString(adminConfig.DBUser, adminConfig.DBPassword))
                            .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
             );
            _addAutomapper(services);
            _addCors(services);
            _registerScopedServices(services);

            services.AddScheduler(builder =>
            {
                //builder.AddJobs(Assembly.GetExecutingAssembly());
                builder.AddJob<UpdatesJob>();
            });
        }

        private void _registerScopedServices(IServiceCollection services)
        {
            _registerAnalyzers(services);
            _registerBuilders(services);
            _registerConverters(services);
            _registerDownloaders(services);
            _registerFactories(services);
            _registerOthers(services);
            _registerUnzippers(services);
            _registerUpdaters(services);
            _registerValidators(services);
        }

        private void _registerValidators(IServiceCollection services)
        {
            services.AddScoped<IDateValidator, DateValidator>();
            services.AddScoped<IDecklistBrowserQueryParametersDtoValidator, DecklistBrowserQueryParametersDtoValidator>();
            services.AddScoped<IBanlistBrowserQueryParamsValidator, BanlistBrowserQueryParamsValidator>();
            services.AddScoped<IArchetypeBrowserQueryParamsValidator, ArchetypeBrowserQueryParamsValidator>();
        }

        private void _registerUpdaters(IServiceCollection services)
        {
            services.AddScoped<IBanlistDataToBanlistUpdater, BanlistDataToBanlistUpdater>();
            services.AddScoped<ICardsDataToCardsAndArchetypesUpdater, CardsDataToCardsAndArchetypesUpdater>();
            services.AddScoped<IServerActivityUpdater, ServerActivityUpdater>();
        }

        private void _registerUnzippers(IServiceCollection services)
        {
            services.AddScoped<IFileUnzipper, FileUnzipper>();
        }

        private void _registerOthers(IServiceCollection services)
        {
            services.AddScoped<IBanlistService, BanlistService>();
            services.AddScoped<IYgoProServerStatusService, YgoProServerStatusService>();
            services.AddScoped<IDecklistService, DecklistService>();
            services.AddScoped<IServerActivityStatisticsService, ServerActivityStatisticsService>();
            services.AddScoped<IArchetypeService, ArchetypeService>();
            services.AddScoped<INumberOfResultsHelper, NumberOfResultsHelper>();
        }

        private void _registerFactories(IServiceCollection services)
        {
            services.AddScoped<ICardDtosFactory, CardDtosFactory>();
            services.AddScoped<IDecksDtosFactory, DecksDtosFactory>();
        }

        private void _registerConverters(IServiceCollection services)
        {
            services.AddScoped<IBanlistToBanlistDTOConverter, BanlistToBanlistDTOConverter>();
            services.AddScoped<IDuelLogConverter, DuelLogConverter>();
            services.AddScoped<IYDKToDecklistConverter, YDKToDecklistConverter>();
            services.AddScoped<IDecklistToDecklistDtoConverter, DecklistToDecklistDtoConverter>();
            services.AddScoped<IArchetypeToDtoConverter, ArchetypeToDtoConverter>();
            services.AddScoped<IBetaCardToOfficialConverter, BetaCardToOfficialConverter>();
        }

        private void _registerBuilders(IServiceCollection services)
        {
            services.AddScoped<ICardBuilder, CardBuilder>();
        }

        private void _registerAnalyzers(IServiceCollection services)
        {
            services.AddScoped<IArchetypeAndDecklistAnalyzer, ArchetypeAndDecklistAnalyzer>();
            services.AddScoped<IDuelLogNameAnalyzer, DuelLogNameAnalyzer>();
        }

        private void _registerDownloaders(IServiceCollection services)
        {
            services.AddScoped<IBanlistDataDownloader, BanlistDataDownloader>();
            services.AddScoped<ICardsDataDownloader, CardsDataDownloader>();
            services.AddScoped<IFTPDownloader, FTPDownloader>();
            services.AddScoped<IYGOProServerRoomsDownloader, YGOProServerRoomsDownloader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.ConfigureExceptionHandler();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void _addAutomapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddMaps(Assembly.GetExecutingAssembly());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private void _addCors(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
               builder =>
               {
                   builder
                        .AllowAnyOrigin()
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
               }));
        }
    }
}
