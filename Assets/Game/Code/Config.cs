using System.Collections.Generic;
using UnityEngine;

namespace JoyTeam.Game
{
    public static class Config
    {
        public static readonly Dictionary<int, string> ViewResourceByData = new ()
        {
            {-1, "Units/Wall"  },
            { 1, "Units/Cube1" },
            { 2, "Units/Cube2" },
            { 3, "Units/Cube3" },
            { 4, "Units/Cube4" },
            { 5, "Units/Cube5" }
        };
        
        public static readonly Level Level = new (
            8,
            7,
            new []
            {
                -1,-1,-1,-1,-1,-1,-1,-1,
                -1, 1, 1, 1, 1, 0,-1,-1,
                -1,-1, 0,-1,-1, 0,-1,-1,
                -1,-1, 0, 2, 2, 2, 2,-1,
                -1,-1, 0,-1,-1, 0,-1,-1,
                -1, 1, 1, 1, 1, 0,-1,-1,
                -1,-1,-1,-1,-1,-1,-1,-1
            }
        );

        public static float TimeForEntry = 1.0f;
        public static float MoveAndMergeDuration = 0.05f;
        public static float MinDistanceForSwipe = 20f;
    }
}