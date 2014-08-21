using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;

namespace Xintric.DataRouter.Core.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class AbstractConnection_Unittest
    {
        public AbstractConnection_Unittest()
        {
            //
            // TODO: Add constructor logic here
            //
            provider = new Core.Connection.Packet.Provider.Implementation();

            provider.RegisterFactory(TestCommand.FactoryInstance);
            provider.RegisterFactory(TestRequest.FactoryInstance);
            provider.RegisterFactory(TestResponse.FactoryInstance);
        }

        Core.Connection.Packet.IProvider provider;
        
        

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
        public void StartAndStopRunner()
        {
            var pair = Core.Connection.P2P.GeneratePair(provider);

            var collectortask = Task.Run(() =>
                {
                    pair.Item1.RunCollector();
                });


            pair.Item1.Dispose();

            Assert.IsTrue(collectortask.Wait(TimeSpan.FromMilliseconds(200)));

        }

        [TestMethod]
        public void CommandPassthrough()
        {
            var pair = Core.Connection.P2P.GeneratePair(provider);

            var item1runner = Task.Run(() =>
                {
                    pair.Item1.RunCollector();
                });
            var item2runner = Task.Run(() =>
                {
                    pair.Item2.RunCollector();
                });

            var gotcommand = new AutoResetEvent(false);
            
            pair.Item2.RegisterOnCommand(cmd =>
                {
                    Thread.Sleep(500);
                    gotcommand.Set();
                    return CommandFilterResult.Consume;
                });

            pair.Item1.SendAsync(new TestCommand("hej!")).FireAndForget();

            Assert.IsTrue(gotcommand.WaitOne(TimeSpan.FromMilliseconds(200)));

            pair.Item1.Dispose();
            pair.Item2.Dispose();


            Assert.IsTrue(item1runner.Wait(TimeSpan.FromMilliseconds(200)));
            Assert.IsTrue(item2runner.Wait(TimeSpan.FromMilliseconds(200)));
        }

        [TestMethod]
        public void RequestPassthrough()
        {
            var pair = Core.Connection.P2P.GeneratePair(provider);

            var item1runner = Task.Run(() =>
                {
                    pair.Item1.RunCollector();
                });
            var item2runner = Task.Run(() =>
                {
                    pair.Item2.RunCollector();
                });

            pair.Item2.RegisterOnRequest(req =>
                {
                    return new TestResponse("tillbaka!");
                });

            var gotcommand = new AutoResetEvent(false);

            var reqtask = pair.Item1.SendAsync(new TestRequest("hej!"));

            Assert.IsTrue(reqtask.Wait(TimeSpan.FromMilliseconds(200)));

            var result = reqtask.Result as TestResponse;
            Assert.AreEqual("tillbaka!", result.Message);


            pair.Item1.Dispose();
            pair.Item2.Dispose();


            Assert.IsTrue(item1runner.Wait(TimeSpan.FromMilliseconds(200)));
            Assert.IsTrue(item2runner.Wait(TimeSpan.FromMilliseconds(200)));
            
        }
    }
}
