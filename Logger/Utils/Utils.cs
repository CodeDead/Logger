namespace CodeDead.Logger.Utils
{
    internal static class Utils
    {
        /// <summary>
        /// Check if an input might be a JSON
        /// </summary>
        /// <param name="input">The input that should be validated</param>
        /// <returns>True if the input might be JSON, otherwise false</returns>
        internal static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        /// <summary>
        /// Check if an input might be XML
        /// </summary>
        /// <param name="data">The input that should be validated</param>
        /// <returns>True if the input might be XML, otherwise false</returns>
        internal static bool IsXml(string data)
        {
            return !string.IsNullOrEmpty(data) && data.TrimStart().StartsWith("<");
        }
    }
}
