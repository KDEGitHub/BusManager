﻿using Newtonsoft.Json;
using System;
using System.Text;

namespace BusManager.Messages
{
    public class BusMessage : IBusMessage
    {
        public Guid Id { get; set; }
        public string MessageType { get; set; }
        public object Body { get; set; }
        public bool IsError { get; set; }
        public string Details { get; set; }
        public DateTime CreateDate { get; set; }
        public int TTL { get; set; }

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
                        result = (T)Convert.ChangeType(Body.ToString(), typeof(T));
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
