using System;

namespace Alsoft.Tweenst
{
    /// <summary>Tween interface</summary>
    public interface ITween : IDisposable
    {
        /// <summary>Tween complete event</summary>
        event Action<ITween> OnComplete;

        /// <summary>Tween update to next position</summary>
        /// <param name="deltaTime">Time changes</param>
        void Update(float deltaTime);

        /// <summary>Force tween to last position and complete tweening</summary>
        void Complete();

        /// <summary>Check tween completed</summary>
        bool Completed { get; }

        /// <summary>Check tween disposed</summary>
        bool Disposed { get; }
    }
}
