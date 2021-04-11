using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.Name);
            Console.WriteLine("\nWhich would you like to do?\n1. Display all blogs\n2. Add blog\n3. Create post\n4. Display posts\n");
            
            try
            {
                int choice = Int32.Parse(Console.ReadLine());
                if(choice == 1)
                {
                    // Display all Blogs from the database
                    Console.WriteLine("All blogs in the database:");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                }
                else if(choice == 2)
                {
                    // Create and save a new Blog
                    Console.Write("Enter a name for a new Blog: ");
                    var name = Console.ReadLine();
                    var blog = new Blog {Name = name};
                    db.AddBlog(blog);
                    logger.Info("Blog added - {name}", name); 
                }
                else if(choice == 3)
                {
                    Post post = new Post();
                    Console.WriteLine("Available blogs:");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                    Console.WriteLine("\nEnter the name of the blog you would like to post to:");
                    try
                    {
                        string selection = Console.ReadLine();
                        Blog selectedBlog = db.Blogs.First(b => b.Name.Contains(selection));
                        Console.Write("What is the title of the post: ");
                        post.Title = Console.ReadLine();
                        Console.Write("What is the content of the post: ");
                        post.Content = Console.ReadLine();
                        post.BlogId = selectedBlog.BlogId;
                        post.Blog = selectedBlog;
                        db.AddPost(post);
                    }
                    catch(Exception xx)
                    {
                        logger.Error(xx.Message);
                        Console.WriteLine("Invalid choice");
                    }
                }
                else if (choice == 4)
                {
                    var blogs = query.ToArray();
                    int count = 1;
                    int counter = 0;
                    foreach (var item in blogs)
                    {
                        Console.WriteLine(count + ". " + item.Name);
                        count++;
                    }
                    Console.Write("\nEnter the number of the blog you would like to view the posts from: ");
                    try
                    {
                        int selection = Convert.ToInt32(Console.ReadLine());
                        var selectedPosts = db.Posts.Where(b => b.BlogId.Equals(blogs[selection - 1].BlogId));
                        foreach(Post post in selectedPosts)
                        {
                            Console.WriteLine($"Blog name: {post.Blog.Name}, Title: {post.Title}, Content: {post.Content}");
                            counter++;
                        }
                        Console.WriteLine("\nNumber of posts in this blog: " + counter +  "\n");
                    }
                    catch (Exception xz)
                    {
                        logger.Error(xz.Message);
                    }
                }
                logger.Info("\nProgram ended");
            }
            catch (Exception xy)
            {
                logger.Error(xy.Message);
            }
        }
    }
}
