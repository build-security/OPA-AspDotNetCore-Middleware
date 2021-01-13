using System.Collections.Generic;
using System.Linq;

namespace Build.Security.AspNetCore.Middleware.Regex
{
    public static class RegexManager
    {
        private static List<System.Text.RegularExpressions.Regex> _regexList = new List<System.Text.RegularExpressions.Regex>();
        private static bool _initialized = false;

        public static bool InitializeOnce(string[] patterns)
        {
            if (!_initialized)
            {
                foreach (var s in patterns)
                {
                    _regexList.Add(new System.Text.RegularExpressions.Regex(s));
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
