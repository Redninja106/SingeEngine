using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Singe
{
    public static class Time
    {
        private static Stopwatch stopwatch;

        public static float DeltaTimeF { get; private set; }
        public static float TotalTimeF { get; private set; }
        public static decimal DeltaTime { get; private set; }
        public static decimal TotalTime { get; private set; }

        private static decimal lastTime;

        public static decimal GetLiveTime()
        {
            return stopwatch.ElapsedTicks / (decimal) Stopwatch.Frequency;
        }

        static Time()
        {
            stopwatch = new Stopwatch();
        }

        internal static void Update()
        {
            // if the stopwatch is not running, start it
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
                TotalTime = 0m; // time of 0 on first frame
            }
            else
            {
                // read current time
                TotalTime = GetLiveTime();
            }

            // compute delta time
            DeltaTime = TotalTime - lastTime;
            lastTime = TotalTime;

            // set float versions of time measurements
            DeltaTimeF = (float)DeltaTime;
            TotalTimeF = (float)TotalTime;
        }
    }
}
