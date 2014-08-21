using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.IO;

namespace Xintric.DataRouter.Core.Connection.UnitTest
{
    /// <summary>
    /// Summary description for Stream_UnitTest
    /// </summary>
    [TestClass]
    public class Stream_UnitTest
    {
        public Stream_UnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var provider = new Core.Connection.Packet.Provider.Implementation();
            provider.Populate();


            var pair = P2P.GeneratePair(provider);


            var runner1 = Task.Run(() => pair.Item1.RunCollector());
            var runner2 = Task.Run(() => pair.Item2.RunCollector());


            using (var stream1 = new Stream(1, pair.Item1))
            using (var stream2 = new Stream(1, pair.Item2))
            {

                var rtask = Task<Tuple<string, int, byte[]>>.Run(() =>
                    {
                        using (var reader = new BinaryReader(stream2,Encoding.UTF8,true))
                        {
                            return new Tuple<string, int, byte[]>(
                                reader.ReadString(),
                                reader.ReadInt32(),
                                reader.ReadBytes(5));
                        }
                    });

                using (var writer = new BinaryWriter(stream1,Encoding.UTF8,true))
                {
                    writer.Write("hej som fan!");
                    writer.Write(56);
                    writer.Write(new byte[] { 51, 0, 0, 56, 48 });
                }

                Assert.IsTrue(rtask.Wait(TimeSpan.FromMilliseconds(100)));

                Assert.AreEqual("hej som fan!", rtask.Result.Item1);
                Assert.AreEqual(56, rtask.Result.Item2);
                Assert.IsTrue((new byte[] { 51, 0, 0, 56, 48 }).Zip(rtask.Result.Item3, (a, b) => a == b).All(x=>x));

                pair.Item1.Dispose();
                pair.Item2.Dispose();

                Assert.IsTrue(runner1.Wait(TimeSpan.FromMilliseconds(200)));
                Assert.IsTrue(runner2.Wait(TimeSpan.FromMilliseconds(200)));

            }


            //
            // TODO: Add test logic here
            //
        }
    }
}
