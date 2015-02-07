using System;
using System.Collections.Generic;

namespace Alsoft.Tweest
{
    /// <summary>
    /// Tween group combines tweens or tween groups in one container. It makes easy
    /// to run and stop multiple tweens in one time
    /// </summary>
    public class TweenGroup : ITween
    {
        /// <summary>
        /// Tween complete event
        /// </summary>
        public event Action<ITween> OnComplete = delegate { };

        List<ITween> _tweens;

        /// <summary>
        /// Initializes a new instance of the <see cref="Alsoft.Tweest.TweenGroup"/> class.
        /// </summary>
        /// <param name="tweens">Tweens.</param>
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

        /// <summary>
        /// Add the specified tween.
        /// </summary>
        /// <param name="tween">Tween.</param>
        public TweenGroup Add(ITween tween)
        {
            AddChild(tween);
            return this;
        }

        /// <summary>
        /// Tween update to next position
        /// </summary>
        /// <param name="deltaTime">Time changes</param>
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

        /// <summary>
        /// Force tween to last position and complete tweening
        /// </summary>
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

        /// <summary>
        /// Check tween completed
        /// </summary>
        /// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
        public bool Completed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Alsoft.Tweest.TweenGroup"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="Alsoft.Tweest.TweenGroup"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the <see cref="Alsoft.Tweest.TweenGroup"/>
        /// so the garbage collector can reclaim the memory that the <see cref="Alsoft.Tweest.TweenGroup"/> was occupying.</remarks>
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

        /// <summary>
        /// Check tween disposed
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed { get; private set; }
    }
}
