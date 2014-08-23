using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

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
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            var pair = P2P.GeneratePair(provider);
            Connection1 = pair.Item1;
            Connection2 = pair.Item2;

            runnertask1 = Task.Run(() => Connection1.RunCollector());
            runnertask2 = Task.Run(() => Connection2.RunCollector());
        }

        IConnection Connection1;
        IConnection Connection2;
        Task runnertask1, runnertask2;

        Core.Connection.Packet.Provider.Implementation provider = new Core.Connection.Packet.Provider.Implementation();
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            Connection1.Dispose();
            Connection2.Dispose();
            runnertask1.Wait();
            runnertask2.Wait();
            //Assert.IsTrue(runnertask1.Wait(TimeSpan.FromMilliseconds(5000)));
            //Assert.IsTrue(runnertask2.Wait(TimeSpan.FromMilliseconds(5000)));
        }
        //
        #endregion

        [TestMethod]
        public void SendAndRecieve()
        {


            using (var stream1 = new Stream(1, Connection1))
            using (var stream2 = new Stream(1, Connection2))
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

                Assert.IsTrue(rtask.Wait(TimeSpan.FromSeconds(30)));

                Assert.AreEqual("hej som fan!", rtask.Result.Item1);
                Assert.AreEqual(56, rtask.Result.Item2);
                Assert.IsTrue((new byte[] { 51, 0, 0, 56, 48 }).Zip(rtask.Result.Item3, (a, b) => a == b).All(x=>x));


            }
        }

        [TestMethod]
        public void ReadUntilClosed()
        {


            using (var stream1 = new Stream(1, Connection1))
            using (var stream2 = new Stream(1, Connection2))
            {

                var rtask = Task<int>.Run(() =>
                {
                    var buf = new byte[1024];

                    var ret = new byte[] { };
                    while(true)
                    {
                        var bytesread = stream2.Read(buf, 0, 1024);
                        if (bytesread == 0) break;
                        ret = ret.Concat(buf.Take(bytesread)).ToArray();
                    };
                    return ret;
                });

                stream1.Write(new byte[] { 1, 2, 3, 4 }, 0, 4);

                Assert.IsFalse(rtask.IsCompleted);

                stream1.Write(new byte[] { 5, 6, 7 }, 0, 3);

                stream1.Dispose();

                Assert.IsTrue(rtask.Wait(TimeSpan.FromSeconds(30)), "Reader haven't received a stop in time");

                Assert.AreEqual(7, rtask.Result.Count());



            }
            
        }
    }
}
