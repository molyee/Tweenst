using System;
using EasyFunc = System.Func<float, float>;

namespace Alsoft.Tweest
{
    /// <summary>
    /// // TODO default tweens configuration (to remove or change)
    /// </summary>
    public static class Tween
    {
        private static IInterpolantManager _interpolantManager;
        /// <summary>
        /// Gets or sets the interpolant manager.
        /// </summary>
        /// <value>The interpolant manager.</value>
        public static IInterpolantManager InterpolantManager
        {
            // TODO some syntax problems with Mono
            get { return _interpolantManager ?? (_interpolantManager = new InterpolantManager()); }
            set { _interpolantManager = value; }
        }
    }

    /// <summary>
    /// // TODO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tween<T> : ITween
    {
        /// <summary>
        /// Tween complete event
        /// </summary>
        public event Action<ITween> OnComplete = delegate { };

        /// <summary>
        /// Gets the type of the easy.
        /// </summary>
        /// <value>The type of the easy.</value>
        public Easy EasyType { get; private set; }

        /// <summary>
        /// Gets from.
        /// </summary>
        /// <value>From.</value>
        public T From { get; private set; }

        /// <summary>
        /// Gets to.
        /// </summary>
        /// <value>To.</value>
        public T To { get; private set; }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public float Duration { get; private set; }
        
        /// <summary>
        /// Gets the elapsed.
        /// </summary>
        /// <value>The elapsed.</value>
        public float Elapsed { get; private set; }

        /// <summary>
        /// Gets the easing func.
        /// </summary>
        /// <value>The easing func.</value>
        public EasyFunc EasingFunc { get; private set; }
        
        private Action<T> _updateTarget;

        private Func<T, T, float, T> _lerp;

        /// <summary>
        /// Initializes a new instance of the <see cref="Alsoft.Tweest.Tween`1"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="updateTarget">Update target.</param>
        /// <param name="easingFuncName">Easing func name.</param>
        /// <param name="easyType">Easy type.</param>
        public Tween(T from, T to, float duration, Action<T> updateTarget, string easingFuncName,
            Easy easyType = Easy.In)
            : this(from, to, duration, updateTarget, Easing.Create(easingFuncName), easyType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Alsoft.Tweest.Tween`1"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="updateTarget">Update target.</param>
        /// <param name="easingFunc">Easing func.</param>
        /// <param name="easyType">Easy type.</param>
        public Tween(T from, T to, float duration, Action<T> updateTarget, EasyFunc easingFunc,
            Easy easyType = Easy.In)
        {
            Init(from, to, duration, updateTarget, easingFunc, easyType);
        }

        protected void Init(T from, T to, float duration, Action<T> updateTarget, EasyFunc easingFunc,
            Easy easyType = Easy.In)
        {
#if DEBUG
            if (updateTarget == null) throw new ArgumentNullException("updateTarget");
            if (easingFunc == null) throw new ArgumentNullException("easingFunc");
#endif
            From = from;
            To = to;
            Duration = duration;
            EasingFunc = easingFunc;
            EasyType = easyType;

            _updateTarget = updateTarget;
            _lerp = Tween.InterpolantManager.GetLerp<T>();
        }

        /// <summary>
        /// Tween update to next position
        /// </summary>
        /// <param name="deltaTime">Time changes</param>
        public void Update(float deltaTime)
        {
#if DEBUG
            if (Completed) throw new InvalidOperationException("Tween is already completed and can't be updated");
            if (Disposed) throw new InvalidOperationException("Tween is already disposed and can't be updated");
#endif
            Elapsed += deltaTime;
            var elapsedPart = Elapsed / Duration;
            var frac = GetRelative(elapsedPart > 1f ? 1f : elapsedPart);
            var value = _lerp(From, To, frac);
            _updateTarget(value);
        }

        private float GetRelative(float frac)
        {
            if (EasyType == Easy.In) return EasingFunc(frac);
            if (EasyType == Easy.Out) return 1f - EasingFunc(1f - frac);
            if (EasyType == Easy.InOut) return (frac < 0.5f) ? EasingFunc(2f * frac) / 2f : 0.5f + EasingFunc(2f * frac - 1f) / 2f;
            if (EasyType == Easy.OutIn) return (frac < 0.5f) ? EasingFunc(2f * frac - 1f) / 2f : 0.5f + EasingFunc(2f * frac) / 2f;
            throw new NotImplementedException("Translator for easy type " + EasyType + " isn't found");
        }

        /// <summary>
        /// Force tween to last position and complete tweening
        /// </summary>
        public void Complete()
        {
#if DEBUG
            if (Completed) throw new InvalidOperationException("Tween is already completed");
            if (Disposed) throw new InvalidOperationException("Tween is already disposed and can't be completed");
#endif
            _updateTarget(To);
            Completed = true;
            OnComplete(this);
        }

        /// <summary>
        /// Check tween completed
        /// </summary>
        /// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
        public bool Completed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Alsoft.Tweest.Tween`1"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="Alsoft.Tweest.Tween`1"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the <see cref="Alsoft.Tweest.Tween`1"/> so
        /// the garbage collector can reclaim the memory that the <see cref="Alsoft.Tweest.Tween`1"/> was occupying.</remarks>
        public void Dispose()
        {
#if DEBUG
            if (Disposed) throw new InvalidOperationException("Tween is already disposed");
#endif
            _updateTarget = null;
            _lerp = null;
        }

        /// <summary>
        /// Check tween disposed
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed { get { return _updateTarget == null; } }
    }
}
