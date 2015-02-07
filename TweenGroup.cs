using System;
using System.Collections.Generic;

namespace Alsoft.Tweenst
{
    /// <summary>
    /// Tween controllers group for managing all tween controllers
    /// in one hand. You can use this class as global tween manager
    /// or as local tween controllers container
    /// </summary>
    public class TweenGroup : ITween
    {
        public event Action<ITween> OnComplete = delegate { };

        List<ITween> _tweens;

        public TweenGroup(IEnumerable<ITween> tweens = null)
        {
            _tweens = new List<ITween>();
            if (tweens != null)
                foreach (var tween in tweens)
                    AddChild(tween);
        }

        protected void AddChild(ITween tween)
        {
            _tweens.Add(tween);
            tween.OnComplete += RemoveChild;
        }

        protected void RemoveChild(ITween tween)
        {
            tween.OnComplete -= RemoveChild;
            _tweens.Remove(tween);
        }

        public TweenGroup Add(ITween tween)
        {
            AddChild(tween);
            return this;
        }

        public void Update(float deltaTime)
        {
#if DEBUG
            if (Completed) throw new InvalidOperationException("TweenGroup is already completed and can't be updated");
            if (Disposed) throw new InvalidOperationException("TweenGroup is already disposed and can't be updated");
            if (_tweens == null) throw new InvalidOperationException("There is no any tween in TweenGroup");
#endif
            var len = _tweens.Count;
            for (var i = 0; i < len; ++i)
            {
                _tweens[i].Update(deltaTime);
            }

            if (_tweens.Count == 0)
            {
                Complete();
            }
        }

        public void Complete()
        {
#if DEBUG
            if (Completed) throw new InvalidOperationException("TweenGroup is already completed");
            if (Disposed) throw new InvalidOperationException("TweenGroup is already disposed and can't be completed");
#endif
            if (_tweens != null)
            {
                var len = _tweens.Count;
                for (var i = 0; i < len; ++i)
                {
                    _tweens[i].Complete();
                }
            }
            Completed = true;
            OnComplete(this);
        }

        public bool Completed { get; private set; }

        public void Dispose()
        {
#if DEBUG
            if (Disposed) throw new InvalidOperationException("TweenGroup is already disposed");
#endif
            if (_tweens != null)
            {
                var index = _tweens.Count;
                while (index-- > 0)
                {
                    var tween = _tweens[index];
                    RemoveChild(tween);
                    tween.Dispose();
                }
                _tweens = null;
            }
            Disposed = true;
        }

        public bool Disposed { get; private set; }
    }
}
