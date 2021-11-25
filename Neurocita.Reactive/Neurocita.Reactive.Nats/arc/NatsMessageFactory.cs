using NATS.Client;
using System;
using System.Reflection;

namespace Neurocita.Reactive.Nats
{
    internal class NatsMessageFactory : ITransportMessageFactory
    {
        private Options options = ConnectionFactory.GetDefaultOptions();
        //private readonly Lazy<IConnection> connection;

        public NatsMessageFactory()
        {
            //connection = new Lazy<IConnection>(() => new ConnectionFactory().CreateConnection(options));
        }

        public NatsMessageFactory(string url, bool secure = false)
        {
            //options.Url = url;
            Type type = typeof(Options);
            type.GetMethod("processUrlString", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(options, new object[] { url });
            options.Secure = secure;
            //connection = new Lazy<IConnection>(() => new ConnectionFactory().CreateConnection(options));
        }

        public NatsMessageFactory(string url, string credentialsPath)
            : this(url)
        {
            if (string.IsNullOrWhiteSpace(credentialsPath))
                throw new ArgumentException("Invalid credentials path", "credentials");

            options.SetUserCredentials(credentialsPath);
        }

        public NatsMessageFactory(string url, string jwt, string privateNkey)
            : this(url)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                throw new ArgumentException("Invalid jwt path", "jwt");
            if (string.IsNullOrWhiteSpace(privateNkey))
                throw new ArgumentException("Invalid nkey path", "privateNkey");

            options.SetUserCredentials(jwt, privateNkey);
        }

        public NatsMessageFactory(Options options)
        {
            this.options = options;
            //connection = new Lazy<IConnection>(() => new ConnectionFactory().CreateConnection(options));
        }

        public ITransportMessageSink CreateSink(string node)
        {
            return new NatsMessageSink(CreateConnection(), node);
        }

        public ITransportMessageSource CreateSource(string node)
        {
            return new NatsMessageSource(CreateConnection(), node); ;
        }

        private Lazy<IConnection> CreateConnection()
        {
            return new Lazy<IConnection>(() => new ConnectionFactory().CreateConnection(options));
        }
        /*
        private IConnection GetConnection()
        {
            if (!connection.IsReconnecting() && (connection.IsClosed() || connection.IsDraining()))
                connection = new ConnectionFactory().CreateConnection(options);
            return connection;
        }
        */
    }
}
