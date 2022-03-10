﻿using AutoMapper;
using SocialNetwork.API.Entities;
using SocialNetwork.API.Models.User;

namespace SocialNetwork.API.Helpers;

/// <summary>
/// Handle models to be update into DB
/// <para>Author: longnp</para>
/// <para>Created: 10/03/2022</para>
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User -> AuthenticateResponse
        CreateMap<User, AuthenticateResponse>();

        // RegisterRequest -> User
        CreateMap<RegisterRequest, User>();

        // UpdateRequest -> User
        CreateMap<UpdateCredentialsRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));
    }
}
