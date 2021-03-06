﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace RaceGame
{
    public class Control
    {
        public Control()
            : this(Keys.W, Keys.S, Keys.A, Keys.D)
        {

        }

        public Control(Keys accelerateKey, Keys decelerateKey, Keys leftKey, Keys rightKey)
        {
            Accelerate = accelerateKey;
            Decelearte = decelerateKey;
            Left = leftKey;
            Right = rightKey;
        }

        public Keys Accelerate { get; set; }
        public Keys Decelearte { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
    }
}
