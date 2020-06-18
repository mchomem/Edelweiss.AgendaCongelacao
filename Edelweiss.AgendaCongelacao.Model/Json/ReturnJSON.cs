using System;
using System.Collections.Generic;

namespace Edelweiss.AgendaCongelacao.Model.Json
{
    [Serializable()]
    public class ReturnJSON<T>
    {
        public List<T> Entities { get; set; }
        public T Entity { get; set; }
        public String Message { get; set; }
        public String ReturnCode { get; set; }
    }

    public enum ReturnType
    {
        ERROR
        , SUCCESS
        , WARNING
    }
}
