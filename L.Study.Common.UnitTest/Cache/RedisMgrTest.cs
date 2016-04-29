using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using L.Study.Common.Cache;

namespace L.Study.Common.UnitTest.Cache
{
    [TestClass]
    public class RedisMgrTest
    {
        //RedisMgr mgr = new RedisMgr("192.168.0.103:6379");
        ICacheMgr mgr = new RedisMgr("192.168.0.103:6379");
        string key = "key1";
        string value1 = "lihanhan";
        string value2 = "lihanhan_Set_result";
        [TestMethod]
        public void RedisAddTest()
        {
            var result = mgr.Add<string>(key, value1);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void RedisSetTest()
        {
            var result = mgr.Set<string>(key, value2);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void RedisExistTest()
        {
            Assert.IsTrue(mgr.Exist(key));
        }
        [TestMethod]
        public void RedisGetTest()
        {
            var result = mgr.Get<string>(key);
            Assert.IsTrue(result == value1 || result == value2);
        }
        [TestMethod]
        public void RedisRemoveTest()
        {
            Assert.IsTrue(mgr.Remove(key));
        }
        [TestMethod]
        public void RedisRemoveAllTest()
        {
            Assert.IsTrue(mgr.RemoveAll());
        }
    }
}
