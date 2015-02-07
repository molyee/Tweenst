using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace Alsoft.Tweenst
{
    /// <summary>Interface for using custom interpolate functions</summary>
    public interface IInterpolantManager
    {
        void Bind<T>(Func<T, T, float, T> lerpFunc);
        Func<T, T, float, T> GetLerp<T>();
    }

    /// <summary>
    /// // TODO
    /// </summary>
    public class InterpolantManager : IInterpolantManager
    {
        readonly Dictionary<Type, Delegate> _interpolants;

        public InterpolantManager()
        {
            _interpolants = new Dictionary<Type, Delegate>();
            Bind<byte>(LerpByte);
            Bind<sbyte>(LerpSByte);
            Bind<int>(LerpInt32);
            Bind<uint>(LerpUInt32);
            Bind<long>(LerpInt64);
            Bind<ulong>(LerpUInt64);
            Bind<float>(LerpSingle);
            Bind<double>(LerpDouble);
#if UNITY
            Bind<Vector2>(Vector2.Lerp);
            Bind<Vector3>(Vector3.Lerp);
            Bind<Vector4>(Vector4.Lerp);
            Bind<Quaternion>(Quaternion.Slerp);
            Bind<Rect>(LerpRect);
            Bind<Color>(Color.Lerp);
            Bind<Color32>(Color32.Lerp);
#endif
        }

        public void Bind<T>(Func<T, T, float, T> lerp)
        {
            _interpolants.Add(typeof(T), lerp);
        }

        public Func<T, T, float, T> GetLerp<T>()
        {
            Type type = typeof(T);
            if (_interpolants.ContainsKey(type))
                return (Func<T, T, float, T>)_interpolants[type];
            throw new NotSupportedException("Type " + type + " is not supported yet");
        }

        // -- Linear interpolate functions

        public byte LerpByte(byte s, byte e, float f) { return (byte)(s + (e - s) * f); }
        public sbyte LerpSByte(sbyte s, sbyte e, float f) { return (sbyte)(s + (e - s) * f); }
        public int LerpInt32(int s, int e, float f) { return s + (int)((e - s) * f); }
        public uint LerpUInt32(uint s, uint e, float f) { return s + (uint)((e - s) * f); }
        public long LerpInt64(long s, long e, float f) { return s + (long)((e - s) * f); }
        public ulong LerpUInt64(ulong s, ulong e, float f) { return s + (ulong)((e - s) * f); }
        public float LerpSingle(float s, float e, float f) { return s + (e - s) * f; }
        public double LerpDouble(double s, double e, float f) { return s + (e - s) * f; }
#if UNITY
        public Rect LerpRect(Rect s, Rect e, float f)
        {
            var x = LerpSingle(s.x, e.x, f);
            var y = LerpSingle(s.y, e.y, f);
            var w = LerpSingle(s.width, e.width, f);
            var h = LerpSingle(s.height, e.height, f);
            return new Rect(x, y, w, h);
        }
#endif
    }
}
