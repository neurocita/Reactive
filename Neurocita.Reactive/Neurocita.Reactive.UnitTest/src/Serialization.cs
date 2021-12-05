using Neurocita.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Neurocita.Reactive.UnitTest
{
    [TestClass]
    public class Serialization
    {
        [TestMethod]
        public void DataContractJson()
        {
            ISerializer serializer = new DataContractJsonSerializerFactory().CreateSerializer();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(12);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            StreamReader streamReader = new StreamReader(message.Body);
            Console.WriteLine(streamReader.ReadToEnd());
            message.Body.Position = 0;
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 12);
        }

        [TestMethod]
        public void DataContractXml()
        {
            ISerializer serializer = new DataContractXmlSerializerFactory().CreateSerializer();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(23);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            StreamReader streamReader = new StreamReader(message.Body);
            Console.WriteLine(streamReader.ReadToEnd());
            message.Body.Position = 0;
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 23);
        }

        [TestMethod]
        public void Xml()
        {
            ISerializer serializer = new XmlSerializerFactory().CreateSerializer();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(32);
            IMessage<Stream> message2 = new TransportMessage(serializer.Serialize(value));
            StreamReader streamReader2 = new StreamReader(message2.Body);
            Console.WriteLine(streamReader2.ReadToEnd());
            message2.Body.Position = 0;
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message2.Body);
            Assert.AreEqual(value, 32);
        }

        [TestMethod]
        public void Binary()
        {
            ISerializer serializer = new BinarySerializerFactory().CreateSerializer();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(167);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 167);
        }
    }
}
