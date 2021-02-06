using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

/// <summary>
/// Wrapper for Logging Application Block
/// </summary>
public static class ApplicationLogger
{
    /// <summary>
    /// Log message
    /// </summary>
    public static void Write(string message)
    {
        Logger.Write(message);
    }

    /// <summary>
    /// Log message with several information
    /// </summary>
    public static void Write(string message, string category, TraceEventType severity, string title)
    {
        Logger.Write(message, category, 1, 1, severity, title);
    }

    /// <summary>
    /// Log exception with several information
    /// </summary>
    /// <param name="message">message to be logged</param>
    /// <param name="category">category</param>
    /// <param name="severity">severity</param>
    public static void Write(string message, string category, TraceEventType severity)
    {
        Logger.Write(message, category, 1, 1, severity, String.Empty);
    }

    /// <summary>
    /// Log exception detail
    /// </summary>
    /// <param name="e">Exception object to be logged</param>
    public static void Write(Exception e)
    {
        Write(e.Message, "General", TraceEventType.Error, e.GetType().Name);
        Logger.Write(String.Format("{0}\n\n{1}\n\n{2}", e.Source, e.Message, e.StackTrace), "Exception", 1, 1, TraceEventType.Critical, e.GetType().Name);
    }
}