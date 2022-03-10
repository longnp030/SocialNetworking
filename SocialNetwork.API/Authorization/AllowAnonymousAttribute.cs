namespace SocialNetwork.API.Authorization;

/// <summary>
/// Attribute to use in controllers which do not require jwt token authentication
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }
