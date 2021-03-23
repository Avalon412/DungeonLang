using System;
using System.Runtime.Serialization;

namespace DungeonLang.Parser.AST
{
    [Serializable]
    internal class RuntimeExpression : Exception
    {
        public RuntimeExpression()
        {
        }

        public RuntimeExpression(string message) : base(message)
        {
        }

        public RuntimeExpression(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RuntimeExpression(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}