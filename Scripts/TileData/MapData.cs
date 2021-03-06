﻿using System.Collections.Generic;
using UnityEngine;

namespace LTD.Map
{
    public class MapData : MonoBehaviour
    {
        public int width = 15;
        public int height = 15;

        public TextAsset textAsset;

        public List<string> GetTextFromFile(TextAsset tAsset)
        {
            List<string> lines = new List<string>();

            if (tAsset != null)
            {
                string textData = tAsset.text;
                string[] delimiters = { "\r\n", "\n" };
                lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
                lines.Reverse();
            }
            else
            {
                Debug.LogWarning("invalid Text Asset");
            }
            return lines;
        }

        public List<string> GetTextFromFile()
        {
            return GetTextFromFile(textAsset);
        }


        public void SetDimensions(List<string> textLines)
        {
            height = textLines.Count;
            foreach (string line in textLines)
            {
                if (line.Length > width)
                {
                    width = line.Length;
                }

            }
        }

        public int[,] MakeMap()
        {
            List<string> lines = new List<string>();
            lines = GetTextFromFile();
            SetDimensions(lines);

            int[,] map = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (lines[y].Length > x)
                    {
                        map[x, y] = (int)char.GetNumericValue(lines[y][x]);
                    }
                }
            }

            return map;
        }

    }
}
