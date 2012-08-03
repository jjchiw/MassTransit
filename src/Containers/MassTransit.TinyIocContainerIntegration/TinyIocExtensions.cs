using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit.SubscriptionConfigurators;
using TinyIoC;
using MassTransit.Saga;
using MassTransit.Saga.SubscriptionConfigurators;
using Magnum.Extensions;

namespace MassTransit.TinyIocIntegration
{
	public static class TinyIocMapExtensions
	{
		public static void LoadFrom(this SubscriptionBusServiceConfigurator configurator, TinyIoCContainer container)
		{
			IList<Type> concreteTypes = FindTypes<IConsumer>(container, x => !x.Implements<ISaga>());
			if (concreteTypes.Count > 0)
			{
				var consumerConfigurator = new TinyIocConsumerFactoryConfigurator(configurator, container);

				foreach (Type concreteType in concreteTypes)
				{
					consumerConfigurator.ConfigureConsumer(concreteType);
				}
			}

			IList<Type> sagaTypes = FindTypes<ISaga>(container, x => true);
			if (sagaTypes.Count > 0)
			{
				var sagaConfigurator = new TinyIocSagaFactoryConfigurator(configurator, container);

				foreach (Type type in sagaTypes)
				{
					sagaConfigurator.ConfigureSaga(type);
				}
			}
		}

		public static ConsumerSubscriptionConfigurator<TConsumer> Consumer<TConsumer>(
			this SubscriptionBusServiceConfigurator configurator, TinyIoCContainer kernel)
			where TConsumer : class, IConsumer
		{
			var consumerFactory = new TinyIocConsumerFactory<TConsumer>(kernel);

			return configurator.Consumer(consumerFactory);
		}

		public static SagaSubscriptionConfigurator<TSaga> Saga<TSaga>(
			this SubscriptionBusServiceConfigurator configurator, TinyIoCContainer kernel)
			where TSaga : class, ISaga
		{
			return configurator.Saga(kernel.Resolve<ISagaRepository<TSaga>>());
		}

		static IList<Type> FindTypes<T>(TinyIoCContainer container, Func<Type, bool> filter) where T : class
		{
			return container.ResolveAll<T>().Select(x => x.GetType()).ToList();


			//return container.GetRegisteredTypes().Where(x => x )
			//    .Model
			//    .PluginTypes
			//    .Where(x => x.PluginType.Implements<T>())
			//    .Select(i => i.PluginType)
			//    .Concat(container.Model.InstancesOf<T>().Select(x => x.ConcreteType))
			//    .Where(filter)
			//    .Distinct()
			//    .ToList();
		}
	}
}
