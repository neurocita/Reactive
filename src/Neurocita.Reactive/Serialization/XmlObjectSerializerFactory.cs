using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Neurocita.Reactive.Utilities;

namespace Neurocita.Reactive.Serialization
{
    internal class XmlObjectSerializerFactory
    {
        #region Private types
        private interface ISettings
        {
            object Inner { get; }
            ICollection<Type> KnownTypes { get; }
        }

        private class SettingsAdapter : ISettings
        {
            private Lazy<ICollection<Type>> _knownTypes;

            public SettingsAdapter(DataContractSerializerSettings settings)
            {
                Inner = settings;
                (Inner as DataContractSerializerSettings).KnownTypes = settings.KnownTypes.ToCollection();
                _knownTypes = new Lazy<ICollection<Type>>(() => (Inner as DataContractSerializerSettings).KnownTypes as ICollection<Type>);
            }

            public SettingsAdapter(DataContractJsonSerializerSettings settings)
            {
                Inner = settings;
                (Inner as DataContractJsonSerializerSettings).KnownTypes = settings.KnownTypes.ToCollection();
                _knownTypes = new Lazy<ICollection<Type>>(() => (Inner as DataContractJsonSerializerSettings).KnownTypes as ICollection<Type>);
            }
            
            public object Inner { get; }
            public ICollection<Type> KnownTypes => _knownTypes.Value;
        }
        #endregion Private types

        private readonly ISettings _settings;

        public XmlObjectSerializerFactory(DataContractSerializeFormat format)
        {
            switch (format)
            {
                case DataContractSerializeFormat.Xml:
                    _settings = new SettingsAdapter(new DataContractSerializerSettings());
                    break;

                case DataContractSerializeFormat.Json:
                    _settings = new SettingsAdapter(new DataContractJsonSerializerSettings());
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }
        }

        public XmlObjectSerializerFactory(DataContractSerializerSettings settings) => _settings = new SettingsAdapter(settings);

        public XmlObjectSerializerFactory(DataContractJsonSerializerSettings settings) => _settings = new SettingsAdapter(settings);

        public XmlObjectSerializer Create(Type type)
        {
            _settings.KnownTypes.Add(type);
            if (_settings.Inner.GetType() == typeof(DataContractSerializerSettings))
                return new System.Runtime.Serialization.DataContractSerializer(type, _settings.Inner as DataContractSerializerSettings);
            else if (_settings.Inner.GetType() == typeof(DataContractJsonSerializerSettings))
                return new System.Runtime.Serialization.Json.DataContractJsonSerializer(type, _settings.Inner as DataContractJsonSerializerSettings);
            else
                throw new ArgumentOutOfRangeException(nameof(_settings.Inner));
        }
    }
}