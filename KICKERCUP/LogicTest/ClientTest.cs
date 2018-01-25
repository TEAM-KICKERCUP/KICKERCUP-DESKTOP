using Logic.ClientManagement.Et;
using Logic.ClientManagement.Impl;
using Logic.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LogicTest
{
    [TestClass]
    public class ClientTest
    {
        ClientIMPL cdl = new ClientIMPL();

        [TestMethod]
        public void TestClient()
        {
            Client vorgabe = new Client("maxxx", "maxxx", "max", "mustermann", "max.mustermann@web.de", "männlich");
            cdl.AddClient("maxxx", "maxxx", "max", "mustermann", "max.mustermann@web.de", "männlich");
            Client test = cdl.FindClient("maxxx");
            Assert.AreEqual(vorgabe, test);           
        }

        [TestMethod]
        public void TestDeleteClient()
        {
            cdl.AddClient("eva", "eva123", "eva", "mustermann", "eva.mustermann@web.de", "männlich");
            cdl.DeleteClient("eva");
            Assert.IsNull(cdl.FindClient("maxxx"));
        }


        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void TestUsernameTaken()
        {
            cdl.AddClient("Frodo", "one", "Frodo", "Beutlin", "theonering@web.de", "männlich");
            cdl.AddClient("Frodo", "two", "Bilbo", "Baggins", "bilbo.baggins@gmail.com", "männlich");
        }


        [TestCleanup]
        public void TestCleanup()
        {
            ClientDL dl = new ClientDL();

            if(dl.FindClient("maxxx") != null)
                dl.DeleteClient("maxxx");

            if (dl.FindClient("eva") != null)
                dl.DeleteClient("eva");

            if (dl.FindClient("Frodo") != null)
                dl.DeleteClient("Frodo");
        }
    }
}
