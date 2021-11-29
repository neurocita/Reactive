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
        public void Json()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(12);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            StreamReader streamReader = new StreamReader(message.Body);
            Console.WriteLine(streamReader.ReadToEnd());
            message.Body.Position = 0;
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 12);
        }

        [TestMethod]
        public void Xml()
        {
            DataContractXmlSerializer serializer = new DataContractXmlSerializer();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(23);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            StreamReader streamReader = new StreamReader(message.Body);
            Console.WriteLine(streamReader.ReadToEnd());
            message.Body.Position = 0;
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 23);

            XmlSerializer serializer2 = new XmlSerializer();
            IMessage<Stream> message2 = new TransportMessage(serializer2.Serialize(value));
            StreamReader streamReader2 = new StreamReader(message2.Body);
            Console.WriteLine(streamReader2.ReadToEnd());
            message2.Body.Position = 0;
            value = serializer2.Deserialize<ValueTypeDataContract<int>>(message2.Body);
            Assert.AreEqual(value, 23);
        }

        [TestMethod]
        public void Binary()
        {
            BinarySerializer serializer = new BinarySerializer();
            IValueTypeDataContract<int> value = new ValueTypeDataContract<int>(167);
            IMessage<Stream> message = new TransportMessage(serializer.Serialize(value));
            value = serializer.Deserialize<ValueTypeDataContract<int>>(message.Body);
            Assert.AreEqual(value, 167);
        }
    }
}
