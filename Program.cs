using System;
using System.Linq;
using System.Threading;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Xml.Linq;
using OpenQA.Selenium.Remote;
using System.IO.Enumeration;
using Ionic.Zip;

namespace PhotoGrepper
{
    class Program
    {
        public static void Main(string[] args)
        {

            searchMedia("https://www.popular.pics/reddit/subreddits/posts?r=EarthPorn", "test");
            
       
            
        }

        // Searches reddit/twitter etc...
        public static void searchMedia(string url, string folderName)
        {
            //OpenQA.Selenium.NoSuchWindowException
            string destination = "C:\\imgDown\\" + folderName + "\\";
            try
            {
                Thread.Sleep(1000); // Wait for chromedriver to load
                ChromeOptions options = new ChromeOptions();
                string adBlockDirectory = Directory.GetCurrentDirectory() + "\\adblock.crx"; // Prevent ad images from being downloaded
                options.AddExtensions(adBlockDirectory);
                var driver = new ChromeDriver(options = options);
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(1000); // Give URL time to load
                var Images = driver.FindElements(By.ClassName("post__image")); // Search for class
                Console.WriteLine("Number of Images to Be Downloaded: " + Images.Count());


                int iter = 0;

                // For every i in Images.Count, re assgign images, then click on Images with class type
                for (int i = 0; i < Images.Count(); i++)
                {

                    Images = driver.FindElements(By.ClassName("post__image")); // For every
                    Images[i].Click();
                    Thread.Sleep(1000); // Give each click time to load
                    var imageToSave = driver.FindElements(By.TagName("img")); // Now we look for the img tag and the source
                    Directory.CreateDirectory("C:\\imgDown\\" + folderName); // Create a new directory for the search\

                    // Iterate for every image count
                    for (int j = 0; j < imageToSave.Count(); j++)
                    {
                        
                        var imageURL = imageToSave[j].GetAttribute("src"); // Get the image URL source

                        Random rnd = new Random(); // Generates a random number to create filename
                        int num = rnd.Next();
                       
                        Console.WriteLine( j + " | Viewing: " + imageURL + " | saving as :" + folderName + num + ".jpeg"); // Testing only, outputs currently viewing 
                        WebClient downloadCurImage = new WebClient();
                      
                        // Generates a random number to create filename
                        
                        downloadCurImage.DownloadFile(imageURL, destination + "image" + num + ".jpeg");


                    }
                    Thread.Sleep(250);
                    iter++;
                    driver.Navigate().Back(); // Navigate backwards to click on the image next to previous one.

                }
                if(iter == Images.Count())
                {
                    driver.Quit();
                    compressDirectory("C:\\imgDown\\" + folderName, folderName);
                }

            }
            catch(OpenQA.Selenium.NoSuchWindowException e)
            {
                // Empty, quiet stop
            }
            


        }


        // When done, compress the directory where files where uploaded.
        // Not working as of yet
        public static void compressDirectory(string directoryPath, string fileName)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.UseUnicodeAsNecessary = true;
                zip.AddDirectory(directoryPath);
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                zip.Save(string.Format(fileName + ".zip"));
            }
        }
    }

}