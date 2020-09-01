using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;

namespace BusManager.Messages
{
    public class BusMessage : IBusMessage
    {
        public Guid Id { get; set; }
        public string MessageType { get; set; }
        public object Body { get; set; }
        public bool IsError { get; set; }
        public string ErrorDetails { get; set; }
        public DateTime CreateDate { get; set; }
        public int TTL { get; set; }
        public Guid UserId { get; set; }

        public virtual T GetBody<T>()
        {
            try
            {
                T result = default(T);
                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Boolean:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.DateTime:
                    case TypeCode.Double:
                    case TypeCode.String:
                        result = (T)Convert.ChangeType(Body.ToString(), typeof(T));
                        break;
                    case TypeCode.Object:
                        if (typeof(T) == typeof(Guid))
                        {
                            Guid current = Guid.Parse(Body.ToString());
                            result = (T)Convert.ChangeType(current, typeof(T), CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            result = JsonConvert.DeserializeObject<T>(Body.ToString());
                        }
                        break;
                    default:
                        result = JsonConvert.DeserializeObject<T>(Body.ToString());
                        break;
                }
                return result;
            }
            catch
            {
                return default(T);
            }
        }
       
        public virtual byte[] GetMessage()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this);
                return Encoding.UTF8.GetBytes(json);
            }
            catch
            {
                return Encoding.UTF8.GetBytes("");
            }
        }
    }
}
