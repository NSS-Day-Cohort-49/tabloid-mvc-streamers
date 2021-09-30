﻿using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAllUsers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, up.DisplayName, up.FirstName, up.LastName, up.UserTypeId, up.IsActive, ut.Name
                        FROM  UserProfile up
                            LEFT JOIN UserType ut ON up.UserTypeId = ut.Id
                        ORDER BY DisplayName ASC";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<UserProfile> users = new List<UserProfile>();

                    while (reader.Read())
                    {
                        UserProfile user = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            UserType = new UserType
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };

                        users.Add(user);
                    }

                    reader.Close();
                    return users;
                }
            }
        }

        public UserProfile GetUserById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                               SELECT up.[Id], up.FirstName, up.LastName, up.DisplayName, up.Email, 
                                      up.CreateDateTime, up.ImageLocation, up.UserTypeId, up.IsActive, ut.Name
                               FROM UserProfile up
                                    LEFT JOIN UserType ut ON up.UserTypeId = ut.Id
                               WHERE up.[Id] = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    UserProfile userProfile = null;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            UserType = new UserType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            },
                        };

                        reader.Close();
                        return userProfile;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }
        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.IsActive
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        public void IsActiveUser(UserProfile user)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE UserProfile
                            SET
                                IsActive = @isActive
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", user.Id);
                    cmd.Parameters.AddWithValue("isActive", !user.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
