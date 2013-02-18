﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace RaceGame
{
    class GameTime : IGameTime
    {
        Stopwatch _timer;

        public GameTime()
        {
            _timer = new Stopwatch();
        }

        public TimeSpan GetElapsedTime()
        {
            return _timer.Elapsed;
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void Resume()
        {
            _timer.Start();
        }

        public void Reset()
        {
            _timer.Reset();
        }
    }
}
