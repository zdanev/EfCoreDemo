using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo
{
    public static class BlogDemo
    {
        public static void Execute()
        {
            using (var db = new BlogDb("john"))
            {
                var blogs = db.Blogs
                    .Include(b => b.Posts)
                    // .IgnoreQueryFilters()
                    .ToList();

                foreach (var blog in blogs)
                {
                    // Console.WriteLine($"{blog.Url.PadRight(40)} [Tenant: {blog.TenantId}]");
                    Console.WriteLine($"{blog.Url.PadRight(40)}");

                    foreach (var post in blog.Posts)
                    {
                        // Console.WriteLine($" - {post.Title.PadRight(37)} [IsDeleted: {post.IsDeleted}]");
                        Console.WriteLine($" - {post.Title.PadRight(37)}");
                    }

                    Console.WriteLine();
                }
            }
        }

        public static void Setup()
        {
            // using (var db = new BlogDb("any"))
            // {
            //     db.Database.EnsureDeleted();
            // }

            using (var db = new BlogDb("john"))
            {
                if (db.Database.EnsureCreated())
                {
                    db.Blogs.Add(new Blog
                    {
                        Url = "http://sample.com/blogs/fish",
                        Posts = new List<Post>
                        {
                            new Post { Title = "Fish care 101" },
                            new Post { Title = "Caring for tropical fish" },
                            new Post { Title = "Types of ornamental fish" }
                        }
                    });

                    db.Blogs.Add(new Blog
                    {
                        Url = "http://sample.com/blogs/cats",
                        Posts = new List<Post>
                        {
                            new Post { Title = "Cat care 101" },
                            new Post { Title = "Caring for tropical cats" },
                            new Post { Title = "Types of ornamental cats" }
                        }
                    });

                    db.SaveChanges();

                    using (var jeff_db = new BlogDb("jeff"))
                    {
                        jeff_db.Blogs.Add(new Blog
                        {
                            Url = "http://sample.com/blogs/catfish",
                            Posts = new List<Post>
                        {
                            new Post { Title = "Catfish care 101" },
                            new Post { Title = "History of the catfish name" },
                        }
                        });

                        jeff_db.SaveChanges();
                    }

                    db.Posts
                        .Where(p => p.Title == "Caring for tropical fish" 
                                    || p.Title == "Cat care 101")
                        .ToList()
                        .ForEach(p => db.Posts.Remove(p));

                    db.SaveChanges();
                }
            }
        }
    }
}