namespace RLabs.Cielo.SDK.Formatters
{
    internal interface IFormatter<TRequest, TResponse, MessageType>
    {
        MessageType ParseRequestToMessage(TRequest requestData);
        TResponse ParseMessageToResponse(MessageType message);
    }
}
