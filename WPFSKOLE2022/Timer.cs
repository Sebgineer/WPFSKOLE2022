using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WPFSKOLE2022
{
    /// <summary>
    /// Timer
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// 
        /// </summary>
        private int seconds;

        /// <summary>
        /// 
        /// </summary>
        private int countdownSeconds;

        /// <summary>
        /// 
        /// </summary>
        private bool run = false;

        /// <summary>
        /// 
        /// </summary>
        private Thread countdownThread;


        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> CountdownChanges;

        /// <summary>
        /// 
        /// </summary>
        object _lock = new object();

        /// <summary>
        /// 
        /// </summary>
        public Timer()
        {
            this.seconds = 0;
            this.countdownSeconds = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="minutes"></param>
        public void SetTimer(int seconds, int minutes)
        {
            this.seconds = seconds + (minutes * 60);
            this.ResetTimer();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartTimer()
        {
            if (this.countdownThread == null || !this.countdownThread.IsAlive)
            {
                this.run = true;
                this.createThread();
                this.countdownThread.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopTimer()
        {
            if (this.countdownThread.IsAlive)
            {
                this.run = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetTimer()
        {
            this.countdownSeconds = this.seconds;
            this.OnCountdownChange(EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string TimeLeft()
        {
            string result;
            int seconds, minutes;
            lock (this._lock)
            {
                seconds = this.countdownSeconds;
                Monitor.Pulse(this._lock);
            }
            minutes = seconds / 60;
            seconds = seconds % 60;
            if (seconds < 10)
            {
                result = $"{minutes}:0{seconds}";
            }
            else
            {
                result = $"{minutes}:{seconds}";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Countdown()
        {
            while (this.run)
            {
                lock(this._lock)
                {
                    if (this.countdownSeconds > 0) 
                    { 
                        this.countdownSeconds--;
                    }
                    if (this.countdownSeconds <= 0)
                    {
                        this.PlaySound();
                        this.StopTimer();
                    }

                    Monitor.Pulse(this._lock);
                }
                this.OnCountdownChange(EventArgs.Empty);
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnCountdownChange(EventArgs e)
        {
            this.CountdownChanges.Invoke(this, e);
        }

        private void createThread()
        {
            this.countdownThread = new Thread(this.Countdown);
            this.countdownThread.Name = "TimerThread";
            this.countdownThread.IsBackground = true;
            this.countdownThread.Priority = ThreadPriority.Normal;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlaySound()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("./Audio/Beep.wav");
            player.Play();
        }
    }
}
