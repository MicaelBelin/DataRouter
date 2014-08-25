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
            provider = new Core.Connection.Packet.Provider.Implementation(Core.Connection.Packet.Provider.Implementation.AutoGenerateFlags.ScanEntireDomain);

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
        [TestInitialize()]
        public void MyTestInitialize()        
        {

            var pair = Connection.P2P.GeneratePair(provider);
            Connection1 = pair.Item1;
            Connection2 = pair.Item2;

            runnertask1 = Task.Run(() => Connection1.RunCollector());
            runnertask2 = Task.Run(() => Connection2.RunCollector());
        }

        IConnection Connection1;
        IConnection Connection2;
        Task runnertask1, runnertask2;


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
        #endregion


        [TestMethod]
        public void StopCollectorWhenDisposed()
        {
            Connection1.Dispose();
            Assert.IsTrue(runnertask1.Wait(TimeSpan.FromSeconds(60)));
        }

        [TestMethod]
        public void StopCollectorWhenClosed()
        {
            Connection2.Dispose();
            Assert.IsTrue(runnertask1.Wait(TimeSpan.FromSeconds(60)));
        }

        [TestMethod]
        public void StartupShutdown()
        {
        }

        [TestMethod]
        public void CommandPassthrough()
        {

            var gotcommand = new AutoResetEvent(false);
            
            Connection2.RegisterOnCommand(cmd =>
                {
                    gotcommand.Set();
                    return Connection.Command.FilterResult.Consume;
                });

            Connection1.SendAsync(new TestCommand("hej!")).FireAndForget();

            Assert.IsTrue(gotcommand.WaitOne(TimeSpan.FromSeconds(30)));

        }

        [TestMethod]
        public void RequestPassthrough()
        {

            Connection2.RegisterOnRequest(req =>
                {
                    return new TestResponse("tillbaka!");
                });

            var gotcommand = new AutoResetEvent(false);

            var reqtask = Connection1.SendAsync(new TestRequest("hej!"));

            Assert.IsTrue(reqtask.Wait(TimeSpan.FromSeconds(30)));

            var result = reqtask.Result as TestResponse;
            Assert.AreEqual("tillbaka!", result.Message);
            
        }


        [TestMethod]
        [ExpectedException(typeof(Core.Connection.Packet.ResponseException))]
        public void ThrowSystemException()
        {

            Connection2.RegisterOnRequest<TestRequest>(req =>
                {
                    throw new Exception();
                });

            var task = Connection1.SendAsync(new TestRequest("hej!"), TimeSpan.FromSeconds(30));

            try
            {
                task.Wait();
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }

        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void ThrowPacketException()
        {

            Connection2.RegisterOnRequest<TestRequest>(req =>
            {
                throw new TestException();
            });

            var task = Connection1.SendAsync(new TestRequest("hej!"), TimeSpan.FromSeconds(30));

            try
            {
                task.Wait();
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }

        }



    }
}
