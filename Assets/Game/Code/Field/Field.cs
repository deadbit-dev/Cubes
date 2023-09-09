using UnityEngine;
using JoyTeam.Core;

namespace JoyTeam.Game
{
    public class Field : MonoBehaviour
    {
        public float XStep;
        public float ZStep;
        public GridService<Unit?> Grid;
    }
}