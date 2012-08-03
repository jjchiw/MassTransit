using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit.SubscriptionConfigurators;
using TinyIoC;
using MassTransit.Util;
using MassTransit.Saga;
using Magnum.Reflection;

namespace MassTransit.TinyIocIntegration
{
	public class TinyIocSagaFactoryConfigurator
	{
		readonly SubscriptionBusServiceConfigurator _configurator;
		readonly TinyIoCContainer _container;

		public TinyIocSagaFactoryConfigurator(SubscriptionBusServiceConfigurator configurator, TinyIoCContainer container)
		{
			_container = container;
			_configurator = configurator;
		}

		public void ConfigureSaga(Type sagaType)
		{
			this.FastInvoke(new[] { sagaType }, "Configure");
		}

		[UsedImplicitly]
		public void Configure<T>()
			where T : class, ISaga
		{
			_configurator.Saga(_container.Resolve<ISagaRepository<T>>());
		}
	}
}
