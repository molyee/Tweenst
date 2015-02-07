using System;
using EasyFunc = System.Func<float, float>;

namespace Alsoft.Tweest
{
    // TODO create tween functions drawer

    /// <summary>
    /// // TODO
    /// </summary>
    public enum Easy
    {
        In,
        Out,
        InOut,
        OutIn
    }

    /// <summary>
    /// // TODO
    /// </summary>
    static public class Easing
    {
        static EasyFunc _lerp;
        static public EasyFunc Lerp { get { return _lerp ?? (_lerp = CreateLerp()); } }

        static EasyFunc _none;
        static public EasyFunc None { get { return _none ?? (_none = CreateNone()); } }

        static EasyFunc _quadratic;
        static public EasyFunc Quadratic { get { return _quadratic ?? (_quadratic = CreateQuadratic()); } }

        static EasyFunc _cubic;
        static public EasyFunc Cubic { get { return _cubic ?? (_cubic = CreateCubic()); } }

        static EasyFunc _sine;
        static public EasyFunc Sine { get { return _sine ?? (_sine = CreateSine()); } }

        static EasyFunc _exponent;
        static public EasyFunc Exponent { get { return _exponent ?? (_exponent = CreateExponent()); } }

        static EasyFunc _circle;
        static public EasyFunc Circle { get { return _circle ?? (_circle = CreateCircle()); } }

        static EasyFunc _elastic;
        static public EasyFunc Elastic { get { return _elastic ?? (_elastic = CreateElastic()); } } 

        // -- easing factories --

        static public EasyFunc Create(string easingName)
        {
            switch (easingName)
            {
                case "lerp": return CreateLerp();
                case "none": return CreateNone();
                case "quadratic": return CreateQuadratic();
                case "cubic": return CreateCubic();
                case "quartic": return CreateQuadratic();
                case "quintic": return CreateQuintic();
                case "sine": return CreateSine();
                case "exponent": return CreateExponent();
                case "circle": return CreateCircle();
                case "elastic": return CreateElastic();
                default: throw new NotSupportedException(
                    "Easy function '" + easingName + "' is not supported");
            }
        }

        // ------

        static public EasyFunc CreateLerp()
        {
            return t => t;
        }

        static public EasyFunc CreateNone()
        {
            return t => 0;
        }

        static public EasyFunc CreateQuadratic()
        {
            return t => t * t;
        }

        static public EasyFunc CreateCubic()
        {
            return t => t * t * t;
        }

        static public EasyFunc CreateQuartic()
        {
            return t => t * t * t * t;
        }

        static public EasyFunc CreateQuintic()
        {
            return t => t * t * t * t * t;
        }

        static public EasyFunc CreateSine()
        {
            return t => (float)(1.0 - Math.Cos(t * Math.PI / 2.0));
        }

        static public EasyFunc CreateExponent()
        {
            return t => 0f == t  ? 0f : (float)Math.Pow(2.0, 10.0 * ((double)t - 1.0));
        }

        static public EasyFunc CreateCircle()
        {
            return t => (float)(1.0 - Math.Sqrt(1.0 - t * t));
        }

        static public EasyFunc CreateElastic()
        {
            return t => -(float)(Math.Pow(2.0, 10.0 * (t - 1.0)) *
                                 Math.Sin(((t - 1.0) - 0.75) * 0.5 * Math.PI));
        }
    }
}
