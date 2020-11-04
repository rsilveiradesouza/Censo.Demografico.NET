using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Censo.NET.Domain.Common.Exceptions
{
    public class RegraDeNegocioException : Exception
    {
        public IEnumerable<string> MensagensErro { get; }
        public IEnumerable<string> CodigoMensagensErro { get; }

        public RegraDeNegocioException()
        {
        }

        protected RegraDeNegocioException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RegraDeNegocioException(string message) : base(message)
        {
            MensagensErro = new List<string>() { message };
        }

        public RegraDeNegocioException(string message, Exception innerException) : base(message, innerException)
        {
            MensagensErro = new List<string>() { message };
        }
    }
}
