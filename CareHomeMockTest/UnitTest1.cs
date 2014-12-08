using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace CareHomeMockTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void YoutubeUrlRegex()
        {
            // <iframe width="560" height="315" src="//www.youtube.com/embed/7sTqTviF_50?list=PL449289AAD921E04E" frameborder="0" allowfullscreen></iframe>

            var regex = new Regex(@"^https?:\/\/(www\.youtube\.com\/watch\?v=(?<v>[a-zA-Z0-9-_]+))|(youtu.be/(?<v>[a-zA-Z0-9-_]+))");

            Assert.IsTrue(regex.IsMatch("https://www.youtube.com/watch?v=7sTqTviF_50"));
            Assert.IsTrue(regex.IsMatch("https://www.youtube.com/watch?v=h-ujfjbVurM&index=1&list=PL449289AAD921E04E"));
            Assert.IsTrue(regex.IsMatch("http://youtu.be/h-ujfjbVurM"));
            Assert.IsTrue(regex.IsMatch("http://youtu.be/h-ujfjbVurM?list=PL449289AAD921E04E"));

            Assert.IsFalse(regex.IsMatch("http://badwebsite.example.com/"));
            Assert.IsFalse(regex.IsMatch("https://www.youtube.com/watch?v="));  // Broken
            Assert.IsFalse(regex.IsMatch("https://www.youtube.com/watch"));  // Broken
            Assert.IsFalse(regex.IsMatch("https://www.youtube.com/"));  // Broken
            Assert.IsFalse(regex.IsMatch("http://youtu.be/"));   // Broken

            var match = regex.Match("https://www.youtube.com/watch?v=h-ujfjbVurM&index=1&list=PL449289AAD921E04E");
            Assert.AreEqual<string>("h-ujfjbVurM", match.Groups["v"].ToString());
        }
    }
}
