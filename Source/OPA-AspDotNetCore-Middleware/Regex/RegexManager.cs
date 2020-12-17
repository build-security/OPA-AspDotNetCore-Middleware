using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpaAuthzMiddleware.RegexCache
{
    public static class RegexManager
    {
        private static List<Regex> _ignoreRegex = new List<Regex>();
        private static bool _initialized = false;

        public static bool InitializeOnce(string[] ignoreRegex)
        {
            if (!_initialized)
            {
                foreach (var s in ignoreRegex)
                {
                    _ignoreRegex.Add(new Regex(s));
                }

                _initialized = true;
                return true;
            }

            return false;
        }

        public static bool IsMatch(string s)
        {
            return _ignoreRegex.Any(regex => regex.IsMatch(s));
        }
    }
}
