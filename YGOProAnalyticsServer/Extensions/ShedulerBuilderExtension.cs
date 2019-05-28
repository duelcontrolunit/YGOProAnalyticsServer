using CronScheduler.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Extensions
{
    public static class SchedulerBuilderExtension
    {
        public static void AddJobs(this SchedulerBuilder builder, params Assembly[] assembliesToScan)
        {

            foreach (var assembly in assembliesToScan)
            {
                var jobs = assembly
                    .GetTypes()
                    .Where(x =>
                        x.GetInterfaces().Contains(typeof(IScheduledJob))
                        && !x.IsAbstract
                        && !x.IsInterface
                        && x.IsClass
                     )
                    .ToArray();

                _registerJobs(builder, jobs);
            }
        }

        private static void _registerJobs(SchedulerBuilder builder, Type[] jobs)
        {
            Type[] types = new Type[] { };
            MethodInfo method = typeof(SchedulerBuilder).GetMethod(nameof(SchedulerBuilder.AddJob), types);
            foreach (Type job in jobs)
            {
                MethodInfo generic = method.MakeGenericMethod(job);
                generic.Invoke(builder, null);
            }
        }
    }
}
