using System;
using EasyFunc = System.Func<float, float>;

namespace Alsoft.Tweenst
{
    /// <summary>
    /// // TODO default tweens configuration (to remove or change)
    /// </summary>
    public static class Tween
    {
        private static IInterpolantManager _interpolantManager;
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
        public event Action<ITween> OnComplete = delegate { };

        public Easy EasyType { get; private set; }

        public T From { get; private set; }

        public T To { get; private set; }

        public float Duration { get; private set; }
        
        public float Elapsed { get; private set; }

        public EasyFunc EasingFunc { get; private set; }
        
        private Action<T> _updateTarget;

        private Func<T, T, float, T> _lerp;

        public Tween(T from, T to, float duration, Action<T> updateTarget, string easingFuncName,
            Easy easyType = Easy.In)
            : this(from, to, duration, updateTarget, Easing.Create(easingFuncName), easyType)
        {
        }

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

        public bool Completed { get; private set; }

        public void Dispose()
        {
#if DEBUG
            if (Disposed) throw new InvalidOperationException("Tween is already disposed");
#endif
            _updateTarget = null;
            _lerp = null;
        }

        public bool Disposed { get { return _updateTarget == null; } }
    }
}
