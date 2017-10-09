using System;
using System.Collections.Generic;
using System.Text;

namespace BoggleSolver
{
    /// <summary>
    /// The GridSolver is responsible for finding all words in a grid of words.
    /// Each grid cell character will once per word.
    /// </summary>
    internal class GridSolver
    {
        
        private struct GridCell
        {
            public string C;
            public bool Searched;
        }

        // keep track of grid cells in use with a stack
        private readonly Stack<GridCell> _searchStack = new Stack<GridCell>();

        private readonly int _width;
        private readonly int _height;
        private readonly BoggleDictionary _dictionary;
        private readonly GridCell[,] _searchGrid;

        public GridSolver(BoggleDictionary dictionary, int width, int height)
        {
            _dictionary = dictionary;
            _width = width;
            _height = height;
            _searchGrid = new GridCell[_height, _width];
        }

        public string[] FindWords(string gridContents)
        {
            var results = new List<string>();
            if (_width * _height != gridContents.Length)
                throw new Exception("Contents length does not equal the number of cells in the grid.");            

            InitSearchGrid(gridContents.ToUpper());
            _searchStack.Clear();

            for (var x = 0; x < _width; ++x)
                for (var y = 0; y < _height; ++y)
                {
                    Search(results, x, y, 0);
                }
            results.Sort(CompareWordsByLength);
            return results.ToArray();
        }

        private static int CompareWordsByLength(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                // If x is null and y is not null, y
                // is greater. 
                return -1;
            }
            // If x is not null...
            //
            if (y == null)
                // ...and y is null, x is greater.
            {
                return 1;
            }
            // ...and y is not null, compare the 
            // lengths of the two strings.
            //
            int retval = y.Length.CompareTo(x.Length);

            return retval != 0 ? retval : string.Compare(x, y, StringComparison.Ordinal);
        }

        // returns the current string made by current selected grid cells.
        private string CurrentStackWord()
        {
            var sb = new StringBuilder();

            // get the array or characters on the stack, then reverse then array
            // the small max length size of words means this is not expensive,
            // but this is far from the ideal method of doing this. 
            GridCell[] arr = _searchStack.ToArray();
            Array.Reverse(arr);

            foreach (GridCell gc in arr)
            {
                sb.Append(gc.C);
            }
            return sb.ToString();
        }

        // The recurive search function that starts at a specific cell and then
        // recursively adds letters in every direction. At each successive 
        // call the search tree is checked to see if either the current
        // stack string is a word in the dictionary or if is a partial word 
        // in the dictionary. Recursion is halted when it is not, or max depth has
        // been reached. 
        private void Search(List<string> results, int x, int y, int depth)
        {
            if (depth >= _dictionary.MaxLenth ||
                x < 0 ||
                x >= _width ||
                y < 0 ||
                y >= _height)
            {
                return;
            }

            if (_searchGrid[x, y].Searched) return;

            //sb.Append(_searchGrid[x, y].C);
            _searchStack.Push(_searchGrid[x, y]);
            string key = CurrentStackWord();
            if (depth > 0 && !_dictionary.PartialKeyExists(key))
            {
                _searchStack.Pop();
                return;
            }
            _searchGrid[x, y].Searched = true;
            string value = _dictionary[key];
            if (value != null && value.Length >= BoggleDictionary.MinLength && !results.Contains(value))
                results.Add(value);

            Search(results, x - 1, y - 1, depth + 1);
            Search(results, x, y - 1, depth + 1);
            Search(results, x + 1, y - 1, depth + 1);
            Search(results, x - 1, y, depth + 1);
            Search(results, x + 1, y, depth + 1);
            Search(results, x - 1, y + 1, depth + 1);
            Search(results, x, y + 1, depth + 1);
            Search(results, x + 1, y + 1, depth + 1);

            _searchStack.Pop();
            _searchGrid[x, y].Searched = false;
        }

        private void InitSearchGrid(string contents)
        {
            var index = 0;            
            for (var i = 0; i < _height; ++i)
                for (var j = 0; j < _width; ++j)
                {
                    _searchGrid[j, i] = new GridCell
                    {
                        C = ToCellString(contents[index]),
                        Searched = false
                    };
                    ++index;
                }
        }

        private static string ToCellString(char c)
        {
            return c == 'Q' ? "QU" : Convert.ToString(c);
        }
    }


}
