using KGNetwork.Data;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Icons;
using System.Diagnostics.Metrics;

namespace KGNetwork.Utils
{
    public static class DocType
    {
        public static List<string> Items()
        {
            return new List<string>()
            {
                "Article",
                "Review",
"Biographical-Item",
"Book",
"Book Chapter",
"Book Review",
"Correction",
"Data Paper",
"Database Review",
"Editorial Material",
"Letter",
"Meeting Abstract",
"Music Performance Review",
"News Item",
"Proceedings Paper",
"Reprint",
"Retracted Publication",
"Retraction",

            };
        }
    }
}
