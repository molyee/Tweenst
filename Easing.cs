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
        /// <summary>
        /// The in.
        /// </summary>
        In,
        /// <summary>
        /// The out.
        /// </summary>
        Out,
        /// <summary>
        /// The in out.
        /// </summary>
        InOut,
        /// <summary>
        /// The out in.
        /// </summary>
        OutIn
    }

    /// <summary>
    /// // TODO
    /// </summary>
    static public class Easing
    {
        static EasyFunc _lerp;
        /// <summary>
        /// Gets the lerp.
        /// </summary>
        /// <value>The lerp.</value>
        static public EasyFunc Lerp { get { return _lerp ?? (_lerp = CreateLerp()); } }

        static EasyFunc _none;
        /// <summary>
        /// Gets the none.
        /// </summary>
        /// <value>The none.</value>
        static public EasyFunc None { get { return _none ?? (_none = CreateNone()); } }

        static EasyFunc _quadratic;
        /// <summary>
        /// Gets the quadratic.
        /// </summary>
        /// <value>The quadratic.</value>
        static public EasyFunc Quadratic { get { return _quadratic ?? (_quadratic = CreateQuadratic()); } }

        static EasyFunc _cubic;
        /// <summary>
        /// Gets the cubic.
        /// </summary>
        /// <value>The cubic.</value>
        static public EasyFunc Cubic { get { return _cubic ?? (_cubic = CreateCubic()); } }

        static EasyFunc _sine;
        /// <summary>
        /// Gets the sine.
        /// </summary>
        /// <value>The sine.</value>
        static public EasyFunc Sine { get { return _sine ?? (_sine = CreateSine()); } }

        static EasyFunc _exponent;
        /// <summary>
        /// Gets the exponent.
        /// </summary>
        /// <value>The exponent.</value>
        static public EasyFunc Exponent { get { return _exponent ?? (_exponent = CreateExponent()); } }

        static EasyFunc _circle;
        /// <summary>
        /// Gets the circle.
        /// </summary>
        /// <value>The circle.</value>
        static public EasyFunc Circle { get { return _circle ?? (_circle = CreateCircle()); } }

        static EasyFunc _elastic;
        /// <summary>
        /// Gets the elastic.
        /// </summary>
        /// <value>The elastic.</value>
        static public EasyFunc Elastic { get { return _elastic ?? (_elastic = CreateElastic()); } } 

        // -- easing factories --

        /// <summary>
        /// Create the specified easingName.
        /// </summary>
        /// <param name="easingName">Easing name.</param>
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

        /// <summary>
        /// Creates the lerp.
        /// </summary>
        /// <returns>The lerp.</returns>
        static public EasyFunc CreateLerp()
        {
            return t => t;
        }

        /// <summary>
        /// Creates the none.
        /// </summary>
        /// <returns>The none.</returns>
        static public EasyFunc CreateNone()
        {
            return t => 0;
        }

        /// <summary>
        /// Creates the quadratic.
        /// </summary>
        /// <returns>The quadratic.</returns>
        static public EasyFunc CreateQuadratic()
        {
            return t => t * t;
        }

        /// <summary>
        /// Creates the cubic.
        /// </summary>
        /// <returns>The cubic.</returns>
        static public EasyFunc CreateCubic()
        {
            return t => t * t * t;
        }

        /// <summary>
        /// Creates the quartic.
        /// </summary>
        /// <returns>The quartic.</returns>
        static public EasyFunc CreateQuartic()
        {
            return t => t * t * t * t;
        }

        /// <summary>
        /// Creates the quintic.
        /// </summary>
        /// <returns>The quintic.</returns>
        static public EasyFunc CreateQuintic()
        {
            return t => t * t * t * t * t;
        }

        /// <summary>
        /// Creates the sine.
        /// </summary>
        /// <returns>The sine.</returns>
        static public EasyFunc CreateSine()
        {
            return t => (float)(1.0 - Math.Cos(t * Math.PI / 2.0));
        }

        /// <summary>
        /// Creates the exponent.
        /// </summary>
        /// <returns>The exponent.</returns>
        static public EasyFunc CreateExponent()
        {
            return t => 0f == t  ? 0f : (float)Math.Pow(2.0, 10.0 * ((double)t - 1.0));
        }

        /// <summary>
        /// Creates the circle.
        /// </summary>
        /// <returns>The circle.</returns>
        static public EasyFunc CreateCircle()
        {
            return t => (float)(1.0 - Math.Sqrt(1.0 - t * t));
        }

        /// <summary>
        /// Creates the elastic.
        /// </summary>
        /// <returns>The elastic.</returns>
        static public EasyFunc CreateElastic()
        {
            return t => -(float)(Math.Pow(2.0, 10.0 * (t - 1.0)) *
                                 Math.Sin(((t - 1.0) - 0.75) * 0.5 * Math.PI));
        }
    }
}
