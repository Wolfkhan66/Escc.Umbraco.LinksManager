using System.Collections.Generic;

namespace Escc.Umbraco.LinksManager.Models.InboundLinkChecker
{
    public class ErrorMessageModel
    {
        private static readonly IDictionary<string, string> ErrorMessage;

        static ErrorMessageModel()
        {
            ErrorMessage = new Dictionary<string, string>();

            ErrorMessage.Add("PageNotFound", "The page was not found.");
            ErrorMessage.Add("ErrorOccurred", "An error occurred while processing your request.");
        }

        /// <summary>
        /// Find error message
        /// </summary>
        /// <param name="msgKey">
        /// Key of required message as a string
        /// </param>
        /// <returns>
        /// Content of required message
        /// </returns>
        public static string ErrorMsg(string msgKey)
        {
            var rtnVal = string.Empty;

            if (ErrorMessage.ContainsKey(msgKey))
            {
                rtnVal = ErrorMessage[msgKey];
            }

            return rtnVal;
        }

        /// <summary>
        /// Find error message
        /// </summary>
        /// <param name="msgKey">
        /// Key of required message as an object
        /// </param>
        /// <returns>
        /// Content of required message
        /// </returns>
        public static string ErrorMsg(object msgKey)
        {
            if (msgKey == null) return string.Empty;

            return ErrorMsg(msgKey.ToString());
        }
    }
}