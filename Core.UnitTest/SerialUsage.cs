using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;

namespace Xintric.DataRouter.Core
{
    [TestClass]
    public class SerialUsage
    {
        void cycle()
        {
            Core.Connection.Packet.Provider.Implementation provider = new Core.Connection.Packet.Provider.Implementation();
            provider.Populate();

            var pair = Connection.P2P.GeneratePair(provider);
            var Connection1 = pair.Item1;
            var Connection2 = pair.Item2;

            var runnertask1 = Task.Run(() => Connection1.RunCollector());
            var runnertask2 = Task.Run(() => Connection2.RunCollector());


            Connection1.Dispose();
            Connection2.Dispose();
            //            runnertask1.Wait();
            //            runnertask2.Wait();

            //Assert.IsTrue(runnertask1.Wait(TimeSpan.FromMilliseconds(500)));
            //Assert.IsTrue(runnertask2.Wait(TimeSpan.FromMilliseconds(500)));
            runnertask1.Wait();
            runnertask2.Wait();

        }


        [TestMethod]
        public void TestMethod1()
        {
            cycle();
            cycle();
            cycle();
            cycle();

            Task.WaitAll(Enumerable.Range(0, 10).Select(x => Task.Run(() => cycle())).ToArray());
        }
    }
}
