using MassTransit;
using System.Net.Mime;

namespace Infrastructure.Messaging.Serializers;

public class RawDataMessageSerializerFactory : ISerializerFactory
{
    readonly Lazy<RawDataMessageSerializer> _serializer;

    public RawDataMessageSerializerFactory(RawSerializerOptions options = RawSerializerOptions.Default)
    {
        _serializer = new Lazy<RawDataMessageSerializer>(() => new RawDataMessageSerializer());
    }

    public ContentType ContentType => _serializer.Value.ContentType;

    public IMessageSerializer CreateSerializer()
    {
        return _serializer.Value;
    }

    public IMessageDeserializer CreateDeserializer()
    {
        return _serializer.Value;
    }
}
