using System;

namespace Combu
{
    /// <summary>
    /// Class to handle generic a generic search filter.
    /// </summary>
    [Serializable]
    public class SearchCustomData
    {
        public string key;
        public eSearchOperator op;
        public string value;

        public SearchCustomData(string key, eSearchOperator op, string value)
        {
            this.key = key;
            this.op = op;
            this.value = value;
        }
    }
}
