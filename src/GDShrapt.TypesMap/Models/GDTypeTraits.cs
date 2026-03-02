namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Represents type characteristics that define its behavior in GDScript.
    /// These traits are used for type checking, operator resolution, and code analysis.
    /// </summary>
    public class GDTypeTraits
    {
        /// <summary>
        /// Whether this is a numeric type (int, float).
        /// Numeric types support arithmetic operations and can be used in numeric contexts.
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// Whether this is a vector type (Vector2, Vector3, Vector4, and their integer variants).
        /// Vector types support component-wise operations and geometric operations.
        /// </summary>
        public bool IsVector { get; set; }

        /// <summary>
        /// Whether this is an integer vector type (Vector2i, Vector3i, Vector4i).
        /// Integer vectors have special promotion rules when combined with floats.
        /// </summary>
        public bool IsIntegerVector { get; set; }

        /// <summary>
        /// Whether this is a transform type (Transform2D, Transform3D, Basis).
        /// Transform types support matrix composition via multiplication.
        /// </summary>
        public bool IsTransform { get; set; }

        /// <summary>
        /// Whether this type supports iteration via for-in loops.
        /// Includes Array, Dictionary, String, and all PackedArray types.
        /// </summary>
        public bool IsIterable { get; set; }

        /// <summary>
        /// Whether this type supports indexing with [] operator.
        /// Includes containers, vectors, matrices, strings.
        /// </summary>
        public bool IsIndexable { get; set; }

        /// <summary>
        /// Whether this type can be null.
        /// False for value types (int, float, Vector*, etc.).
        /// True for reference types (Object, Node, Resource, etc.).
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Whether this is a RefCounted type (Resource and its descendants).
        /// </summary>
        public bool IsRefCounted { get; set; }

        /// <summary>
        /// Whether this is a packed array type (PackedByteArray, PackedVector2Array, etc.).
        /// Packed arrays have optimized storage and specific element types.
        /// </summary>
        public bool IsPackedArray { get; set; }

        /// <summary>
        /// Whether this is a string-like type (String, StringName).
        /// String-like types support concatenation and format operations.
        /// </summary>
        public bool IsStringLike { get; set; }

        /// <summary>
        /// Whether this is a container type (Array, Dictionary, or typed variants).
        /// </summary>
        public bool IsContainer { get; set; }

        /// <summary>
        /// The float variant of an integer vector (e.g., Vector2 for Vector2i).
        /// Null if not applicable.
        /// </summary>
        public string? FloatVariant { get; set; }

        /// <summary>
        /// The integer variant of a float vector (e.g., Vector2i for Vector2).
        /// Null if not applicable.
        /// </summary>
        public string? IntVariant { get; set; }

        /// <summary>
        /// The element type for packed arrays (e.g., "int" for PackedInt32Array).
        /// Null if not a packed array.
        /// </summary>
        public string? PackedElementType { get; set; }

        /// <summary>
        /// GDScript types this type can be implicitly converted to.
        /// Populated based on GDScript language rules (e.g., int → float, Array → PackedStringArray).
        /// Null if no implicit conversions exist.
        /// </summary>
        public string[]? ImplicitlyConvertibleTo { get; set; }

        /// <summary>
        /// Creates default traits for an unknown type.
        /// </summary>
        public GDTypeTraits()
        {
        }

        /// <summary>
        /// Creates a copy of traits.
        /// </summary>
        public GDTypeTraits Clone()
        {
            return new GDTypeTraits
            {
                IsNumeric = IsNumeric,
                IsVector = IsVector,
                IsIntegerVector = IsIntegerVector,
                IsTransform = IsTransform,
                IsIterable = IsIterable,
                IsIndexable = IsIndexable,
                IsNullable = IsNullable,
                IsRefCounted = IsRefCounted,
                IsPackedArray = IsPackedArray,
                IsStringLike = IsStringLike,
                IsContainer = IsContainer,
                FloatVariant = FloatVariant,
                IntVariant = IntVariant,
                PackedElementType = PackedElementType,
                ImplicitlyConvertibleTo = ImplicitlyConvertibleTo
            };
        }
    }
}
