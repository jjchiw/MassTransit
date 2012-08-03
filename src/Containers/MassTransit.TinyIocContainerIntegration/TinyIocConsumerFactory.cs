using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyIoC;
using MassTransit.Pipeline;
using MassTransit.Exceptions;

namespace MassTransit.TinyIocIntegration
{
	public class TinyIocConsumerFactory<T> :
		IConsumerFactory<T>
		where T : class
	{
		readonly TinyIoCContainer _container;

		public TinyIocConsumerFactory(TinyIoCContainer container)
		{
			_container = container;
		}

		public IEnumerable<Action<IConsumeContext<TMessage>>> GetConsumer<TMessage>(
			IConsumeContext<TMessage> context, InstanceHandlerSelector<T, TMessage> selector)
			where TMessage : class
		{
			var consumer = _container.Resolve<T>();
			if (consumer == null)
				throw new ConfigurationException(string.Format("Unable to resolve type '{0}' from container: ", typeof(T)));

			foreach (var handler in selector(consumer, context))
			{
				yield return handler;
			}
		}
	}
}
