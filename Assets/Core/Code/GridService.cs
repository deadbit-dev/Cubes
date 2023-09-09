using System;
using UnityEngine;

namespace JoyTeam.Core
{
    public class GridService<T>
    {
        public readonly int Width;
        public readonly int Height;
        public readonly T[] Cells;
        
        public GridService (int width, int height)
        {
            Width = width; 
            Height = height;
            Cells = new T[width * height];
        }

        public void AddValue(Vector2Int coord, T value)
        {
            if (coord.x < 0 || coord.x >= Width) throw new IndexOutOfRangeException();
            if (coord.y < 0 || coord.y >= Height) throw new IndexOutOfRangeException();
            Cells[Width * coord.y + coord.x] = value;
        }
        
        public T GetValue(Vector2Int coord)
        {
            if (coord.x < 0 || coord.x >= Width) throw new IndexOutOfRangeException();
            if (coord.y < 0 || coord.y >= Height) throw new IndexOutOfRangeException();
            return Cells[Width * coord.y + coord.x];
        }

        public T GetNeighbour(Vector2Int coord, Vector2Int direction)
        {
            var neighbourCoord = coord + direction;

            if (neighbourCoord.x < 0 || neighbourCoord.x >= Width) throw new IndexOutOfRangeException();
            if (neighbourCoord.y < 0 || neighbourCoord.y >= Height) throw new IndexOutOfRangeException();

            return Cells[Width * neighbourCoord.y + neighbourCoord.x];
        }

        public Vector3 GridToWorld(int column, int row, Transform world, float xStep, float zStep)
        {
            return new Vector3(column * xStep, world.position.y, row * zStep);
        }

        public Vector2Int WorldToGrid(Vector3 worldPos, float xStep, float zStep)
        {
            return new Vector2Int(
               Mathf.FloorToInt(worldPos.x / xStep),
               Mathf.FloorToInt(worldPos.z / zStep)
            );
        }
    }
}