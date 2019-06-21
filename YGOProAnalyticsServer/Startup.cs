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
using YGOProAnalyticsServer.Extensions;
using System.Reflection;
using MediatR;
using YGOProAnalyticsServer.Jobs;
using YGOProAnalyticsServer.Services.Factories.Interfaces;
using YGOProAnalyticsServer.Services.Factories;
using AutoMapper;
using System.Reflection;
using YGOProAnalyticsServer.Services.Validators.Interfaces;
using YGOProAnalyticsServer.Services.Validators;

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
            services.AddDbContext<YgoProAnalyticsDatabase>(options => options.UseSqlServer(YgoProAnalyticsDatabase.connectionString));
            _addAutomapper(services);
            _addCors(services);
            services.AddScoped<ICardBuilder, CardBuilder>();
            services.AddScoped<IBanlistDataDownloader, BanlistDataDownloader>();
            services.AddScoped<ICardsDataDownloader, CardsDataDownloader>();
            services.AddScoped<IDuelLogNameAnalyzer, DuelLogNameAnalyzer>();
            services.AddScoped<IDuelLogConverter, DuelLogConverter>();
            services.AddScoped<IBanlistDataToBanlistUpdater, BanlistDataToBanlistUpdater>();
            services.AddScoped<IFTPDownloader, FTPDownloader>();
            services.AddScoped<IFileUnzipper, FileUnzipper>();
            services.AddScoped<IYGOProServerRoomsDownloader, YGOProServerRoomsDownloader>();
            services.AddScoped<IYgoProServerStatusService, YgoProServerStatusService>();
            services.AddScoped<ICardsDataToCardsAndArchetypesUpdater, CardsDataToCardsAndArchetypesUpdater>();
            services.AddScoped<IArchetypeAndDecklistAnalyzer, ArchetypeAndDecklistAnalyzer>();
            services.AddScoped<IYDKToDecklistConverter, YDKToDecklistConverter>();
            services.AddScoped<ICardDtosFactory, CardDtosFactory>();
            services.AddScoped<IDecksDtosFactory, DecksDtosFactory>();
            services.AddScoped<IDecklistToDecklistDtoConverter, DecklistToDecklistDtoConverter>();
            services.AddScoped<IDecklistService, DecklistService>();
            services.AddScoped<IServerActivityStatisticsService, ServerActivityStatisticsService>();
            services.AddScoped<IBanlistService, BanlistService>();
            services.AddScoped<IDateValidator, DateValidator>();
            services.AddScoped<IDecklistBrowserQueryParametersDtoValidator, DecklistBrowserQueryParametersDtoValidator>();
            services.AddScoped<IArchetypeService, ArchetypeService>();
            services.AddScoped<INumberOfResultsHelper, NumberOfResultsHelper>();
            services.AddScoped<IBanlistBrowserQueryParamsValidator, BanlistBrowserQueryParamsValidator>();
            services.AddScoped<IBanlistToBanlistDTOConverter, BanlistToBanlistDTOConverter>();
            services.AddScoped<IArchetypeBrowserQueryParamsValidator, ArchetypeBrowserQueryParamsValidator>();
            services.AddScoped<IArchetypeToDtoConverter, ArchetypeToDtoConverter>();
            services.AddScoped<IServerActivityUpdater, ServerActivityUpdater>();

            services.AddSingleton<IAdminConfig, AdminConfig>();

            services.AddScheduler(builder =>
            {
                //builder.AddJobs(Assembly.GetExecutingAssembly());
                builder.AddJob<UpdatesJob>();
            });
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
                        //.WithOrigins("http://localhost:4200")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
               }));
        }
    }
}
