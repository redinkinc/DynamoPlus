using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;

namespace IDF4DynamoNodes
{
    class idfUtilities
    {
        [MultiReturn(new[] { "add", "mult" })]
        
        public static Dictionary<string, object> ReturnMultiExample(double a, double b)
        {
            return new Dictionary<string, object>
            {
                { "add", (a + b) },
                { "mult", (a * b) }
            };
        }

    }
}
