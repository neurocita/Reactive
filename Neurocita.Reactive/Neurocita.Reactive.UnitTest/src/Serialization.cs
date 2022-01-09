using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;
using System;
using System.IO;

namespace Neurocita.Reactive.UnitTest
{
    [TestClass]
    public class Serialization
    {
        [TestMethod]
        public void TestDataContractJson()
        {
            ISerializer serializer = new DataContractJsonSerializerFactory().Create();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(12);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            StreamReader streamReader = new StreamReader(message.Body);
            Console.WriteLine(streamReader.ReadToEnd());
            message.Body.Position = 0;
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 12);
        }

        [TestMethod]
        public void TestDataContractXml()
        {
            ISerializer serializer = new DataContractXmlSerializerFactory().Create();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(23);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            StreamReader streamReader = new StreamReader(message.Body);
            Console.WriteLine(streamReader.ReadToEnd());
            message.Body.Position = 0;
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 23);
        }

        [TestMethod]
        public void TestXml()
        {
            ISerializer serializer = new XmlSerializerFactory().Create();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(32);
            IMessage<Stream> message2 = new TransportMessage(serializer.Serialize(value));
            StreamReader streamReader2 = new StreamReader(message2.Body);
            Console.WriteLine(streamReader2.ReadToEnd());
            message2.Body.Position = 0;
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message2.Body);
            Assert.AreEqual(value, 32);
        }

        [TestMethod]
        public void TestBinary()
        {
            ISerializer serializer = new BinarySerializerFactory().Create();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(167);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 167);
        }
    }
}
