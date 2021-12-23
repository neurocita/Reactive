using NATS.Client;
using Neurocita.Reactive.Transport;
using System;
using System.Reflection;

namespace Neurocita.Reactive.Configuration
{
    public class NatsTransportConfiguration : INatsCanAddUrlOrOptions, INatsCanAddSecureOrUserCredentials
    {
        private readonly ICanAddTransport canAddTransport;
        private Options options = ConnectionFactory.GetDefaultOptions();

        internal NatsTransportConfiguration(ICanAddTransport canAddTransport)
        {
            this.canAddTransport = canAddTransport;
        }

        public INatsCanAddSecureOrUserCredentials WithUrl(string url)
        {
            //options.Url = url;
            Type type = typeof(Options);
            type.GetMethod("processUrlString", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(options, new object[] { url });
            return this;
        }

        public ICanAddSerializer Secure(bool secure)
        {
            options.Secure = secure;
            return canAddTransport.WithTransport(new NatsTransportFactory(options));
        }

        public ICanAddSerializer WithUserCredentials(string credentialsPath)
        {
            options.SetUserCredentials(credentialsPath);
            return canAddTransport.WithTransport(new NatsTransportFactory(options));
        }

        public ICanAddSerializer WithUserCredentials(string jwt, string privateNkey)
        {
            options.SetUserCredentials(jwt, privateNkey);
            return canAddTransport.WithTransport(new NatsTransportFactory(options));
        }

        public ICanAddSerializer WithOptions(Options options)
        {
            this.options = options;
            return canAddTransport.WithTransport(new NatsTransportFactory(options));
        }
    }
}