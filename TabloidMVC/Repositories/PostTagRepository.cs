using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository : BaseRepository, IPostTagRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }
        public List<PostTag> GetPostTagsbyPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
  
                    cmd.CommandText = @"Select pt.Id, TagId, PostId, t.Name AS TagName
                                        FROM PostTag pt
                                        JOIN Tag t ON t.Id = TagId 
                                        JOIN Post p ON p.Id = PostId 
                                        WHERE @Id = PostId";

                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

   
                    List<PostTag> postTags = new List<PostTag>();

                    while (reader.Read())
                    {
                        PostTag postTag = new PostTag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TagId = reader.GetInt32(reader.GetOrdinal("TagId")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            Tag = new Tag
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                                Name = reader.GetString(reader.GetOrdinal("TagName"))
                            }
                        };
                        postTags.Add(postTag);
                    }
                    reader.Close();
                    return postTags;
                }

            }
        }

        public PostTag GetPostTagbyPostIdAndTagId(int postId, int tagId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"Select pt.Id, TagId, PostId, t.Name AS TagName
                                        FROM PostTag pt
                                        JOIN Tag t ON t.Id = TagId 
                                        JOIN Post p ON p.Id = PostId 
                                        WHERE @PostId = PostId AND @TagId = TagId";

                    cmd.Parameters.AddWithValue("@PostId", postId);
                    cmd.Parameters.AddWithValue("@TagId", tagId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        PostTag postTag = new PostTag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TagId = reader.GetInt32(reader.GetOrdinal("TagId")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                        };

                        reader.Close();
                        return postTag;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }

            }
        }

        //Get all tags
        public List<PostTag> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
 
                    cmd.CommandText = @"Select Id, PostId, TagId
                                        FROM PostTag";

                    SqlDataReader reader = cmd.ExecuteReader();


                    List<PostTag> postTags = new List<PostTag>();

                    while (reader.Read())
                    {
                        PostTag postTag = new PostTag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            TagId = reader.GetInt32(reader.GetOrdinal("TagId"))
                        };
                        postTags.Add(postTag);
                    }
                    reader.Close();
                    return postTags;
                }

            }
        }



        public void AddTag(PostTag postTag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PostTag (TagId, PostId)
                        OUTPUT INSERTED.ID
                        VALUES (@tagId, @postId);";

                    cmd.Parameters.AddWithValue("@tagId", postTag.TagId);
                    cmd.Parameters.AddWithValue("@postId", postTag.PostId);
                    int id = (int)cmd.ExecuteScalar();

                    postTag.Id = id;
                }

            }
        }

        public void DeleteTag(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM PostTag WHERE Id = @Id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}