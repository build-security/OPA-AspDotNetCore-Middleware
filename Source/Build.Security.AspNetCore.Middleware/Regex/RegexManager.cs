using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Build.Security.AspNetCore.Middleware.RegexCache
{
    public static class RegexManager
    {
        private static List<Regex> _regexList = new List<Regex>();
        private static bool _initialized = false;

        public static bool InitializeOnce(string[] patterns)
        {
            if (!_initialized)
            {
                foreach (var s in patterns)
                {
                    _regexList.Add(new Regex(s));
                }

                _initialized = true;
                return true;
            }

            return false;
        }

        public static bool IsMatch(string s)
        {
            return _regexList.Any(regex => regex.IsMatch(s));
        }
    }
}
