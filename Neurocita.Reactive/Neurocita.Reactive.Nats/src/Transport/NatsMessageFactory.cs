using NATS.Client;
using System;
using System.Reactive.Disposables;
using System.Reflection;

namespace Neurocita.Reactive.Nats
{
    public class NatsMessageFactory : ITransportMessageFactory
    {
        private readonly Options options;
        private readonly NatsConnectionManager connectionManager;

        public NatsMessageFactory()
            : this(ConnectionFactory.GetDefaultOptions())
        {
            
        }

        public NatsMessageFactory(string url, bool secure = false)
            : this()
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Invalid url", nameof(url));

            //options.Url = url;
            Type type = typeof(Options);
            type.GetMethod("processUrlString", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(options, new object[] { url });
            options.Secure = secure;
        }

        public NatsMessageFactory(string url, string credentialsPath)
            : this(url)
        {
            if (string.IsNullOrWhiteSpace(credentialsPath))
                throw new ArgumentException("Invalid credentials path", nameof(credentialsPath));

            options.SetUserCredentials(credentialsPath);
        }

        public NatsMessageFactory(string url, string jwt, string privateNkey)
            : this(url)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                throw new ArgumentException("Invalid jwt path", nameof(jwt));
            if (string.IsNullOrWhiteSpace(privateNkey))
                throw new ArgumentException("Invalid nkey path", nameof(privateNkey));

            options.SetUserCredentials(jwt, privateNkey);
        }

        public NatsMessageFactory(Options options)
        {
            this.options = options;
            connectionManager = new NatsConnectionManager(options);
        }

        public ITransportMessageSource CreateSource(string source)
        {
            return new NatsMessageSource(connectionManager, source);
        }

        public ITransportMessageSink CreateSink(string destination)
        {
            return new NatsMessageSink(connectionManager, destination);
        }
    }
}
