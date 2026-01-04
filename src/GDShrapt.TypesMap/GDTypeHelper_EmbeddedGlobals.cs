using System.Linq;
using Godot;
using GodotDictionary = Godot.Collections.Dictionary;
using GodotArray = Godot.Collections.Array;

namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Contains embedded global type mappings for GDScript built-in functions,
    /// constants, and types that are available at global scope.
    /// </summary>
    public static partial class GDTypeHelper
    {
        private static void AddEmbeddedGlobalConstants(Dictionary<string, GDConstantInfo> constants)
        {
            constants["PI"] = new GDConstantInfo("PI", nameof(Mathf.Pi), typeof(double), typeof(Mathf));
            constants["TAU"] = new GDConstantInfo("TAU", nameof(Mathf.Tau), typeof(double), typeof(Mathf));
            constants["INF"] = new GDConstantInfo("INF", nameof(Mathf.Inf), typeof(double), typeof(Mathf));
            constants["NAN"] = new GDConstantInfo("NAN", nameof(Mathf.NaN), typeof(double), typeof(Mathf));
        }

        private static void AddEmbeddedGlobalMethods(Dictionary<string, List<GDMethodData>> methodDatas)
        {
            methodDatas["abs"] = new List<GDMethodData>() { new GDMethodData("abs", typeof(Mathf).GetMethod(nameof(Mathf.Abs), new Type[] { typeof(double) })!) };
            methodDatas["absf"] = new List<GDMethodData>() { new GDMethodData("absf", typeof(Mathf).GetMethod(nameof(Mathf.Abs), new Type[] { typeof(float) })!) };
            methodDatas["acos"] = new List<GDMethodData>() { new GDMethodData("acos", typeof(Mathf).GetMethod(nameof(Mathf.Acos), new Type[] { typeof(double) })!) };
            methodDatas["asin"] = new List<GDMethodData>() { new GDMethodData("asin", typeof(Mathf).GetMethod(nameof(Mathf.Asin), new Type[] { typeof(double) })!) };
            methodDatas["atan"] = new List<GDMethodData>() { new GDMethodData("atan", typeof(Mathf).GetMethod(nameof(Mathf.Atan), new Type[] { typeof(double) })!) };
            methodDatas["atan2"] = new List<GDMethodData>() { new GDMethodData("atan2", typeof(Mathf).GetMethod(nameof(Mathf.Atan2), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["bezier_derivative"] = new List<GDMethodData>() { new GDMethodData("bezier_derivative", typeof(Mathf).GetMethod(nameof(Mathf.BezierDerivative), new Type[] { typeof(double), typeof(double), typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["bezier_interpolate"] = new List<GDMethodData>() { new GDMethodData("bezier_interpolate", typeof(Mathf).GetMethod(nameof(Mathf.BezierInterpolate), new Type[] { typeof(double), typeof(double), typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["bytes_to_var"] = new List<GDMethodData>() { new GDMethodData("bytes_to_var", typeof(GD).GetMethod(nameof(GD.BytesToVar))!) };
            methodDatas["bytes_to_var_with_objects"] = new List<GDMethodData>() { new GDMethodData("bytes_to_var_with_objects", typeof(GD).GetMethod(nameof(GD.BytesToVarWithObjects))!) };
            methodDatas["ceil"] = new List<GDMethodData>() { new GDMethodData("ceil", typeof(Mathf).GetMethod(nameof(Mathf.Ceil), new Type[] { typeof(double) })!) };
            methodDatas["ceilf"] = new List<GDMethodData>() { new GDMethodData("ceilf", typeof(Mathf).GetMethod(nameof(Mathf.Ceil), new Type[] { typeof(float) })!) };
            methodDatas["ceili"] = new List<GDMethodData>() { new GDMethodData("ceili", typeof(Mathf).GetMethod(nameof(Mathf.CeilToInt), new Type[] { typeof(double) })!) };
            methodDatas["clamp"] = new List<GDMethodData>() { new GDMethodData("clamp", typeof(Mathf).GetMethod(nameof(Mathf.Clamp), new Type[] { typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["clampf"] = new List<GDMethodData>() { new GDMethodData("clampf", typeof(Mathf).GetMethod(nameof(Mathf.Clamp), new Type[] { typeof(float), typeof(float), typeof(float) })!) };
            methodDatas["clampi"] = new List<GDMethodData>() { new GDMethodData("clampi", typeof(Mathf).GetMethod(nameof(Mathf.Clamp), new Type[] { typeof(int), typeof(int), typeof(int) })!) };
            methodDatas["cos"] = new List<GDMethodData>() { new GDMethodData("cos", typeof(Mathf).GetMethod(nameof(Mathf.Cos), new Type[] { typeof(double) })!) };
            methodDatas["cosh"] = new List<GDMethodData>() { new GDMethodData("cosh", typeof(Mathf).GetMethod(nameof(Mathf.Cosh), new Type[] { typeof(double) })!) };
            methodDatas["cubic_interpolate"] = new List<GDMethodData>() { new GDMethodData("cubic_interpolate", typeof(Mathf).GetMethod(nameof(Mathf.CubicInterpolate), new Type[] { typeof(double), typeof(double), typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["cubic_interpolate_angle"] = new List<GDMethodData>() { new GDMethodData("cubic_interpolate_angle", typeof(Mathf).GetMethod(nameof(Mathf.CubicInterpolateAngle), new Type[] { typeof(double), typeof(double), typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["cubic_interpolate_angle_in_time"] = new List<GDMethodData>() { new GDMethodData("cubic_interpolate_angle_in_time", typeof(Mathf).GetMethod(nameof(Mathf.CubicInterpolateAngleInTime), new Type[] { typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["cubic_interpolate_in_time"] = new List<GDMethodData>() { new GDMethodData("cubic_interpolate_in_time", typeof(Mathf).GetMethod(nameof(Mathf.CubicInterpolateInTime), new Type[] { typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["db_to_linear"] = new List<GDMethodData>() { new GDMethodData("db_to_linear", typeof(Mathf).GetMethod(nameof(Mathf.DbToLinear), new Type[] { typeof(double) })!) };
            methodDatas["deg_to_rad"] = new List<GDMethodData>() { new GDMethodData("deg_to_rad", typeof(Mathf).GetMethod(nameof(Mathf.DegToRad), new Type[] { typeof(double) })!) };
            methodDatas["ease"] = new List<GDMethodData>() { new GDMethodData("ease", typeof(Mathf).GetMethod(nameof(Mathf.Ease), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["error_string"] = new List<GDMethodData>() { new GDMethodData("error_string", typeof(Error).GetMethod(nameof(Error.ToString), new Type[] { })!) };
            methodDatas["exp"] = new List<GDMethodData>() { new GDMethodData("exp", typeof(Mathf).GetMethod(nameof(Mathf.Exp), new Type[] { typeof(double) })!) };
            methodDatas["floor"] = new List<GDMethodData>() { new GDMethodData("floor", typeof(Mathf).GetMethod(nameof(Mathf.Floor), new Type[] { typeof(double) })!) };
            methodDatas["floorf"] = new List<GDMethodData>() { new GDMethodData("floorf", typeof(Mathf).GetMethod(nameof(Mathf.Floor), new Type[] { typeof(float) })!) };
            methodDatas["floori"] = new List<GDMethodData>() { new GDMethodData("floori", typeof(Mathf).GetMethod(nameof(Mathf.FloorToInt), new Type[] { typeof(double) })!) };
            methodDatas["fposmod"] = new List<GDMethodData>() { new GDMethodData("fposmod", typeof(Mathf).GetMethod(nameof(Mathf.PosMod), new Type[] { typeof(float), typeof(float) })!) };
            methodDatas["hash"] = new List<GDMethodData>() { new GDMethodData("hash", typeof(GD).GetMethod(nameof(GD.Hash))!) };
            methodDatas["instance_from_id"] = new List<GDMethodData>() { new GDMethodData("instance_from_id", typeof(GodotObject).GetMethod(nameof(GodotObject.InstanceFromId))!) };
            methodDatas["inverse_lerp"] = new List<GDMethodData>() { new GDMethodData("inverse_lerp", typeof(Mathf).GetMethod(nameof(Mathf.InverseLerp), new Type[] { typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["is_equal_approx"] = new List<GDMethodData>() { new GDMethodData("is_equal_approx", typeof(Mathf).GetMethod(nameof(Mathf.IsEqualApprox), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["is_finite"] = new List<GDMethodData>() { new GDMethodData("is_finite", typeof(Mathf).GetMethod(nameof(Mathf.IsFinite), new Type[] { typeof(double) })!) };
            methodDatas["is_inf"] = new List<GDMethodData>() { new GDMethodData("is_inf", typeof(Mathf).GetMethod(nameof(Mathf.IsInf), new Type[] { typeof(double) })!) };
            methodDatas["is_instance_id_valid"] = new List<GDMethodData>() { new GDMethodData("is_instance_id_valid", typeof(GodotObject).GetMethod(nameof(GodotObject.IsInstanceIdValid))!) };
            methodDatas["is_instance_valid"] = new List<GDMethodData>() { new GDMethodData("is_instance_valid", typeof(GodotObject).GetMethod(nameof(GodotObject.IsInstanceValid))!) };
            methodDatas["is_nan"] = new List<GDMethodData>() { new GDMethodData("is_nan", typeof(Mathf).GetMethod(nameof(Mathf.IsNaN), new Type[] { typeof(double) })!) };
            methodDatas["is_same"] = new List<GDMethodData>() { new GDMethodData("is_same", typeof(object).GetMethod(nameof(object.ReferenceEquals))!) };
            methodDatas["is_zero_approx"] = new List<GDMethodData>() { new GDMethodData("is_zero_approx", typeof(Mathf).GetMethod(nameof(Mathf.IsZeroApprox), new Type[] { typeof(double) })!) };
            methodDatas["lerp"] = new List<GDMethodData>() { new GDMethodData("lerp", typeof(Mathf).GetMethod(nameof(Mathf.Lerp), new Type[] { typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["lerp_angle"] = new List<GDMethodData>() { new GDMethodData("lerp_angle", typeof(Mathf).GetMethod(nameof(Mathf.LerpAngle), new Type[] { typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["lerpf"] = new List<GDMethodData>() { new GDMethodData("lerpf", typeof(Mathf).GetMethod(nameof(Mathf.Lerp), new Type[] { typeof(float), typeof(float), typeof(float) })!) };
            methodDatas["linear_to_db"] = new List<GDMethodData>() { new GDMethodData("linear_to_db", typeof(Mathf).GetMethod(nameof(Mathf.LinearToDb), new Type[] { typeof(double) })!) };
            methodDatas["log"] = new List<GDMethodData>() { new GDMethodData("log", typeof(Mathf).GetMethod(nameof(Mathf.Log), new Type[] { typeof(double) })!) };
            methodDatas["max"] = new List<GDMethodData>() { new GDMethodData("max", typeof(Mathf).GetMethod(nameof(Mathf.Max), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["maxf"] = new List<GDMethodData>() { new GDMethodData("maxf", typeof(Mathf).GetMethod(nameof(Mathf.Max), new Type[] { typeof(float), typeof(float) })!) };
            methodDatas["maxi"] = new List<GDMethodData>() { new GDMethodData("maxi", typeof(Mathf).GetMethod(nameof(Mathf.Max), new Type[] { typeof(int), typeof(int) })!) };
            methodDatas["min"] = new List<GDMethodData>() { new GDMethodData("min", typeof(Mathf).GetMethod(nameof(Mathf.Min), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["minf"] = new List<GDMethodData>() { new GDMethodData("minf", typeof(Mathf).GetMethod(nameof(Mathf.Min), new Type[] { typeof(float), typeof(float) })!) };
            methodDatas["mini"] = new List<GDMethodData>() { new GDMethodData("mini", typeof(Mathf).GetMethod(nameof(Mathf.Min), new Type[] { typeof(int), typeof(int) })!) };
            methodDatas["move_toward"] = new List<GDMethodData>() { new GDMethodData("move_toward", typeof(Mathf).GetMethod(nameof(Mathf.MoveToward), new Type[] { typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["nearest_po2"] = new List<GDMethodData>() { new GDMethodData("nearest_po2", typeof(Mathf).GetMethod(nameof(Mathf.NearestPo2))!) };
            methodDatas["pingpong"] = new List<GDMethodData>() { new GDMethodData("pingpong", typeof(Mathf).GetMethod(nameof(Mathf.PingPong), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["posmod"] = new List<GDMethodData>() { new GDMethodData("posmod", typeof(Mathf).GetMethod(nameof(Mathf.PosMod), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["pow"] = new List<GDMethodData>() { new GDMethodData("pow", typeof(Mathf).GetMethod(nameof(Mathf.Pow), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["print"] = new List<GDMethodData>() { new GDMethodData("print", typeof(GD).GetMethod(nameof(GD.Print), new Type[] { typeof(object[]) })!) };
            methodDatas["print_rich"] = new List<GDMethodData>() { new GDMethodData("print_rich", typeof(GD).GetMethod(nameof(GD.PrintRich), new Type[] { typeof(object[]) })!) };
            methodDatas["print_verbose"] = new List<GDMethodData>() { new GDMethodData("print_verbose", typeof(GD).GetMethod(nameof(GD.Print), new Type[] { typeof(object[]) })!) };
            methodDatas["printerr"] = new List<GDMethodData>() { new GDMethodData("printerr", typeof(GD).GetMethod(nameof(GD.PrintErr), new Type[] { typeof(object[]) })!) };
            methodDatas["printraw"] = new List<GDMethodData>() { new GDMethodData("printraw", typeof(GD).GetMethod(nameof(GD.PrintRaw), new Type[] { typeof(object[]) })!) };
            methodDatas["prints"] = new List<GDMethodData>() { new GDMethodData("prints", typeof(GD).GetMethod(nameof(GD.PrintS))!) };
            methodDatas["printt"] = new List<GDMethodData>() { new GDMethodData("printt", typeof(GD).GetMethod(nameof(GD.PrintT))!) };
            methodDatas["push_error"] = new List<GDMethodData>() { new GDMethodData("push_error", typeof(GD).GetMethod(nameof(GD.PushError), new Type[] { typeof(object[]) })!) };
            methodDatas["push_warning"] = new List<GDMethodData>() { new GDMethodData("push_warning", typeof(GD).GetMethod(nameof(GD.PushWarning), new Type[] { typeof(object[]) })!) };
            methodDatas["rad_to_deg"] = new List<GDMethodData>() { new GDMethodData("rad_to_deg", typeof(Mathf).GetMethod(nameof(Mathf.RadToDeg), new Type[] { typeof(double) })!) };
            methodDatas["rand_from_seed"] = new List<GDMethodData>() { new GDMethodData("rand_from_seed", typeof(GD).GetMethod(nameof(GD.RandFromSeed))!) };
            methodDatas["randf"] = new List<GDMethodData>() { new GDMethodData("randf", typeof(GD).GetMethod(nameof(GD.Randf))!) };
            methodDatas["rand_range"] = new List<GDMethodData>() { new GDMethodData("rand_range", typeof(GD).GetMethod(nameof(GD.RandRange), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["randf_range"] = new List<GDMethodData>() { new GDMethodData("randf_range", typeof(GD).GetMethod(nameof(GD.RandRange), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["randfn"] = new List<GDMethodData>() { new GDMethodData("randfn", typeof(GD).GetMethod(nameof(GD.Randfn))!) };
            methodDatas["randi"] = new List<GDMethodData>() { new GDMethodData("randi", typeof(GD).GetMethod(nameof(GD.Randi))!) };
            methodDatas["randi_range"] = new List<GDMethodData>() { new GDMethodData("randi_range", typeof(GD).GetMethod(nameof(GD.RandRange), new Type[] { typeof(int), typeof(int) })!) };
            methodDatas["randomize"] = new List<GDMethodData>() { new GDMethodData("randomize", typeof(GD).GetMethod(nameof(GD.Randomize))!) };
            methodDatas["remap"] = new List<GDMethodData>() { new GDMethodData("remap", typeof(Mathf).GetMethod(nameof(Mathf.Remap), new Type[] { typeof(double), typeof(double), typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["round"] = new List<GDMethodData>() { new GDMethodData("round", typeof(Mathf).GetMethod(nameof(Mathf.Round), new Type[] { typeof(double) })!) };
            methodDatas["roundf"] = new List<GDMethodData>() { new GDMethodData("roundf", typeof(Mathf).GetMethod(nameof(Mathf.Round), new Type[] { typeof(float) })!) };
            methodDatas["roundi"] = new List<GDMethodData>() { new GDMethodData("roundi", typeof(Mathf).GetMethod(nameof(Mathf.RoundToInt), new Type[] { typeof(double) })!) };
            methodDatas["seed"] = new List<GDMethodData>() { new GDMethodData("seed", typeof(GD).GetMethod(nameof(GD.Seed))!) };
            methodDatas["sign"] = new List<GDMethodData>() { new GDMethodData("sign", typeof(Mathf).GetMethod(nameof(Mathf.Sign), new Type[] { typeof(double) })!) };
            methodDatas["signf"] = new List<GDMethodData>() { new GDMethodData("signf", typeof(Mathf).GetMethod(nameof(Mathf.Sign), new Type[] { typeof(float) })!) };
            methodDatas["signi"] = new List<GDMethodData>() { new GDMethodData("signi", typeof(Mathf).GetMethod(nameof(Mathf.Sign), new Type[] { typeof(int) })!) };
            methodDatas["sin"] = new List<GDMethodData>() { new GDMethodData("sin", typeof(Mathf).GetMethod(nameof(Mathf.Sin), new Type[] { typeof(double) })!) };
            methodDatas["sinh"] = new List<GDMethodData>() { new GDMethodData("sinh", typeof(Mathf).GetMethod(nameof(Mathf.Sinh), new Type[] { typeof(double) })!) };
            methodDatas["smoothstep"] = new List<GDMethodData>() { new GDMethodData("smoothstep", typeof(Mathf).GetMethod(nameof(Mathf.SmoothStep), new Type[] { typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["snapped"] = new List<GDMethodData>() { new GDMethodData("snapped", typeof(Mathf).GetMethod(nameof(Mathf.Snapped), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["snappedf"] = new List<GDMethodData>() { new GDMethodData("snappedf", typeof(Mathf).GetMethod(nameof(Mathf.Snapped), new Type[] { typeof(float), typeof(float) })!) };
            methodDatas["snappedi"] = new List<GDMethodData>() { new GDMethodData("snappedi", typeof(Mathf).GetMethod(nameof(Mathf.Snapped), new Type[] { typeof(int), typeof(int) })!) };
            methodDatas["sqrt"] = new List<GDMethodData>() { new GDMethodData("sqrt", typeof(Mathf).GetMethod(nameof(Mathf.Sqrt), new Type[] { typeof(double) })!) };
            methodDatas["step_decimals"] = new List<GDMethodData>() { new GDMethodData("step_decimals", typeof(Mathf).GetMethod(nameof(Mathf.StepDecimals))!) };
            methodDatas["str_to_var"] = new List<GDMethodData>() { new GDMethodData("str_to_var", typeof(GD).GetMethod(nameof(GD.StrToVar))!) };
            methodDatas["tan"] = new List<GDMethodData>() { new GDMethodData("tan", typeof(Mathf).GetMethod(nameof(Mathf.Tan), new Type[] { typeof(double) })!) };
            methodDatas["tanh"] = new List<GDMethodData>() { new GDMethodData("tanh", typeof(Mathf).GetMethod(nameof(Mathf.Tanh), new Type[] { typeof(double) })!) };
            methodDatas["typeof"] = new List<GDMethodData>() { new GDMethodData("typeof", typeof(Variant).GetMethod("get_" + nameof(Variant.VariantType))!) };
            methodDatas["var_to_str"] = new List<GDMethodData>() { new GDMethodData("var_to_str", typeof(GD).GetMethod(nameof(GD.VarToStr))!) };
            methodDatas["wrap"] = new List<GDMethodData>() { new GDMethodData("wrap", typeof(Mathf).GetMethod(nameof(Mathf.Wrap), new Type[] { typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["wrapf"] = new List<GDMethodData>() { new GDMethodData("wrapf", typeof(Mathf).GetMethod(nameof(Mathf.Wrap), new Type[] { typeof(float), typeof(float), typeof(float) })!) };
            methodDatas["wrapi"] = new List<GDMethodData>() { new GDMethodData("wrapi", typeof(Mathf).GetMethod(nameof(Mathf.Wrap), new Type[] { typeof(int), typeof(int), typeof(int) })!) };
            methodDatas["var_to_bytes"] = new List<GDMethodData>() { new GDMethodData("var_to_bytes", typeof(GD).GetMethod(nameof(GD.VarToBytes))!) };
            methodDatas["var_to_bytes_with_objects"] = new List<GDMethodData>() { new GDMethodData("var_to_bytes_with_objects", typeof(GD).GetMethod(nameof(GD.VarToBytesWithObjects))!) };
            methodDatas["weakref"] = new List<GDMethodData>() { new GDMethodData("weakref", typeof(GodotObject).GetMethod(nameof(GodotObject.WeakRef))!) };

            // Additional @GlobalScope methods (Godot 4.5)
            methodDatas["absi"] = new List<GDMethodData>() { new GDMethodData("absi", typeof(Mathf).GetMethod(nameof(Mathf.Abs), new Type[] { typeof(int) })!) };
            methodDatas["acosh"] = new List<GDMethodData>() { new GDMethodData("acosh", typeof(Mathf).GetMethod(nameof(Mathf.Acosh), new Type[] { typeof(double) })!) };
            methodDatas["asinh"] = new List<GDMethodData>() { new GDMethodData("asinh", typeof(Mathf).GetMethod(nameof(Mathf.Asinh), new Type[] { typeof(double) })!) };
            methodDatas["atanh"] = new List<GDMethodData>() { new GDMethodData("atanh", typeof(Mathf).GetMethod(nameof(Mathf.Atanh), new Type[] { typeof(double) })!) };
            methodDatas["angle_difference"] = new List<GDMethodData>() { new GDMethodData("angle_difference", typeof(Mathf).GetMethod(nameof(Mathf.AngleDifference), new Type[] { typeof(double), typeof(double) })!) };
            methodDatas["rotate_toward"] = new List<GDMethodData>() { new GDMethodData("rotate_toward", typeof(Mathf).GetMethod(nameof(Mathf.RotateToward), new Type[] { typeof(double), typeof(double), typeof(double) })!) };
            methodDatas["fmod"] = new List<GDMethodData>() { new GDMethodData("fmod", typeof(Math).GetMethod(nameof(Math.IEEERemainder), new Type[] { typeof(double), typeof(double) })!) };
            // Note: str() in GDScript converts arguments to string - maps to object.ToString() or string concatenation
            methodDatas["str"] = new List<GDMethodData>() { new GDMethodData("str", typeof(object).GetMethod(nameof(object.ToString), Type.EmptyTypes)!) };
            // Note: type_string() returns the string name of a Variant type
            methodDatas["type_string"] = new List<GDMethodData>() { new GDMethodData("type_string", typeof(Enum).GetMethod(nameof(Enum.ToString), Type.EmptyTypes)!) };
        }

        private static void AddEmbeddedGlobalTypes(Dictionary<string, GDGlobalTypeProxyInfo> typeDatas)
        {
            typeDatas["nil"] = new GDGlobalTypeProxyInfo(typeof(Godot.Variant), "null");
            typeDatas["bool"] = new GDGlobalTypeProxyInfo(typeof(bool));
            typeDatas["float"] = new GDGlobalTypeProxyInfo(typeof(double));
            typeDatas["int"] = new GDGlobalTypeProxyInfo(typeof(long));
            typeDatas["string"] = new GDGlobalTypeProxyInfo(typeof(string));
            typeDatas["vector2"] = new GDGlobalTypeProxyInfo(typeof(Vector2));
            typeDatas["vector2i"] = new GDGlobalTypeProxyInfo(typeof(Vector2I));
            typeDatas["rect2"] = new GDGlobalTypeProxyInfo(typeof(Rect2));
            typeDatas["rect2i"] = new GDGlobalTypeProxyInfo(typeof(Rect2I));
            typeDatas["vector3"] = new GDGlobalTypeProxyInfo(typeof(Vector3));
            typeDatas["vector3i"] = new GDGlobalTypeProxyInfo(typeof(Vector3I));
            typeDatas["transform2d"] = new GDGlobalTypeProxyInfo(typeof(Transform2D));
            typeDatas["vector4"] = new GDGlobalTypeProxyInfo(typeof(Vector4));
            typeDatas["vector4i"] = new GDGlobalTypeProxyInfo(typeof(Vector4I));
            typeDatas["plane"] = new GDGlobalTypeProxyInfo(typeof(Plane));
            typeDatas["quaternion"] = new GDGlobalTypeProxyInfo(typeof(Quaternion));
            typeDatas["aabb"] = new GDGlobalTypeProxyInfo(typeof(Aabb));
            typeDatas["basis"] = new GDGlobalTypeProxyInfo(typeof(Basis));
            typeDatas["transform3d"] = new GDGlobalTypeProxyInfo(typeof(Transform3D));
            typeDatas["projection"] = new GDGlobalTypeProxyInfo(typeof(Projection));
            typeDatas["color"] = new GDGlobalTypeProxyInfo(typeof(Color));
            typeDatas["stringname"] = new GDGlobalTypeProxyInfo(typeof(StringName));
            typeDatas["nodepath"] = new GDGlobalTypeProxyInfo(typeof(NodePath));
            typeDatas["rid"] = new GDGlobalTypeProxyInfo(typeof(Rid));
            typeDatas["object"] = new GDGlobalTypeProxyInfo(typeof(GodotObject));
            typeDatas["callable"] = new GDGlobalTypeProxyInfo(typeof(Callable));
            typeDatas["signal"] = new GDGlobalTypeProxyInfo(typeof(Signal));
            typeDatas["dictionary"] = new GDGlobalTypeProxyInfo(typeof(GodotDictionary));
            typeDatas["array"] = new GDGlobalTypeProxyInfo(typeof(GodotArray));
            typeDatas["packedbytearray"] = new GDGlobalTypeProxyInfo(typeof(List<byte>));
            typeDatas["packedint32array"] = new GDGlobalTypeProxyInfo(typeof(List<int>));
            typeDatas["packedint64array"] = new GDGlobalTypeProxyInfo(typeof(List<long>));
            typeDatas["packedfloat32array"] = new GDGlobalTypeProxyInfo(typeof(List<float>));
            typeDatas["packedfloat64array"] = new GDGlobalTypeProxyInfo(typeof(List<double>));
            typeDatas["packedstringarray"] = new GDGlobalTypeProxyInfo(typeof(List<string>));
            typeDatas["packedvector2array"] = new GDGlobalTypeProxyInfo(typeof(List<Vector2>));
            typeDatas["packedvector3array"] = new GDGlobalTypeProxyInfo(typeof(List<Vector3>));
            typeDatas["packedcolorarray"] = new GDGlobalTypeProxyInfo(typeof(List<Color>));
            typeDatas["packedvector4array"] = new GDGlobalTypeProxyInfo(typeof(List<Vector4>));
        }

        /// <summary>
        /// Adds GDScript-specific built-in methods that are available in @GDScript scope.
        /// These are GDScript language features that may not have direct C# equivalents.
        /// </summary>
        private static void AddEmbeddedGDScriptMethods(Dictionary<string, List<GDMethodData>> methodDatas)
        {
            // Color8 - Create color from 8-bit values
            methodDatas["Color8"] = new List<GDMethodData>() { new GDMethodData("Color8", typeof(Color).GetMethod(nameof(Color.Color8), new Type[] { typeof(byte), typeof(byte), typeof(byte), typeof(byte) })!) };

            // load/preload - Resource loading (GD.Load in C#)
            // GD.Load has generic overloads, so we need to find the non-generic one explicitly
            var loadMethod = typeof(GD).GetMethods()
                .FirstOrDefault(m => m.Name == nameof(GD.Load) && !m.IsGenericMethod && m.GetParameters().Length == 1);
            if (loadMethod != null)
            {
                methodDatas["load"] = new List<GDMethodData>() { new GDMethodData("load", loadMethod) };
                methodDatas["preload"] = new List<GDMethodData>() { new GDMethodData("preload", loadMethod) };
            }

            // type_exists - Check if class exists
            methodDatas["type_exists"] = new List<GDMethodData>() { new GDMethodData("type_exists", typeof(ClassDB).GetMethod(nameof(ClassDB.ClassExists), new Type[] { typeof(StringName) })!) };

            // is_instance_of - Type checking
            methodDatas["is_instance_of"] = new List<GDMethodData>() { new GDMethodData("is_instance_of", typeof(GodotObject).GetMethod(nameof(GodotObject.IsInstanceValid))!) };

            // print_debug, print_stack, get_stack - Debug functions
            methodDatas["print_debug"] = new List<GDMethodData>() { new GDMethodData("print_debug", typeof(GD).GetMethod(nameof(GD.Print), new Type[] { typeof(object[]) })!) };
            methodDatas["print_stack"] = new List<GDMethodData>() { new GDMethodData("print_stack", typeof(GD).GetMethod(nameof(GD.Print), new Type[] { typeof(object[]) })!) };

            // range - Iterator creation (maps to GD.Range or Enumerable.Range)
            methodDatas["range"] = new List<GDMethodData>() { new GDMethodData("range", typeof(GD).GetMethod(nameof(GD.Range), new Type[] { typeof(int) })!) };
        }

        private static void AddEmbeddedGlobalEnums(Dictionary<string, List<GDEnumTypeInfo>> dictionary)
        {
            Add(dictionary, "Side", new GDEnumTypeInfo("", typeof(Side),
                 new[]
                 {
                 "SIDE_LEFT",
                 "SIDE_TOP",
                 "SIDE_RIGHT",
                 "SIDE_BOTTOM",
                 },
            new[]
            {
                 Side.Left,
                 Side.Top,
                 Side.Right,
                 Side.Bottom
                 }));

            Add(dictionary, "Corner", new GDEnumTypeInfo("", typeof(Corner),
                 new[]
                 {
                 "CORNER_TOP_LEFT",
                 "CORNER_TOP_RIGHT",
                 "CORNER_BOTTOM_RIGHT",
                 "CORNER_BOTTOM_LEFT",
                 },
            new[]
                 {
                 Corner.TopLeft,
                 Corner.TopRight,
                 Corner.BottomRight,
                 Corner.BottomLeft
                 }));

            Add(dictionary, "Orientation", new GDEnumTypeInfo("", typeof(Orientation),
              new[]
              {
               "HORIZONTAL",
               "VERTICAL",
              },
              new[]
              {
               Orientation.Horizontal,
               Orientation.Vertical,
              }));

            Add(dictionary, "ClockDirection", new GDEnumTypeInfo("", typeof(ClockDirection),
              new[]
              {
               "CLOCKWISE",
               "COUNTERCLOCKWISE",
              },
              new[]
              {
               ClockDirection.Clockwise,
               ClockDirection.Counterclockwise,
              }));

            Add(dictionary, "HorizontalAlignment", new GDEnumTypeInfo("", typeof(HorizontalAlignment),
              new[]
              {
               "HORIZONTAL_ALIGNMENT_LEFT",
               "HORIZONTAL_ALIGNMENT_CENTER",
               "HORIZONTAL_ALIGNMENT_RIGHT",
               "HORIZONTAL_ALIGNMENT_FILL",
              },
              new[]
              {
               HorizontalAlignment.Left,
               HorizontalAlignment.Center,
               HorizontalAlignment.Right,
               HorizontalAlignment.Fill,
              }));

            Add(dictionary, "VerticalAlignment", new GDEnumTypeInfo("", typeof(VerticalAlignment),
             new[]
             {
               "VERTICAL_ALIGNMENT_TOP",
               "VERTICAL_ALIGNMENT_CENTER",
               "VERTICAL_ALIGNMENT_BOTTOM",
               "VERTICAL_ALIGNMENT_FILL",
             },
             new[]
             {
               VerticalAlignment.Top,
               VerticalAlignment.Center,
               VerticalAlignment.Bottom,
               VerticalAlignment.Fill,
             }));

            Add(dictionary, "InlineAlignment", new GDEnumTypeInfo("", typeof(InlineAlignment),
             new[]
             {
               "INLINE_ALIGNMENT_TOP_TO",
               "INLINE_ALIGNMENT_CENTER_TO",
               "INLINE_ALIGNMENT_BASELINE_TO",
               "INLINE_ALIGNMENT_BOTTOM_TO",
               "INLINE_ALIGNMENT_TO_TOP",
               "INLINE_ALIGNMENT_TO_CENTER",
               "INLINE_ALIGNMENT_TO_BASELINE",
               "INLINE_ALIGNMENT_TO_BOTTOM",
               "INLINE_ALIGNMENT_TOP",
               "INLINE_ALIGNMENT_CENTER",
               "INLINE_ALIGNMENT_BOTTOM",
               "INLINE_ALIGNMENT_IMAGE_MASK",
               "INLINE_ALIGNMENT_TEXT_MASK",
             },
             new[]
             {
               InlineAlignment.TopTo,
               InlineAlignment.CenterTo,
               InlineAlignment.BaselineTo,
               InlineAlignment.BottomTo,
               InlineAlignment.ToTop,
               InlineAlignment.ToCenter,
               InlineAlignment.ToBaseline,
               InlineAlignment.ToBottom,
               InlineAlignment.Top,
               InlineAlignment.Center,
               InlineAlignment.Bottom,
               InlineAlignment.ImageMask,
               InlineAlignment.TextMask,
             }));

            Add(dictionary, "EulerOrder", new GDEnumTypeInfo("", typeof(EulerOrder),
            new[]
            {
               "EULER_ORDER_XYZ",
               "EULER_ORDER_XZY",
               "EULER_ORDER_YXZ",
               "EULER_ORDER_YZX",
               "EULER_ORDER_ZXY",
               "EULER_ORDER_ZYX",
            },
            new[]
            {
               EulerOrder.Xyz,
               EulerOrder.Xzy,
               EulerOrder.Yxz,
               EulerOrder.Yzx,
               EulerOrder.Zxy,
               EulerOrder.Zyx,
            }));

            Add(dictionary, "Key", new GDEnumTypeInfo("", typeof(Key),
            new[]
            {
               "KEY_NONE",
               "KEY_SPECIAL",
               "KEY_ESCAPE",
               "KEY_TAB",
               "KEY_BACKTAB",
               "KEY_BACKSPACE",
               "KEY_ENTER",
               "KEY_KP_ENTER",
               "KEY_INSERT",
               "KEY_DELETE",
               "KEY_PAUSE",
               "KEY_PRINT",
               "KEY_SYSREQ",
               "KEY_CLEAR",
               "KEY_HOME",
               "KEY_END",
               "KEY_LEFT",
               "KEY_UP",
               "KEY_RIGHT",
               "KEY_DOWN",
               "KEY_PAGEUP",
               "KEY_PAGEDOWN",
               "KEY_SHIFT",
               "KEY_CTRL",
               "KEY_META",
               "KEY_ALT",
               "KEY_CAPSLOCK",
               "KEY_NUMLOCK",
               "KEY_SCROLLLOCK",
               "KEY_F1",
               "KEY_F2",
               "KEY_F3",
               "KEY_F4",
               "KEY_F5",
               "KEY_F6",
               "KEY_F7",
               "KEY_F8",
               "KEY_F9",
               "KEY_F10",
               "KEY_F11",
               "KEY_F12",
               "KEY_F13",
               "KEY_F14",
               "KEY_F15",
               "KEY_F16",
               "KEY_F17",
               "KEY_F18",
               "KEY_F19",
               "KEY_F20",
               "KEY_F21",
               "KEY_F22",
               "KEY_F23",
               "KEY_F24",
               "KEY_F25",
               "KEY_F26",
               "KEY_F27",
               "KEY_F28",
               "KEY_F29",
               "KEY_F30",
               "KEY_F31",
               "KEY_F32",
               "KEY_F33",
               "KEY_F34",
               "KEY_F35",
               "KEY_KP_MULTIPLY",
               "KEY_KP_DIVIDE",
               "KEY_KP_SUBTRACT",
               "KEY_KP_PERIOD",
               "KEY_KP_ADD",
               "KEY_KP_0",
               "KEY_KP_1",
               "KEY_KP_2",
               "KEY_KP_3",
               "KEY_KP_4",
               "KEY_KP_5",
               "KEY_KP_6",
               "KEY_KP_7",
               "KEY_KP_8",
               "KEY_KP_9",
               "KEY_MENU",
               "KEY_HYPER",
               "KEY_HELP",
               "KEY_BACK",
               "KEY_FORWARD",
               "KEY_STOP",
               "KEY_REFRESH",
               "KEY_VOLUMEDOWN",
               "KEY_VOLUMEMUTE",
               "KEY_VOLUMEUP",
               "KEY_MEDIAPLAY",
               "KEY_MEDIASTOP",
               "KEY_MEDIAPREVIOUS",
               "KEY_MEDIANEXT",
               "KEY_MEDIARECORD",
               "KEY_HOMEPAGE",
               "KEY_FAVORITES",
               "KEY_SEARCH",
               "KEY_STANDBY",
               "KEY_OPENURL",
               "KEY_LAUNCHMAIL",
               "KEY_LAUNCHMEDIA",
               "KEY_LAUNCH0",
               "KEY_LAUNCH1",
               "KEY_LAUNCH2",
               "KEY_LAUNCH3",
               "KEY_LAUNCH4",
               "KEY_LAUNCH5",
               "KEY_LAUNCH6",
               "KEY_LAUNCH7",
               "KEY_LAUNCH8",
               "KEY_LAUNCH9",
               "KEY_LAUNCHA",
               "KEY_LAUNCHB",
               "KEY_LAUNCHC",
               "KEY_LAUNCHD",
               "KEY_LAUNCHE",
               "KEY_LAUNCHF",
               "KEY_GLOBE",
               "KEY_KEYBOARD",
               "KEY_JIS_EISU",
               "KEY_JIS_KANA",
               "KEY_UNKNOWN",
               "KEY_SPACE",
               "KEY_EXCLAM",
               "KEY_QUOTEDBL",
               "KEY_NUMBERSIGN",
               "KEY_DOLLAR",
               "KEY_PERCENT",
               "KEY_AMPERSAND",
               "KEY_APOSTROPHE",
               "KEY_PARENLEFT",
               "KEY_PARENRIGHT",
               "KEY_ASTERISK",
               "KEY_PLUS",
               "KEY_COMMA",
               "KEY_MINUS",
               "KEY_PERIOD",
               "KEY_SLASH",
               "KEY_0",
               "KEY_1",
               "KEY_2",
               "KEY_3",
               "KEY_4",
               "KEY_5",
               "KEY_6",
               "KEY_7",
               "KEY_8",
               "KEY_9",
               "KEY_COLON",
               "KEY_SEMICOLON",
               "KEY_LESS",
               "KEY_EQUAL",
               "KEY_GREATER",
               "KEY_QUESTION",
               "KEY_AT",
               "KEY_A",
               "KEY_B",
               "KEY_C",
               "KEY_D",
               "KEY_E",
               "KEY_F",
               "KEY_G",
               "KEY_H",
               "KEY_I",
               "KEY_J",
               "KEY_K",
               "KEY_L",
               "KEY_M",
               "KEY_N",
               "KEY_O",
               "KEY_P",
               "KEY_Q",
               "KEY_R",
               "KEY_S",
               "KEY_T",
               "KEY_U",
               "KEY_V",
               "KEY_W",
               "KEY_X",
               "KEY_Y",
               "KEY_Z",
               "KEY_BRACKETLEFT",
               "KEY_BACKSLASH",
               "KEY_BRACKETRIGHT",
               "KEY_ASCIICIRCUM",
               "KEY_UNDERSCORE",
               "KEY_QUOTELEFT",
               "KEY_BRACELEFT",
               "KEY_BAR",
               "KEY_BRACERIGHT",
               "KEY_ASCIITILDE",
               "KEY_YEN",
               "KEY_SECTION",
            },
            new[]
            {
               Key.None,
               Key.Special,
               Key.Escape,
               Key.Tab,
               Key.Backtab,
               Key.Backspace,
               Key.Enter,
               Key.KpEnter,
               Key.Insert,
               Key.Delete,
               Key.Pause,
               Key.Print,
               Key.Sysreq,
               Key.Clear,
               Key.Home,
               Key.End,
               Key.Left,
               Key.Up,
               Key.Right,
               Key.Down,
               Key.Pageup,
               Key.Pagedown,
               Key.Shift,
               Key.Ctrl,
               Key.Meta,
               Key.Alt,
               Key.Capslock,
               Key.Numlock,
               Key.Scrolllock,
               Key.F1,
               Key.F2,
               Key.F3,
               Key.F4,
               Key.F5,
               Key.F6,
               Key.F7,
               Key.F8,
               Key.F9,
               Key.F10,
               Key.F11,
               Key.F12,
               Key.F13,
               Key.F14,
               Key.F15,
               Key.F16,
               Key.F17,
               Key.F18,
               Key.F19,
               Key.F20,
               Key.F21,
               Key.F22,
               Key.F23,
               Key.F24,
               Key.F25,
               Key.F26,
               Key.F27,
               Key.F28,
               Key.F29,
               Key.F30,
               Key.F31,
               Key.F32,
               Key.F33,
               Key.F34,
               Key.F35,
               Key.KpMultiply,
               Key.KpDivide,
               Key.KpSubtract,
               Key.KpPeriod,
               Key.KpAdd,
               Key.Kp0,
               Key.Kp1,
               Key.Kp2,
               Key.Kp3,
               Key.Kp4,
               Key.Kp5,
               Key.Kp6,
               Key.Kp7,
               Key.Kp8,
               Key.Kp9,
               Key.Menu,
               Key.Hyper,
               Key.Help,
               Key.Back,
               Key.Forward,
               Key.Stop,
               Key.Refresh,
               Key.Volumedown,
               Key.Volumemute,
               Key.Volumeup,
               Key.Mediaplay,
               Key.Mediastop,
               Key.Mediaprevious,
               Key.Medianext,
               Key.Mediarecord,
               Key.Homepage,
               Key.Favorites,
               Key.Search,
               Key.Standby,
               Key.Openurl,
               Key.Launchmail,
               Key.Launchmedia,
               Key.Launch0,
               Key.Launch1,
               Key.Launch2,
               Key.Launch3,
               Key.Launch4,
               Key.Launch5,
               Key.Launch6,
               Key.Launch7,
               Key.Launch8,
               Key.Launch9,
               Key.Launcha,
               Key.Launchb,
               Key.Launchc,
               Key.Launchd,
               Key.Launche,
               Key.Launchf,
               Key.Globe,
               Key.Keyboard,
               Key.JisEisu,
               Key.JisKana,
               Key.Unknown,
               Key.Space,
               Key.Exclam,
               Key.Quotedbl,
               Key.Numbersign,
               Key.Dollar,
               Key.Percent,
               Key.Ampersand,
               Key.Apostrophe,
               Key.Parenleft,
               Key.Parenright,
               Key.Asterisk,
               Key.Plus,
               Key.Comma,
               Key.Minus,
               Key.Period,
               Key.Slash,
               Key.Key0,
               Key.Key1,
               Key.Key2,
               Key.Key3,
               Key.Key4,
               Key.Key5,
               Key.Key6,
               Key.Key7,
               Key.Key8,
               Key.Key9,
               Key.Colon,
               Key.Semicolon,
               Key.Less,
               Key.Equal,
               Key.Greater,
               Key.Question,
               Key.At,
               Key.A,
               Key.B,
               Key.C,
               Key.D,
               Key.E,
               Key.F,
               Key.G,
               Key.H,
               Key.I,
               Key.J,
               Key.K,
               Key.L,
               Key.M,
               Key.N,
               Key.O,
               Key.P,
               Key.Q,
               Key.R,
               Key.S,
               Key.T,
               Key.U,
               Key.V,
               Key.W,
               Key.X,
               Key.Y,
               Key.Z,
               Key.Bracketleft,
               Key.Backslash,
               Key.Bracketright,
               Key.Asciicircum,
               Key.Underscore,
               Key.Quoteleft,
               Key.Braceleft,
               Key.Bar,
               Key.Braceright,
               Key.Asciitilde,
               Key.Yen,
               Key.Section,
            }));

            Add(dictionary, "KeyModifierMask", new GDEnumTypeInfo("", typeof(KeyModifierMask),
           new[]
           {
               "KEY_CODE_MASK",
               "KEY_MODIFIER_MASK",
               "KEY_MASK_CMD_OR_CTRL",
               "KEY_MASK_SHIFT",
               "KEY_MASK_ALT",
               "KEY_MASK_META",
               "KEY_MASK_CTRL",
               "KEY_MASK_KPAD",
               "KEY_MASK_GROUP_SWITCH",
           },
           new[]
           {
               KeyModifierMask.CodeMask,
               KeyModifierMask.ModifierMask,
               KeyModifierMask.MaskCmdOrCtrl,
               KeyModifierMask.MaskShift,
               KeyModifierMask.MaskAlt,
               KeyModifierMask.MaskMeta,
               KeyModifierMask.MaskCtrl,
               KeyModifierMask.MaskKpad,
               KeyModifierMask.MaskGroupSwitch,
           }));

            Add(dictionary, "MouseButton", new GDEnumTypeInfo("", typeof(MouseButton),
            new[]
            {
                   "MOUSE_BUTTON_NONE",
                   "MOUSE_BUTTON_LEFT",
                   "MOUSE_BUTTON_RIGHT",
                   "MOUSE_BUTTON_MIDDLE",
                   "MOUSE_BUTTON_WHEEL_UP",
                   "MOUSE_BUTTON_WHEEL_DOWN",
                   "MOUSE_BUTTON_WHEEL_LEFT",
                   "MOUSE_BUTTON_WHEEL_RIGHT",
                   "MOUSE_BUTTON_XBUTTON1",
                   "MOUSE_BUTTON_XBUTTON2",
            },
            new[]
            {
                   MouseButton.None,
                   MouseButton.Left,
                   MouseButton.Right,
                   MouseButton.Middle,
                   MouseButton.WheelUp,
                   MouseButton.WheelDown,
                   MouseButton.WheelLeft,
                   MouseButton.WheelRight,
                   MouseButton.Xbutton1,
                   MouseButton.Xbutton2,
            }));

            Add(dictionary, "MouseButtonMask", new GDEnumTypeInfo("", typeof(MouseButtonMask),
            new[]
            {
                   "MOUSE_BUTTON_MASK_LEFT",
                   "MOUSE_BUTTON_MASK_RIGHT",
                   "MOUSE_BUTTON_MASK_MIDDLE",
                   "MOUSE_BUTTON_MASK_MB_XBUTTON1",
                   "MOUSE_BUTTON_MASK_MB_XBUTTON2",
            },
            new[]
            {
                   MouseButtonMask.Left,
                   MouseButtonMask.Right,
                   MouseButtonMask.Middle,
                   MouseButtonMask.MbXbutton1,
                   MouseButtonMask.MbXbutton2,
            }));

            Add(dictionary, "JoyButton", new GDEnumTypeInfo("", typeof(JoyButton),
            new[]
            {
                   "JOY_BUTTON_INVALID",
                   "JOY_BUTTON_A",
                   "JOY_BUTTON_B",
                   "JOY_BUTTON_X",
                   "JOY_BUTTON_Y",
                   "JOY_BUTTON_BACK",
                   "JOY_BUTTON_GUIDE",
                   "JOY_BUTTON_START",
                   "JOY_BUTTON_LEFT_STICK",
                   "JOY_BUTTON_RIGHT_STICK",
                   "JOY_BUTTON_LEFT_SHOULDER",
                   "JOY_BUTTON_RIGHT_SHOULDER",
                   "JOY_BUTTON_DPAD_UP",
                   "JOY_BUTTON_DPAD_DOWN",
                   "JOY_BUTTON_DPAD_LEFT",
                   "JOY_BUTTON_DPAD_RIGHT",
                   "JOY_BUTTON_MISC1",
                   "JOY_BUTTON_PADDLE1",
                   "JOY_BUTTON_PADDLE2",
                   "JOY_BUTTON_PADDLE3",
                   "JOY_BUTTON_PADDLE4",
                   "JOY_BUTTON_TOUCHPAD",
                   "JOY_BUTTON_SDL_MAX",
                   "JOY_BUTTON_MAX",
            },
            new[]
            {
                   JoyButton.Invalid,
                   JoyButton.A,
                   JoyButton.B,
                   JoyButton.X,
                   JoyButton.Y,
                   JoyButton.Back,
                   JoyButton.Guide,
                   JoyButton.Start,
                   JoyButton.LeftStick,
                   JoyButton.RightStick,
                   JoyButton.LeftShoulder,
                   JoyButton.RightShoulder,
                   JoyButton.DpadUp,
                   JoyButton.DpadDown,
                   JoyButton.DpadLeft,
                   JoyButton.DpadRight,
                   JoyButton.Misc1,
                   JoyButton.Paddle1,
                   JoyButton.Paddle2,
                   JoyButton.Paddle3,
                   JoyButton.Paddle4,
                   JoyButton.Touchpad,
                   JoyButton.SdlMax,
                   JoyButton.Max,
            }));

            Add(dictionary, "JoyAxis", new GDEnumTypeInfo("", typeof(JoyAxis),
            new[]
            {
                   "JOY_AXIS_INVALID",
                   "JOY_AXIS_LEFT_X",
                   "JOY_AXIS_LEFT_Y",
                   "JOY_AXIS_RIGHT_X",
                   "JOY_AXIS_RIGHT_Y",
                   "JOY_AXIS_TRIGGER_LEFT",
                   "JOY_AXIS_TRIGGER_RIGHT",
                   "JOY_AXIS_SDL_MAX",
                   "JOY_AXIS_MAX",
              },
              new[]
              {
                   JoyAxis.Invalid,
                   JoyAxis.LeftX,
                   JoyAxis.LeftY,
                   JoyAxis.RightX,
                   JoyAxis.RightY,
                   JoyAxis.TriggerLeft,
                   JoyAxis.TriggerRight,
                   JoyAxis.SdlMax,
                   JoyAxis.Max,
            }));

            Add(dictionary, "MidiMessage", new GDEnumTypeInfo("", typeof(MidiMessage),
            new[]
            {
                   "MIDI_MESSAGE_NONE",
                   "MIDI_MESSAGE_NOTE_OFF",
                   "MIDI_MESSAGE_NOTE_ON",
                   "MIDI_MESSAGE_AFTERTOUCH",
                   "MIDI_MESSAGE_CONTROL_CHANGE",
                   "MIDI_MESSAGE_PROGRAM_CHANGE",
                   "MIDI_MESSAGE_CHANNEL_PRESSURE",
                   "MIDI_MESSAGE_PITCH_BEND",
                   "MIDI_MESSAGE_SYSTEM_EXCLUSIVE",
                   "MIDI_MESSAGE_QUARTER_FRAME",
                   "MIDI_MESSAGE_SONG_POSITION_POINTER",
                   "MIDI_MESSAGE_SONG_SELECT",
                   "MIDI_MESSAGE_TUNE_REQUEST",
                   "MIDI_MESSAGE_TIMING_CLOCK",
                   "MIDI_MESSAGE_START",
                   "MIDI_MESSAGE_CONTINUE",
                   "MIDI_MESSAGE_STOP",
                   "MIDI_MESSAGE_ACTIVE_SENSING",
                   "MIDI_MESSAGE_SYSTEM_RESET",
              },
              new[]
              {
                   MidiMessage.None,
                   MidiMessage.NoteOff,
                   MidiMessage.NoteOn,
                   MidiMessage.Aftertouch,
                   MidiMessage.ControlChange,
                   MidiMessage.ProgramChange,
                   MidiMessage.ChannelPressure,
                   MidiMessage.PitchBend,
                   MidiMessage.SystemExclusive,
                   MidiMessage.QuarterFrame,
                   MidiMessage.SongPositionPointer,
                   MidiMessage.SongSelect,
                   MidiMessage.TuneRequest,
                   MidiMessage.TimingClock,
                   MidiMessage.Start,
                   MidiMessage.Continue,
                   MidiMessage.Stop,
                   MidiMessage.ActiveSensing,
                   MidiMessage.SystemReset,
            }));

            Add(dictionary, "Error", new GDEnumTypeInfo("", typeof(Error),
           new[]
           {
                   "OK",
                   "FAILED",
                   "ERR_UNAVAILABLE",
                   "ERR_UNCONFIGURED",
                   "ERR_UNAUTHORIZED",
                   "ERR_PARAMETER_RANGE_ERROR",
                   "ERR_OUT_OF_MEMORY",
                   "ERR_FILE_NOT_FOUND",
                   "ERR_FILE_BAD_DRIVE",
                   "ERR_FILE_BAD_PATH",
                   "ERR_FILE_NO_PERMISSION",
                   "ERR_FILE_ALREADY_IN_USE",
                   "ERR_FILE_CANT_OPEN",
                   "ERR_FILE_CANT_WRITE",
                   "ERR_FILE_CANT_READ",
                   "ERR_FILE_UNRECOGNIZED",
                   "ERR_FILE_CORRUPT",
                   "ERR_FILE_MISSING_DEPENDENCIES",
                   "ERR_FILE_EOF",
                   "ERR_CANT_OPEN",
                   "ERR_CANT_CREATE",
                   "ERR_QUERY_FAILED",
                   "ERR_ALREADY_IN_USE",
                   "ERR_LOCKED",
                   "ERR_TIMEOUT",
                   "ERR_CANT_CONNECT",
                   "ERR_CANT_RESOLVE",
                   "ERR_CONNECTION_ERROR",
                   "ERR_CANT_ACQUIRE_RESOURCE",
                   "ERR_CANT_FORK",
                   "ERR_INVALID_DATA",
                   "ERR_INVALID_PARAMETER",
                   "ERR_ALREADY_EXISTS",
                   "ERR_DOES_NOT_EXIST",
                   "ERR_DATABASE_CANT_READ",
                   "ERR_DATABASE_CANT_WRITE",
                   "ERR_COMPILATION_FAILED",
                   "ERR_METHOD_NOT_FOUND",
                   "ERR_LINK_FAILED",
                   "ERR_SCRIPT_FAILED",
                   "ERR_CYCLIC_LINK",
                   "ERR_INVALID_DECLARATION",
                   "ERR_DUPLICATE_SYMBOL",
                   "ERR_PARSE_ERROR",
                   "ERR_BUSY",
                   "ERR_SKIP",
                   "ERR_HELP",
                   "ERR_BUG",
                   "ERR_PRINTER_ON_FIRE",
             },
             new[]
             {
                   Error.Ok,
                   Error.Failed,
                   Error.Unavailable,
                   Error.Unconfigured,
                   Error.Unauthorized,
                   Error.ParameterRangeError,
                   Error.OutOfMemory,
                   Error.FileNotFound,
                   Error.FileBadDrive,
                   Error.FileBadPath,
                   Error.FileNoPermission,
                   Error.FileAlreadyInUse,
                   Error.FileCantOpen,
                   Error.FileCantWrite,
                   Error.FileCantRead,
                   Error.FileUnrecognized,
                   Error.FileCorrupt,
                   Error.FileMissingDependencies,
                   Error.FileEof,
                   Error.CantOpen,
                   Error.CantCreate,
                   Error.QueryFailed,
                   Error.AlreadyInUse,
                   Error.Locked,
                   Error.Timeout,
                   Error.CantConnect,
                   Error.CantResolve,
                   Error.ConnectionError,
                   Error.CantAcquireResource,
                   Error.CantFork,
                   Error.InvalidData,
                   Error.InvalidParameter,
                   Error.AlreadyExists,
                   Error.DoesNotExist,
                   Error.DatabaseCantRead,
                   Error.DatabaseCantWrite,
                   Error.CompilationFailed,
                   Error.MethodNotFound,
                   Error.LinkFailed,
                   Error.ScriptFailed,
                   Error.CyclicLink,
                   Error.InvalidDeclaration,
                   Error.DuplicateSymbol,
                   Error.ParseError,
                   Error.Busy,
                   Error.Skip,
                   Error.Help,
                   Error.Bug,
                   Error.PrinterOnFire,
           }));

            Add(dictionary, "PropertyHint", new GDEnumTypeInfo("", typeof(PropertyHint),
    new[]
    {
               "PROPERTY_HINT_NONE",
               "PROPERTY_HINT_RANGE",
               "PROPERTY_HINT_ENUM",
               "PROPERTY_HINT_ENUM_SUGGESTION",
               "PROPERTY_HINT_EXP_EASING",
               "PROPERTY_HINT_LINK",
               "PROPERTY_HINT_FLAGS",
               "PROPERTY_HINT_LAYERS_2D_RENDER",
               "PROPERTY_HINT_LAYERS_2D_PHYSICS",
               "PROPERTY_HINT_LAYERS_2D_NAVIGATION",
               "PROPERTY_HINT_LAYERS_3D_RENDER",
               "PROPERTY_HINT_LAYERS_3D_PHYSICS",
               "PROPERTY_HINT_LAYERS_3D_NAVIGATION",
               "PROPERTY_HINT_LAYERS_AVOIDANCE",
               "PROPERTY_HINT_FILE",
               "PROPERTY_HINT_DIR",
               "PROPERTY_HINT_GLOBAL_FILE",
               "PROPERTY_HINT_GLOBAL_DIR",
               "PROPERTY_HINT_RESOURCE_TYPE",
               "PROPERTY_HINT_MULTILINE_TEXT",
               "PROPERTY_HINT_EXPRESSION",
               "PROPERTY_HINT_PLACEHOLDER_TEXT",
               "PROPERTY_HINT_COLOR_NO_ALPHA",
               "PROPERTY_HINT_OBJECT_ID",
               "PROPERTY_HINT_TYPE_STRING",
               "PROPERTY_HINT_NODE_PATH_TO_EDITED_NODE",
               "PROPERTY_HINT_OBJECT_TOO_BIG",
               "PROPERTY_HINT_NODE_PATH_VALID_TYPES",
               "PROPERTY_HINT_SAVE_FILE",
               "PROPERTY_HINT_GLOBAL_SAVE_FILE",
               "PROPERTY_HINT_INT_IS_OBJECTID",
               "PROPERTY_HINT_INT_IS_POINTER",
               "PROPERTY_HINT_ARRAY_TYPE",
               "PROPERTY_HINT_LOCALE_ID",
               "PROPERTY_HINT_LOCALIZABLE_STRING",
               "PROPERTY_HINT_NODE_TYPE",
               "PROPERTY_HINT_HIDE_QUATERNION_EDIT",
               "PROPERTY_HINT_PASSWORD",
               "PROPERTY_HINT_MAX",
    },
    new[]
    {
               PropertyHint.None,
               PropertyHint.Range,
               PropertyHint.Enum,
               PropertyHint.EnumSuggestion,
               PropertyHint.ExpEasing,
               PropertyHint.Link,
               PropertyHint.Flags,
               PropertyHint.Layers2DRender,
               PropertyHint.Layers2DPhysics,
               PropertyHint.Layers2DNavigation,
               PropertyHint.Layers3DRender,
               PropertyHint.Layers3DPhysics,
               PropertyHint.Layers3DNavigation,
               PropertyHint.LayersAvoidance,
               PropertyHint.File,
               PropertyHint.Dir,
               PropertyHint.GlobalFile,
               PropertyHint.GlobalDir,
               PropertyHint.ResourceType,
               PropertyHint.MultilineText,
               PropertyHint.Expression,
               PropertyHint.PlaceholderText,
               PropertyHint.ColorNoAlpha,
               PropertyHint.ObjectId,
               PropertyHint.TypeString,
               PropertyHint.NodePathToEditedNode,
               PropertyHint.ObjectTooBig,
               PropertyHint.NodePathValidTypes,
               PropertyHint.SaveFile,
               PropertyHint.GlobalSaveFile,
               PropertyHint.IntIsObjectid,
               PropertyHint.IntIsPointer,
               PropertyHint.ArrayType,
               PropertyHint.LocaleId,
               PropertyHint.LocalizableString,
               PropertyHint.NodeType,
               PropertyHint.HideQuaternionEdit,
               PropertyHint.Password,
               PropertyHint.Max,
    }));


            Add(dictionary, "PropertyUsageFlags", new GDEnumTypeInfo("", typeof(PropertyUsageFlags),
            new[]
            {
                   "PROPERTY_USAGE_NONE",
                   "PROPERTY_USAGE_STORAGE",
                   "PROPERTY_USAGE_EDITOR",
                   "PROPERTY_USAGE_INTERNAL",
                   "PROPERTY_USAGE_CHECKABLE",
                   "PROPERTY_USAGE_CHECKED",
                   "PROPERTY_USAGE_GROUP",
                   "PROPERTY_USAGE_CATEGORY",
                   "PROPERTY_USAGE_SUBGROUP",
                   "PROPERTY_USAGE_CLASS_IS_BITFIELD",
                   "PROPERTY_USAGE_NO_INSTANCE_STATE",
                   "PROPERTY_USAGE_RESTART_IF_CHANGED",
                   "PROPERTY_USAGE_SCRIPT_VARIABLE",
                   "PROPERTY_USAGE_STORE_IF_NULL",
                   "PROPERTY_USAGE_UPDATE_ALL_IF_MODIFIED",
                   "PROPERTY_USAGE_SCRIPT_DEFAULT_VALUE",
                   "PROPERTY_USAGE_CLASS_IS_ENUM",
                   "PROPERTY_USAGE_NIL_IS_VARIANT",
                   "PROPERTY_USAGE_ARRAY",
                   "PROPERTY_USAGE_ALWAYS_DUPLICATE",
                   "PROPERTY_USAGE_NEVER_DUPLICATE",
                   "PROPERTY_USAGE_HIGH_END_GFX",
                   "PROPERTY_USAGE_NODE_PATH_FROM_SCENE_ROOT",
                   "PROPERTY_USAGE_RESOURCE_NOT_PERSISTENT",
                   "PROPERTY_USAGE_KEYING_INCREMENTS",
                   "PROPERTY_USAGE_DEFERRED_SET_RESOURCE",
                   "PROPERTY_USAGE_EDITOR_INSTANTIATE_OBJECT",
                   "PROPERTY_USAGE_EDITOR_BASIC_SETTING",
                   "PROPERTY_USAGE_READ_ONLY",
                   "PROPERTY_USAGE_SECRET",
                   "PROPERTY_USAGE_DEFAULT",
                   "PROPERTY_USAGE_NO_EDITOR",
              },
              new[]
              {
                   PropertyUsageFlags.None,
                   PropertyUsageFlags.Storage,
                   PropertyUsageFlags.Editor,
                   PropertyUsageFlags.Internal,
                   PropertyUsageFlags.Checkable,
                   PropertyUsageFlags.Checked,
                   PropertyUsageFlags.Group,
                   PropertyUsageFlags.Category,
                   PropertyUsageFlags.Subgroup,
                   PropertyUsageFlags.ClassIsBitfield,
                   PropertyUsageFlags.NoInstanceState,
                   PropertyUsageFlags.RestartIfChanged,
                   PropertyUsageFlags.ScriptVariable,
                   PropertyUsageFlags.StoreIfNull,
                   PropertyUsageFlags.UpdateAllIfModified,
                   PropertyUsageFlags.ScriptDefaultValue,
                   PropertyUsageFlags.ClassIsEnum,
                   PropertyUsageFlags.NilIsVariant,
                   PropertyUsageFlags.Array,
                   PropertyUsageFlags.AlwaysDuplicate,
                   PropertyUsageFlags.NeverDuplicate,
                   PropertyUsageFlags.HighEndGfx,
                   PropertyUsageFlags.NodePathFromSceneRoot,
                   PropertyUsageFlags.ResourceNotPersistent,
                   PropertyUsageFlags.KeyingIncrements,
                   PropertyUsageFlags.DeferredSetResource,
                   PropertyUsageFlags.EditorInstantiateObject,
                   PropertyUsageFlags.EditorBasicSetting,
                   PropertyUsageFlags.ReadOnly,
                   PropertyUsageFlags.Secret,
                   PropertyUsageFlags.Default,
                   PropertyUsageFlags.NoEditor,
            }));

            Add(dictionary, "MethodFlags", new GDEnumTypeInfo("", typeof(MethodFlags),
            new[]
            {
                   "METHOD_FLAG_NORMAL",
                   "METHOD_FLAG_EDITOR",
                   "METHOD_FLAG_CONST",
                   "METHOD_FLAG_VIRTUAL",
                   "METHOD_FLAG_VARARG",
                   "METHOD_FLAG_STATIC",
                   "METHOD_FLAG_OBJECT_CORE",
                   "METHOD_FLAGS_DEFAULT",
              },
              new[]
              {
                   MethodFlags.Normal,
                   MethodFlags.Editor,
                   MethodFlags.Const,
                   MethodFlags.Virtual,
                   MethodFlags.Vararg,
                   MethodFlags.Static,
                   MethodFlags.ObjectCore,
                   MethodFlags.Default,
            }));

            Add(dictionary, "Type", new GDEnumTypeInfo("", typeof(Variant.Type),
            new[]
            {
                   "TYPE_NIL",
                   "TYPE_BOOL",
                   "TYPE_INT",
                   "TYPE_FLOAT",
                   "TYPE_STRING",
                   "TYPE_VECTOR2",
                   "TYPE_VECTOR2I",
                   "TYPE_RECT2",
                   "TYPE_RECT2I",
                   "TYPE_VECTOR3",
                   "TYPE_VECTOR3I",
                   "TYPE_TRANSFORM2D",
                   "TYPE_VECTOR4",
                   "TYPE_VECTOR4I",
                   "TYPE_PLANE",
                   "TYPE_QUATERNION",
                   "TYPE_AABB",
                   "TYPE_BASIS",
                   "TYPE_TRANSFORM3D",
                   "TYPE_PROJECTION",
                   "TYPE_COLOR",
                   "TYPE_STRING_NAME",
                   "TYPE_NODE_PATH",
                   "TYPE_RID",
                   "TYPE_OBJECT",
                   "TYPE_CALLABLE",
                   "TYPE_SIGNAL",
                   "TYPE_DICTIONARY",
                   "TYPE_ARRAY",
                   "TYPE_PACKED_BYTE_ARRAY",
                   "TYPE_PACKED_INT32_ARRAY",
                   "TYPE_PACKED_INT64_ARRAY",
                   "TYPE_PACKED_FLOAT32_ARRAY",
                   "TYPE_PACKED_FLOAT64_ARRAY",
                   "TYPE_PACKED_STRING_ARRAY",
                   "TYPE_PACKED_VECTOR2_ARRAY",
                   "TYPE_PACKED_VECTOR3_ARRAY",
                   "TYPE_PACKED_COLOR_ARRAY",
                   "TYPE_MAX",
              },
              new[]
              {
                   Variant.Type.Nil,
                   Variant.Type.Bool,
                   Variant.Type.Int,
                   Variant.Type.Float,
                   Variant.Type.String,
                   Variant.Type.Vector2,
                   Variant.Type.Vector2I,
                   Variant.Type.Rect2,
                   Variant.Type.Rect2I,
                   Variant.Type.Vector3,
                   Variant.Type.Vector3I,
                   Variant.Type.Transform2D,
                   Variant.Type.Vector4,
                   Variant.Type.Vector4I,
                   Variant.Type.Plane,
                   Variant.Type.Quaternion,
                   Variant.Type.Aabb,
                   Variant.Type.Basis,
                   Variant.Type.Transform3D,
                   Variant.Type.Projection,
                   Variant.Type.Color,
                   Variant.Type.StringName,
                   Variant.Type.NodePath,
                   Variant.Type.Rid,
                   Variant.Type.Object,
                   Variant.Type.Callable,
                   Variant.Type.Signal,
                   Variant.Type.Dictionary,
                   Variant.Type.Array,
                   Variant.Type.PackedByteArray,
                   Variant.Type.PackedInt32Array,
                   Variant.Type.PackedInt64Array,
                   Variant.Type.PackedFloat32Array,
                   Variant.Type.PackedFloat64Array,
                   Variant.Type.PackedStringArray,
                   Variant.Type.PackedVector2Array,
                   Variant.Type.PackedVector3Array,
                   Variant.Type.PackedColorArray,
                   Variant.Type.Max,
            }));

            Add(dictionary, "Operator", new GDEnumTypeInfo("", typeof(Variant.Operator),
           new[]
           {
                   "OP_EQUAL",
                   "OP_NOT_EQUAL",
                   "OP_LESS",
                   "OP_LESS_EQUAL",
                   "OP_GREATER",
                   "OP_GREATER_EQUAL",
                   "OP_ADD",
                   "OP_SUBTRACT",
                   "OP_MULTIPLY",
                   "OP_DIVIDE",
                   "OP_NEGATE",
                   "OP_POSITIVE",
                   "OP_MODULE",
                   "OP_POWER",
                   "OP_SHIFT_LEFT",
                   "OP_SHIFT_RIGHT",
                   "OP_BIT_AND",
                   "OP_BIT_OR",
                   "OP_BIT_XOR",
                   "OP_BIT_NEGATE",
                   "OP_AND",
                   "OP_OR",
                   "OP_XOR",
                   "OP_NOT",
                   "OP_IN",
                   "OP_MAX",
             },
             new[]
             {
                   Variant.Operator.Equal,
                   Variant.Operator.NotEqual,
                   Variant.Operator.Less,
                   Variant.Operator.LessEqual,
                   Variant.Operator.Greater,
                   Variant.Operator.GreaterEqual,
                   Variant.Operator.Add,
                   Variant.Operator.Subtract,
                   Variant.Operator.Multiply,
                   Variant.Operator.Divide,
                   Variant.Operator.Negate,
                   Variant.Operator.Positive,
                   Variant.Operator.Module,
                   Variant.Operator.Power,
                   Variant.Operator.ShiftLeft,
                   Variant.Operator.ShiftRight,
                   Variant.Operator.BitAnd,
                   Variant.Operator.BitOr,
                   Variant.Operator.BitXor,
                   Variant.Operator.BitNegate,
                   Variant.Operator.And,
                   Variant.Operator.Or,
                   Variant.Operator.Xor,
                   Variant.Operator.Not,
                   Variant.Operator.In,
                   Variant.Operator.Max,
             }));
        }
    }
}
