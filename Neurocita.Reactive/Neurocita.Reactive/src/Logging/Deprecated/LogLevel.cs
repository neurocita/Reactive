﻿namespace Neurocita.Reactive.Logging
{
    /// <summary>
    ///   Supporting Logger levels.
    /// </summary>
    public enum LogLevel
	{
		/// <summary>
		///   Logging will be off
		/// </summary>
		Off = 0,
		/// <summary>
		///   Fatal logging level
		/// </summary>
		Fatal = 1,
		/// <summary>
		///   Error logging level
		/// </summary>
		Error = 2,
		/// <summary>
		///   Warn logging level
		/// </summary>
		Warn = 3,
		/// <summary>
		///   Info logging level
		/// </summary>
		Info = 4,
		/// <summary>
		///   Debug logging level
		/// </summary>
		Debug = 5,
		/// <summary>
		///   Trace logging level
		/// </summary>
		Trace = 6
	}
}
