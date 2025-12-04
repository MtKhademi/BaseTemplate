using System.Text.Json;

namespace Infrastructure.Messaging.Serializers;

internal class RawDataMessageSerializer :
   RawMessageSerializer,
   IMessageDeserializer,
   IMessageSerializer
{
    public RawDataMessageSerializer()
    {
    }

    public ContentType ContentType => new ContentType("text/plain;");

    public void Probe(ProbeContext context)
    {
        var scope = context.CreateScope("text");
        scope.Add("contentType", ContentType.MediaType);
        scope.Add("provider", "System.Text");
    }

    public ConsumeContext Deserialize(ReceiveContext receiveContext)
    {
        return new BodyConsumeContext(receiveContext, Deserialize(receiveContext.Body, receiveContext.TransportHeaders, receiveContext.InputAddress));
    }

    public SerializerContext Deserialize(MessageBody body, Headers headers, Uri? destinationAddress = null)
    {
        try
        {
            JsonElement? bodyElement = null;

            var rawMessage = System.Text.Json.JsonSerializer.Serialize(new RawDataMessage { Data = body.GetBytes() });
            bodyElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(rawMessage, JsonSerializerOptions.Default);

            var messageTypes = headers.GetMessageTypes();

            var messageContext = new RawMessageContext(headers, destinationAddress, RawSerializerOptions.Default);

            var serializerContext = new SystemTextJsonRawSerializerContext(SystemTextJsonMessageSerializer.Instance,
                JsonSerializerOptions.Default, ContentType, messageContext, messageTypes, RawSerializerOptions.Default, bodyElement.Value);

            return serializerContext;
        }
        catch (SerializationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new SerializationException("An error occured while deserializing the message enveloper", ex);
        }
    }

    public MessageBody GetMessageBody(string text)
    {
        return new StringMessageBody(text);
    }

    public MessageBody GetMessageBody<T>(SendContext<T> context)
        where T : class
    {
        //if (_options.HasFlag(RawSerializerOptions.AddTransportHeaders))
        //    SetRawMessageHeaders(context);

        return new SystemTextJsonRawMessageBody<T>(context, JsonSerializerOptions.Default);
    }
}
