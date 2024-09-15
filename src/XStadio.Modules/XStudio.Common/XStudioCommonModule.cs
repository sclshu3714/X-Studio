using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.Autofac;
using Volo.Abp.EventBus.Kafka;
using Volo.Abp.Kafka;
using Volo.Abp.Modularity;

namespace XStudio.Common
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpEventBusKafkaModule)
    )]
    public class XStudioCommonModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //var configuration = context.Services.GetConfiguration();
            //var hostingEnvironment = context.Services.GetHostingEnvironment();
            //if (configuration.GetSection("Kafka") != null)
            //{
            //    ConfigureKafka(context, configuration, hostingEnvironment);
            //}
        }

        //private void ConfigureKafka(ServiceConfigurationContext context, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        //{
        //    var kafkaOptions = new AbpKafkaOptions();
        //    configuration.GetSection("Kafka:Connections").Bind(kafkaOptions.Connections);
        //    context.Services.Configure<AbpKafkaOptions>(options =>
        //    {
        //        foreach (var connection in options.Connections) {
        //            options.Connections.TryAdd(connection.Key, connection.Value);
        //        }
        //    });

        //    var kafkaEventBusOptions = new AbpKafkaEventBusOptions();
        //    configuration.GetSection("Kafka:EventBus").Bind(kafkaEventBusOptions);
        //    context.Services.Configure<AbpKafkaEventBusOptions>(options =>
        //    {
        //        options.GroupId = kafkaEventBusOptions.GroupId;
        //        options.TopicName = kafkaEventBusOptions.TopicName;
        //        options.ConnectionName = kafkaEventBusOptions.ConnectionName;
        //    });
        //}
    }
}
