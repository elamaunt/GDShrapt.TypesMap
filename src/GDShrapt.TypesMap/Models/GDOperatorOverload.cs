using System.Collections.Generic;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents an operator overload for a type.
    /// Defines what types can be combined with an operator and what the result type is.
    /// </summary>
    public class GDOperatorOverload
    {
        /// <summary>
        /// The type of the right operand.
        /// Null for unary operators.
        /// </summary>
        public string? RightType { get; set; }

        /// <summary>
        /// The result type of the operation.
        /// </summary>
        public string ResultType { get; set; } = "Variant";

        /// <summary>
        /// Optional notes about special behavior.
        /// </summary>
        public string? Notes { get; set; }

        public GDOperatorOverload()
        {
        }

        public GDOperatorOverload(string? rightType, string resultType)
        {
            RightType = rightType;
            ResultType = resultType;
        }
    }

    /// <summary>
    /// Collection of operator overloads for a type, keyed by operator symbol.
    /// </summary>
    public class GDTypeOperators
    {
        /// <summary>
        /// Addition overloads (+).
        /// </summary>
        public List<GDOperatorOverload>? Addition { get; set; }

        /// <summary>
        /// Subtraction overloads (-).
        /// </summary>
        public List<GDOperatorOverload>? Subtraction { get; set; }

        /// <summary>
        /// Multiplication overloads (*).
        /// </summary>
        public List<GDOperatorOverload>? Multiplication { get; set; }

        /// <summary>
        /// Division overloads (/).
        /// </summary>
        public List<GDOperatorOverload>? Division { get; set; }

        /// <summary>
        /// Modulo overloads (%).
        /// </summary>
        public List<GDOperatorOverload>? Modulo { get; set; }

        /// <summary>
        /// Power overloads (**).
        /// </summary>
        public List<GDOperatorOverload>? Power { get; set; }

        /// <summary>
        /// Unary negation (-x).
        /// </summary>
        public GDOperatorOverload? Negate { get; set; }

        /// <summary>
        /// Bitwise AND (&).
        /// </summary>
        public List<GDOperatorOverload>? BitwiseAnd { get; set; }

        /// <summary>
        /// Bitwise OR (|).
        /// </summary>
        public List<GDOperatorOverload>? BitwiseOr { get; set; }

        /// <summary>
        /// Bitwise XOR (^).
        /// </summary>
        public List<GDOperatorOverload>? BitwiseXor { get; set; }

        /// <summary>
        /// Bitwise NOT (~).
        /// </summary>
        public GDOperatorOverload? BitwiseNot { get; set; }

        /// <summary>
        /// Left shift (<<).
        /// </summary>
        public List<GDOperatorOverload>? ShiftLeft { get; set; }

        /// <summary>
        /// Right shift (>>).
        /// </summary>
        public List<GDOperatorOverload>? ShiftRight { get; set; }

        /// <summary>
        /// Gets operator overloads by operator name.
        /// </summary>
        public List<GDOperatorOverload>? GetByName(string operatorName)
        {
            return operatorName switch
            {
                "+" or "Addition" => Addition,
                "-" or "Subtraction" => Subtraction,
                "*" or "Multiplication" => Multiplication,
                "/" or "Division" => Division,
                "%" or "Modulo" => Modulo,
                "**" or "Power" => Power,
                "&" or "BitwiseAnd" => BitwiseAnd,
                "|" or "BitwiseOr" => BitwiseOr,
                "^" or "BitwiseXor" => BitwiseXor,
                "<<" or "ShiftLeft" => ShiftLeft,
                ">>" or "ShiftRight" => ShiftRight,
                _ => null
            };
        }
    }
}
