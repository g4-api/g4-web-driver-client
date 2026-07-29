using System.Collections.Generic;
using System.Net.Http;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// A model for sending commands to a WebDriver service.
    /// </summary>
    public class WebDriverCommandModel
    {
        #region *** Properties   ***

        /// <summary>
        /// Gets or sets the request content type. Default is "application/json";
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets data (payload) of the command
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the element id to send with the command.
        /// </summary>
        public string Element { get; set; }

        /// <summary>
        /// Gets or sets additional headers to send with the command.
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets the method of the RavenCommand
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the route of the RavenCommand
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// Gets or sets the session id to use with the command.
        /// </summary>
        public string Session { get; set; }

        #endregion

        #region *** Methods      ***

        /// <summary>
        /// Copies this command into invocation-owned state so request-specific values cannot mutate a registered template.
        /// </summary>
        /// <returns>A new command containing the current definition and request values.</returns>
        internal WebDriverCommandModel Copy()
        {
            // Duplicate caller-provided headers so one invocation cannot change another invocation's collection.
            var headers = Headers == null
                ? null
                : new Dictionary<string, string>(Headers);

            // Preserve every command value on a distinct model so later route and payload mutations remain isolated.
            return new WebDriverCommandModel
            {
                ContentType = ContentType,
                Data = Data,
                Element = Element,
                Headers = headers,
                Method = Method,
                Route = Route,
                Session = Session
            };
        }

        #endregion
    }
}
