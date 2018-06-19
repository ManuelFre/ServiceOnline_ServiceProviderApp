using Microsoft.VisualStudio.TestTools.UnitTesting;
using PL_ServiceOnline.ViewModel;

namespace ViewModel.Test
{
    [TestClass]
    public class DetailVmTest
    {
        DetailVm detailVm = new DetailVm();

        [TestMethod]
        public void TestStatus_NotConfirmed()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual(statuses[2], detailVm.GetStatus("Y", null));
        }
        [TestMethod]
        public void TestStatus_NotConfirmed2()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual(statuses[2], detailVm.GetStatus("N", "N"));
        }

        [TestMethod]
        public void TestStatus_Declined()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual(statuses[3], detailVm.GetStatus("N", "x"));
        }

        [TestMethod]
        public void TestStatus_Accepted()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual(statuses[1], detailVm.GetStatus("N", "Y"));
        }

        [TestMethod]
        public void TestStatus_Finished()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual(statuses[0], detailVm.GetStatus("Y", "Y"));
        }

        [TestMethod]
        public void TestFinishedStatus_NotFinished()
        {
            Assert.AreEqual("N", detailVm.GetFinishedStatus(null));
        }

        [TestMethod]
        public void TestFinishedStatus_Finished()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual("Y", detailVm.GetFinishedStatus(statuses[0]));
        }

        [TestMethod]
        public void TestFinishedStatus_NotFinished2()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual("N", detailVm.GetFinishedStatus(statuses[1]));
        }

        [TestMethod]
        public void TestConfirmStatus_Declined()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual("x", detailVm.GetConfirmStatus(statuses[3]));
        }

        [TestMethod]
        public void TestConfirmStatus_Confirmed()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual("Y", detailVm.GetConfirmStatus(statuses[0]));
        }

        [TestMethod]
        public void TestConfirmStatus_Confirmed2()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual("Y", detailVm.GetConfirmStatus(statuses[1]));
        }

        [TestMethod]
        public void TestConfirmStatus_NotConfirmed()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual("N", detailVm.GetConfirmStatus(statuses[2]));
        }

        [TestMethod]
        public void TestConfirmStatus_NotConfirmed2()
        {
            var statuses = detailVm.AllStatuses;
            Assert.AreEqual("N", detailVm.GetConfirmStatus(null));
        }
    }
}

