﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaceGame;
using NUnit.Framework;

namespace RaceGame_Tests {
    [TestFixture]
    class World_Test 
    {
        private World MakeWorld()
        {
            return new World(null, null, null, null, null);
        }
    }
}
