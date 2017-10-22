namespace Indigo.Server.Models
{
    /// <summary>
    /// Represents infomation about an error
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or Sets the request id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets if or not thhe string is set
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}