using NATS.Client;
using System;
using System.Reactive.Disposables;
using System.Reflection;

namespace Neurocita.Reactive.Transport
{
    public class NatsTransportFactory : ITransportFactory
    {
        private readonly Options options;

        public NatsTransportFactory()
            : this(ConnectionFactory.GetDefaultOptions())
        {
            
        }

        public NatsTransportFactory(string url, bool secure = false)
            : this()
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Invalid url", nameof(url));

            //options.Url = url;
            Type type = typeof(Options);
            type.GetMethod("processUrlString", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(options, new object[] { url });
            options.Secure = secure;
        }

        public NatsTransportFactory(string url, string credentialsPath)
            : this(url)
        {
            if (string.IsNullOrWhiteSpace(credentialsPath))
                throw new ArgumentException("Invalid credentials path", nameof(credentialsPath));

            options.SetUserCredentials(credentialsPath);
        }

        public NatsTransportFactory(string url, string jwt, string privateNkey)
            : this(url)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                throw new ArgumentException("Invalid jwt path", nameof(jwt));
            if (string.IsNullOrWhiteSpace(privateNkey))
                throw new ArgumentException("Invalid nkey path", nameof(privateNkey));

            options.SetUserCredentials(jwt, privateNkey);
        }

        public NatsTransportFactory(Options options)
        {
            this.options = options;
        }

        public ITransport Create()
        {
            return new NatsTransport(options);
        }
    }
}
