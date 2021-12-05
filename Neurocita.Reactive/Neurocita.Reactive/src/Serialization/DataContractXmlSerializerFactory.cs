﻿using System.Runtime.Serialization;

namespace Neurocita.Reactive
{
    public class DataContractXmlSerializerFactory : ISerializerFactory
    {
        private readonly DataContractSerializerSettings settings = new DataContractSerializerSettings();

        public DataContractSerializerSettings Settings => settings;

        public ISerializer CreateSerializer()
        {
            settings.KnownTypes = DataContractUtil.PrepareKnownTypes(settings.KnownTypes);
            return new DataContractXmlSerializer(settings);
        }
    }
}
