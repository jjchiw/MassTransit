using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit.SubscriptionConfigurators;
using TinyIoC;
using MassTransit.Util;
using Magnum.Reflection;

namespace MassTransit.TinyIocIntegration
{
	public class TinyIocConsumerFactoryConfigurator
	{
		readonly SubscriptionBusServiceConfigurator _configurator;
		readonly TinyIoCContainer _container;

		public TinyIocConsumerFactoryConfigurator(SubscriptionBusServiceConfigurator configurator, TinyIoCContainer container)
		{
			_container = container;
			_configurator = configurator;
		}

		public void ConfigureConsumer(Type messageType)
		{
			this.FastInvoke(new[] {messageType}, "Configure");
		}

		[UsedImplicitly]
		public void Configure<T>()
			where T : class, IConsumer
		{
			_configurator.Consumer(new TinyIocConsumerFactory<T>(_container));
		}
	}
}
