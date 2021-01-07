using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public List<Comment> getAllByPost(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Command not tested because no created comments
                    cmd.CommandText = @"
                        SELECT c.Id, c.Subject, c.Content, c.CreateDateTime,
	                           c.PostId, c.UserProfileId,
	                           p.Title as PostTitle, p.Content as PostContent,
	                           u.DisplayName
                        FROM Comment c
	                         LEFT JOIN Post p ON c.PostId = p.Id
	                         LEFT JOIN UserProfile u ON c.UserProfileId = u.Id
                        WHERE c.PostId = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    var comments = new List<Comment>();

                    while (reader.Read())
                    {
                        comments.Add(NewCommentFromReader(reader));
                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        public Comment GetCommentById(int commentId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.Id, c.Subject, c.Content, c.CreateDateTime,
	                           c.PostId, c.UserProfileId,
	                           p.Title as PostTitle, p.Content as PostContent,
	                           u.DisplayName
                        FROM Comment c
	                         LEFT JOIN Post p ON c.PostId = p.Id
	                         LEFT JOIN UserProfile u ON c.UserProfileId = u.Id
                       WHERE c.Id = @commentId";

                    cmd.Parameters.AddWithValue("@commentId", commentId);

                    var reader = cmd.ExecuteReader();

                    Comment comment = null;

                    if (reader.Read())
                    {
                        comment = NewCommentFromReader(reader);
                    }

                    reader.Close();

                    return comment;
                }
            }
        }

        public void Add(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (
                            PostId, UserProfileId, Subject, Content, CreateDateTime )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @PostId, @UserProfileId, @Subject, @Content, @CreateDateTime )";

                    cmd.Parameters.AddWithValue("@PostId", comment.PostId );
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId );
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        
        public void UpdateComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Comment 
                            SET 
                                PostId = @PostId, 
                                UserProfileId = @UserProfileId, 
                                Subject = @Subject, 
                                Content = @Content, 
                                CreateDateTime = @CreateDateTime
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);
                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteComment(int commentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Comment
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", commentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private Comment NewCommentFromReader(SqlDataReader reader)
        {
            return new Comment()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                Content = reader.GetString(reader.GetOrdinal("Content")),
                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                Post = new Post()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("PostId")),
                    Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                    Content = reader.GetString(reader.GetOrdinal("PostContent"))
                },
                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                UserProfile = new UserProfile()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                    DisplayName = reader.GetString(reader.GetOrdinal("DisplayName"))
                }
            };
        }

    }
}
