namespace Hasidi.Infra.ObjectConvertors
{
    public interface IObjectConvertor
    {
        T ConvertToObject<T>(byte[] byteArray);
        byte[] ConvertToBytes<T>(T objectData);
    }
}
