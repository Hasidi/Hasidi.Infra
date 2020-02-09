using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Hasidi.Infra.ObjectConvertors
{
    public class JsonObjectConvertor : IObjectConvertor
    {
        public T ConvertToObject<T>(byte[] byteArray)
        {
            var bytes = Encoding.UTF8.GetString(byteArray);
            var res = JsonConvert.DeserializeObject<T>(bytes);
            return res;
        }

        public byte[] ConvertToBytes<T>(T objectData)
        {
            var json = JsonConvert.SerializeObject(objectData);
            var res = Encoding.UTF8.GetBytes(json);
            return res;
        }
    }
}
