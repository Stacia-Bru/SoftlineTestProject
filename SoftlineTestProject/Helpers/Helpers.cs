using System;
using System.Text.RegularExpressions;
using System.Globalization;


namespace SoftlineTestProject.Helpers
{
    /// <summary>
    /// Помощник-конвертор кодировок
    /// </summary>
    public class ConversionHelper
    {
        /// <summary>
        /// Перекодирует эскейп-последовательность Unicode в строку
        /// </summary>
        /// <param name="escapedUnicode">строка с эскейп-последовательность Unicode</param>
        /// <returns>строка</returns>
        public static string decodeEscapedUnicode(string escapedUnicode)
        {
            return (new Regex(@"\\[uU]([0-9A-F]{4})")).Replace(escapedUnicode, match => ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
        }
    }
}