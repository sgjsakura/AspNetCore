using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc
{
    /// <summary>
    ///     Define the necessary feature for generating HTML content for <see cref="OperationMessage" /> list.
    /// </summary>
    public interface IOperationMessageHtmlGenerator
    {
        /// <summary>
        ///     Generate HTML content for one or more <see cref="OperationMessage" /> objects.
        /// </summary>
        /// <param name="messages">The list of message to generating the HTML content.</param>
        /// <param name="listStyle">The style of the message.</param>
        /// <param name="useTwoLineMode">Whether two line mode should be used.</param>
        /// <returns></returns>
        IHtmlContent GenerateList(IEnumerable<OperationMessage> messages, MessageListStyle listStyle,
            bool useTwoLineMode);
    }
}