using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo
{
    public static class FuncDemo
    {
        static IEnumerable<Blog> FindBlogSql(string term)
        {
            var likeExpr = $"%{term}%";
            var sql = $"SELECT * FROM Blogs WHERE Url LIKE '{likeExpr}'";

            using (var db = new BlogDb("john"))
            {
                return db.Blogs.FromSql(sql).ToArray();
            }
        }

        static IEnumerable<Blog> FindBlogFunc(string term)
        {
            var likeExpr = $"%{term}%";

            using (var db = new BlogDb("john"))
            {
                return db.Blogs.Where(b => EF.Functions.Like(b.Url, likeExpr)).ToArray();
            }
        }

        static IEnumerable<Blog> FindBlogCompiled(string term)
        {
            var likeExpr = $"%{term}%";
            var query = EF.CompileQuery((BlogDb db, string expr) => db.Blogs.Where(b => EF.Functions.Like(b.Url, expr)));

            using (var db = new BlogDb("john"))
            {
                return query(db, likeExpr).ToArray();
            }
        }

        static void ShowBlogs(IEnumerable<Blog> blogs)
        {
            foreach (var blog in blogs)
            {
                Console.WriteLine(blog.Url);
            }
        }

        public static void Execute()
        {
            Console.WriteLine("Query with SQL...");
            ShowBlogs(FindBlogSql("cat"));
            Console.WriteLine();

            Console.WriteLine("Query with LIKE function...");
            ShowBlogs(FindBlogFunc("cat"));
            Console.WriteLine();

            Console.WriteLine("Compiled Query with LIKE function...");
            ShowBlogs(FindBlogCompiled("cat"));
            Console.WriteLine();
        }
    }
}