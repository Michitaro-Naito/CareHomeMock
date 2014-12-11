using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using CareHomeMock.Helper;

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

        class Foo
        {
            public int a, b, c, d;
            public string e { get; set; }
            public string f { get; set; }
        }

        [TestMethod]
        public void CopyValues()
        {
            var foo = new Foo()
            {
                a = 1, b = 2, c = 3, d = 100, e = "aaa", f = "こんにちは"
            };
            var bar = new Foo()
            {
                a = 4, b = 5, c = 6, d = 101, e = "bbb", f = "こんばんは"
            };

            //Helper.CopyTo(ref foo, bar, "a,b,c,e,f");
            foo.CopyTo(ref bar, "a,b,c,e,f");

            Assert.AreEqual<int>(foo.a, bar.a);
            Assert.AreEqual<int>(foo.b, bar.b);
            Assert.AreEqual<int>(foo.c, bar.c);
            Assert.AreNotEqual<int>(foo.d, bar.d);
            Assert.AreEqual<string>(foo.e, bar.e);
            Assert.AreEqual<string>(foo.f, bar.f);
            Assert.AreEqual<int>(1, foo.a);
            Assert.AreEqual<int>(1, bar.a);

            try
            {
                foo.CopyTo(ref bar, "TheNameNotExists");
                Assert.Fail();
            }
            catch
            {

            }
        }

        public class Bar
        {
            [Tag("Super,Miracle")]
            public int a;
            
            public string b;

            [Tag("Ultimate,Super")]
            public double C { get; set; }

            public string D { get; set; }
        }

        [TestMethod]
        public void CopyToTagged()
        {
            var src = new Bar()
            {
                a = 1,
                b = "こんにちは！",
                C = 1.2,
                D = "Hello?"
            };
            var dest = new Bar()
            {
                a = 2,
                b = "ばんわ！",
                C = 3.14,
                D = "Hi!"
            };

            src.CopyToTagged(ref dest, "Super");

            Assert.AreEqual<int>(dest.a, src.a);
            Assert.AreNotEqual<string>(dest.b, src.b);
            Assert.AreEqual<double>(dest.C, src.C);
            Assert.AreNotEqual<string>(dest.D, src.D);
        }
    }
}
