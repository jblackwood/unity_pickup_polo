using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Utils
{

    public static class Utils
    {

        public static string PrettyPrint(this IEnumerable<KeyValuePair<string, object>> d)
        {
            var lines = d.Select(kvp => $"{kvp.Key}: {kvp.Value}");
            return string.Join(Environment.NewLine, lines);
        }
    }
}