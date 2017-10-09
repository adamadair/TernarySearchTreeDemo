using System.IO;
using AWA.TernarySearchTree;

namespace BoggleSolver
{
    /// <summary>
    /// The boggle word dictionary, is a data structure built with a TernarySearchTree based
    /// dictionary, which will aid in quick searching for words and partial words. 
    /// The dictionary is loaded with words by text file, each word should be on a new line
    /// in the file. 
    /// </summary>
    internal class BoggleDictionary : TstDictionary<string, string>
    {
        public const int MinLength = 3;

        /// <summary>
        /// Length of longest word in the dictionary
        /// </summary>
        public int MaxLenth { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">file name of text dictionary. One word per line is expected.</param>
        public BoggleDictionary(string fileName)
        {
            MaxLenth = 0;
            LoadDictionary(fileName);            
        }

        private void LoadDictionary(string fileName)
        {           
            Clear();
            foreach (var line in File.ReadLines(fileName))
            {
                var word = line.ToUpper().Trim();
                var len = word.Length;
                if (len > MaxLenth)
                    MaxLenth = len;
                if (len >= MinLength)
                {
                    this[word] = word;
                }
                
            }
            BalanceSearchTree();
        }

        /*
           Boggle word scoring works as follows

           Word
           length	Points
           ------   ------
            3, 4	1
            5	    2
            6	    3
            7	    5
            8+	    11
        */
        public static int ScoreWord(string wordLen)
        {
            switch (wordLen.Length)
            {
                case 3:
                    return 1;
                case 4:
                    return 1;
                case 5:
                    return 2;
                case 6:
                    return 3;
                case 7:
                    return 5;
                default:
                    return 11;
            }            
        }
    }
}
