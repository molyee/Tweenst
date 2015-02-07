namespace Alsoft.Tweest
{
    /// <summary>
    /// Tweens controller interface
    /// </summary>
    public interface ITweener
    {
        /// <summary>
        /// Animations playback rate
        /// </summary>
        float TimeSpeedMultiplier { get; set; }

        /// <summary>
        /// Add new tween
        /// </summary>
        /// <param name="tween">Tween</param>
        /// <param name="locker">Tween locker (owner)</param>
        void Add(ITween tween, object locker = null);

        /// <summary>
        /// Remove and dispose tween (with locker checking)
        /// </summary>
        /// <param name="tween">Tween to remove</param>
        /// <param name="locker">Tween locker (owner)</param>
        void Remove(ITween tween, object locker = null);

        /// <summary>
        /// Remove and dispose all tweens locked by locker-object (tween owner)
        /// </summary>
        /// <param name="lockedBy">Tween locker (owner)</param>
        void RemoveAll(object locker);

        /// <summary>
        /// Remove and dispose all tweens
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Start tween timer from begining
        /// </summary>
        void Start();

        /// <summary>
        /// Update all tweens with custom frame rate
        /// </summary>
        void Update();

        /// <summary>
        /// Pause all tweens and save last tween positions
        /// </summary>
        void Pause();

        /// <summary>
        /// Resume tweens after pause
        /// </summary>
        void Resume();

        /// <summary>
        /// Stop and reset timer (use to restart all tweens later from begining)
        /// </summary>
        void Stop();
    }
}
