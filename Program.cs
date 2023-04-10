using A10BlogsPosts.Services;
using A10BlogsPosts.Models;

namespace A10BlogsPosts
{
    public class Program
    {
        static void Main(string[] args)
        {
            int menuSelection = 0;
            //Max number of menu options. Allows programmer to change in one spot if menu item added or removed.
            int maxMenu = 5;

            while (menuSelection != maxMenu)
            {
                Console.WriteLine("Menu Options");
                Console.WriteLine("1. Display Blogs");
                Console.WriteLine("2. Add Blog");
                Console.WriteLine("3. Display Posts");
                Console.WriteLine("4. Add Post");
                Console.WriteLine("5. Exit");
                Console.WriteLine();

                bool validEntry = false;

                //Keep looping through until user chooses a valid entry, an integer and between 1 and the maxMenu option.
                while (!validEntry)
                {
                    menuSelection = InputService.GetIntWithPrompt("Select an option: ", "Entry must be an integer");
                    if (menuSelection < 1 || menuSelection > maxMenu)
                    {
                        Console.WriteLine($"Entry must be between 1 and {maxMenu}");
                    }
                    else
                    {
                        validEntry = true;
                    }
                }

                if (menuSelection == 1)
                {
                    DisplayBlogs();
                }
                else if (menuSelection == 2)
                {
                    AddBlog();
                }
                else if (menuSelection == 3)
                {
                    DisplayPosts();
                }
                else if (menuSelection == 4)
                {
                    AddPost();
                }

                if (menuSelection != maxMenu)
                {
                    Console.WriteLine();
                }

            }
        }
        
        //CRUD(Create, Read, Update, Delete)

        //Add (create) a Blog
        static void AddBlog()
        {
            using (var context = new BlogContext()) //Will auto close the database connection when it hits the close bracket }
            {
                Console.WriteLine("Enter a blog name");
                var blogName = Console.ReadLine();

                var blog = new Blog();
                blog.Name = blogName; //Don't need to add the Id, the database will do that since it's a primary key

                context.Blogs.Add(blog);
                context.SaveChanges(); //Must commit the changes (adds) to the database
            }
        }

        // Display (read) the Blogs
        static int DisplayBlogs()
        {
            var count = 0;

            using (var context = new BlogContext())
            {
                var blogsList = context.Blogs.ToList();

                Console.WriteLine();
                if (blogsList.Count > 0)
                {
                    Console.WriteLine("The blogs are:");
                    foreach (var blog in blogsList)
                    {
                        Console.WriteLine($"{blog.BlogId} {blog.Name}");
                        count++;
                    }
                }
                else
                {
                    Console.WriteLine("There are no blogs to display.");
                }
            }
            return count;
        }

        // Select a Blog
        static int SelectBlog()
        {
            int blogId = 0;
            bool validEntry = false;

            //Give the user a list to choose from
            int maxMenu = DisplayBlogs();
            Console.WriteLine();

            while (!validEntry)
            {
                blogId = InputService.GetIntWithPrompt("Select a blog: ", "Entry must be an integer");
                if (blogId < 1 || blogId > maxMenu)
                {
                    Console.WriteLine($"Entry must be between 1 and {maxMenu}");
                }
                else
                {
                    validEntry = true;
                }
            }
            return blogId;
        }

        // Update the Blogs: Just keeping this code as reference material only.
        /*
        static void UpdateBlog()
        {
            using (var context = new BlogContext())
            {
                var blogToUpdate = context.Blogs.Where(b => b.BlogId == 1).First(); //Would normally ask the user, show list of blogs and have users enter the id
                                                                                    //blogToUpdate is a "blog" type, so holds the entire record from the database?

                Console.WriteLine($"Your choice is {blogToUpdate.Name}");
                Console.WriteLine("What do you want the name to be?");
                var updatedName = Console.ReadLine();

                blogToUpdate.Name = updatedName;
                context.SaveChanges();
            }
        }
        // Delete a Blog
        static void DeleteBlog()
        {
            using (var context = new BlogContext()) //Will auto close the database connection when it hits the close bracket }
            {
                var blogToRemove = context.Blogs.Where(b => b.BlogId == 1).First(); //Would normally ask the user, show list of blogs and have users enter the id
                context.Remove(blogToRemove);
                context.SaveChanges();
            }
        }
        */

        // Display (read) the Posts
        static void DisplayPosts()
        {
            using (var context = new BlogContext())
            {
                //Select a blog to display the posts of
                int blogId = SelectBlog();

                var postsList = context.Posts.Where(b => b.BlogId == blogId).ToList(); //Forces the get of the data and put it in a list. Related to LazyLoading in the Context class.
                var blogItem = context.Blogs.Where(b => b.BlogId == blogId).First();

                Console.WriteLine();
                if (postsList.Count > 0)
                {
                    Console.WriteLine($"Below is a list of the posts for '{blogItem.Name}':");
                    
                    foreach (var post in postsList)
                    {
                        Console.WriteLine($"   Title: {post.Title}");
                        Console.WriteLine($"   Content: {post.Content}");
                    }
                }
                else
                {
                    Console.WriteLine("There are no posts to display.");
                }
            }
        }

        // Add (create) a Post
        static void AddPost()
        {
            using (var context = new BlogContext())
            {
                //Select a blog to add the post to
                int blogId = SelectBlog();

                var title = InputService.GetStringWithPrompt("Enter a post title: ", "Entry must be a string");
                var content = InputService.GetStringWithPrompt("Enter post content: ", "Entry must be a string");

                var post = new Post();
                post.Title = title;
                post.Content = content;
                post.BlogId = blogId;

                context.Posts.Add(post);
                context.SaveChanges();
            }
        }
    }
}