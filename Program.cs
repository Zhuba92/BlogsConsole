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
                    var blog = new Blog { Name = name };
                    db.AddBlog(blog);
                    logger.Info("Blog added - {name}", name); 
                }
                else if(choice == 3)
                {
                    Post post = new Post();
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                    Console.WriteLine("Enter the name of the blog you would like to post to:");
                    string selection = Console.ReadLine();
                    var selectedBlog = db.Blogs.Where(b => b.Name.Contains(selection));
                    Console.Write("Please enter a number for the ID of the post: ");
                    try
                    {
                        post.PostId = Int32.Parse(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                    Console.Write("What is the title of the post: ");
                    post.Title = Console.ReadLine();
                    Console.Write("What is the content of the post: ");
                    post.Content = Console.ReadLine();
                    
                }
                else
                {

                }
                logger.Info("Program ended");
            }
            catch (Exception xy)
            {
                logger.Error(xy.Message);
            }
        }
    }
}
