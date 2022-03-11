﻿namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who liked which post
/// <para>User - n : n : Entity -> Like</para>
/// </summary>
public class Like : Entity
{
    /// <summary>
    /// Entity's unique identifier
    /// <para>Reference to Entity.Id</para>
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// User's unique identifier
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid UserId { get; set; }
}