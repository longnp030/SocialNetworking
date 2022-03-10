using System.Globalization;

namespace SocialNetwork.API.Helpers;

/// <summary>
/// Custom exception class for throwing application specific exceptions (e.g. for validation) 
/// that can be caught and handled within the application
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class AppException : Exception
{
    public AppException() : base() { }

    public AppException(string message) : base(message) { }

    public AppException(string message, params object[] args)
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}