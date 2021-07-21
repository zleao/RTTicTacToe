using System;
using System.Text;

namespace RTTicTacToe.WebApi.Extensions
{
    /// <summary>
    /// Extensions for Exception class
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Converts Exception to a readable string
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string ExceptionToString(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Date/Time: " + DateTime.UtcNow.ToString());
            sb.AppendLine("Exception Type: " + ex.GetType().FullName);
            sb.AppendLine("Message: " + ex.Message);
            sb.AppendLine("Source: " + ex.Source);
            foreach (var key in ex.Data.Keys)
            {
                sb.AppendLine(key.ToString() + ": " + ex.Data[key].ToString());
            }

            if (String.IsNullOrEmpty(ex.StackTrace))
            {
                sb.AppendLine("Environment Stack Trace: " + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("Stack Trace: " + ex.StackTrace);
            }
            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception: " + ex.InnerException.ExceptionToString());
            }

            return sb.ToString();
        }
    }
}
