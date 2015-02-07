using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alsoft.Tweenst
{
    /// <summary>Simple tweens controller</summary>
    public class Tweener : ITweener
    {
        /// <summary>Tween speed multiplier</summary>
        public float TimeSpeedMultiplier { get; set; }

        private readonly Dictionary<ITween, object> _tweens;
        private readonly Stopwatch _timer;
        private double _lastElapsedMsec;

        /// <summary>Tweens controller constructor</summary>
        /// <param name="timeSpeedMultiplier">Animation playback rate</param>
        public Tweener(float timeSpeedMultiplier = 1f)
        {
            _timer = new Stopwatch();
            _tweens = new Dictionary<ITween, object>();
            TimeSpeedMultiplier = timeSpeedMultiplier;
        }

        /// <summary>Add new tween</summary>
        /// <param name="tween">Tween</param>
        /// <param name="locker">Tween locker (owner)</param>
        public void Add(ITween tween, object locker = null)
        {
            object currentLocker;
            if (_tweens.TryGetValue(tween, out currentLocker))
                throw new InvalidOperationException("Tween is already added");
            tween.OnComplete += Delete;
            _tweens.Add(tween, locker);
        }

        /// <summary>Remove and dispose tween (with locker checking)</summary>
        /// <param name="tween">Tween to remove</param>
        /// <param name="locker">Tween locker (owner)</param>
        public void Remove(ITween tween, object locker = null)
        {
            object currentLocker;
            if (!_tweens.TryGetValue(tween, out currentLocker))
                return;
            if (locker != currentLocker)
                throw new InvalidOperationException(
                    "Unable to remove tween, as it was added with another locker object: locked by "
                     + currentLocker + ", removing with " + locker);
            Delete(tween);
        }

        /// <summary>Remove from list and dispose tween</summary>
        /// <param name="tween">Tween to remove</param>
        protected void Delete(ITween tween)
        {
            tween.OnComplete -= Delete;
            _tweens.Remove(tween);
            tween.Dispose();
        }

        /// <summary>
        /// Remove and dispose all tweens locked by locker-object (tween owner)
        /// </summary>
        /// <param name="lockedBy">Tween locker (owner)</param>
        public void RemoveAll(object lockedBy)
        {
            EachTween(Delete, (tween, locker) => locker == lockedBy);
        }

        /// <summary>Remove and dispose all tweens</summary>
        public void RemoveAll()
        {
            EachTween(Delete);
        }

        protected void EachTween(Action<ITween> call)
        {
            var tweens = GetTweens();
            foreach (var tween in tweens)
                call(tween);
        }

        protected void EachTween(Action<ITween> call, Func<ITween, object, bool> predicate)
        {
            var tweens = GetTweens(predicate);
            foreach (var tween in tweens)
                call(tween);
        }

        /// <summary>Select all managed tweens</summary>
        /// <returns>List of tweens</returns>
        public IList<ITween> GetTweens()
        {
            var list = new List<ITween>();
            foreach (var pair in _tweens)
                list.Add(pair.Key);
            return list;
        }

        /// <summary>Select suitable tweens</summary>
        /// <param name="predicate">Check tween is suitable</param>
        /// <returns>List of tweens</returns>
        public IList<ITween> GetTweens(Func<ITween, bool> predicate)
        {
            var list = new List<ITween>();
            foreach (var pair in _tweens)
            {
                var tween = pair.Key;
                if (predicate(tween))
                    list.Add(tween);
            }
            return list;
        }

        /// <summary>Select suitable tweens</summary>
        /// <param name="predicate">Check tween and/or locker is suitable</param>
        /// <returns>List of tweens</returns>
        public IList<ITween> GetTweens(Func<ITween, object, bool> predicate)
        {
            var list = new List<ITween>();
            foreach (var pair in _tweens)
            {
                var tween = pair.Key;
                if (predicate(tween, pair.Value))
                    list.Add(tween);
            }
            return list;
        }

        /// <summary>Check tween timer is running</summary>
        public bool IsRunning { get { return _timer.IsRunning; } }

        /// <summary>Start tween timer from begining</summary>
        public void Start()
        {
            _timer.Restart();
        }

        /// <summary>Update all tweens with custom frame rate</summary>
        public void Update()
        {
            if (!IsRunning)
                return;
            var elapsed = _timer.ElapsedMilliseconds;
            var deltaTime = (float)(elapsed - _lastElapsedMsec) * TimeSpeedMultiplier;
            EachTween(tween => tween.Update(deltaTime));
            _lastElapsedMsec = elapsed;
        }

        /// <summary>Pause all tweens and save last tween positions</summary>
        public void Pause()
        {
            if (IsRunning) _timer.Stop();
        }

        /// <summary>Resume tweens after pause</summary>
        public void Resume()
        {
            _timer.Start();
        }

        /// <summary>
        /// Stop and reset timer (use to restart all tweens later from begining)
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            _timer.Reset();
        }

        /// <summary>Stop timer and remove all tweens</summary>
        public void Clear()
        {
            Stop();
            RemoveAll();
        }
    }
}
