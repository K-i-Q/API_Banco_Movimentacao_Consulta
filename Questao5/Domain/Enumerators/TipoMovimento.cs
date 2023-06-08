using System;

namespace Questao5.Domain.Enumerators
{
    public enum TipoMovimento
    {
        [StringValue("C")]
        Credito,

        [StringValue("D")]
        Debito
    }

    public class StringValueAttribute : Attribute
    {
        public string Value { get; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }
}
