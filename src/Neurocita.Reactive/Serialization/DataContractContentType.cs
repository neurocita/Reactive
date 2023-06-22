using System.Net.Mime;

namespace Neurocita.Reactive.Serialization
{
    public static class DataContractContentType
    {
        public static string TextXml => MediaTypeNames.Text.Xml;
        public static string ApplicationJson => "application/json";     //  MediaTypeNames.Application.Json;
    }
}