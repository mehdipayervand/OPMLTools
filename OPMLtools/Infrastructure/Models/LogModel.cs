// -----------------------------------------------------------------------
// <copyright file="LogModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OPMLtools.Infrastructure.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LogModel
    {
        #region Enums (1)

        public enum LogType
        {
            Error,
            Info,
            Warn
        }

        #endregion Enums

        #region Properties (3)

        public string Message { get; set; }

        public string Time { get; set; }

        public LogType Type { get; set; }

        #endregion Properties
    }
}
