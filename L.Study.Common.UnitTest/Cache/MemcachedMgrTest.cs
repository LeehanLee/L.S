using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using L.Study.Common.Cache;

namespace L.Study.Common.UnitTest.Cache
{
    /// <summary>
    /// 这个名字的单元测试类，表示对MemcachedMgr类进行测试的
    /// </summary>
    [TestClass]
    public class MemcachedMgrTest
    {
        //ICache mgr = new MemcachedMgr();
        ICacheMgr mgr = new MemcachedMgr();
        //ICacheCreator c1 = new MemcachedMgrCreator();
        //ICache mgr = MemcachedMgrCreator.test();
        string key = "key1";
        string value1 = "lihanhan";
        string value2 = "lihanhan_Set_result";
        /// <summary>
        /// 这个名字的方法表示对Add方法进行测试
        /// </summary>
        [TestMethod]
        public void MemcachedAddTest()
        {
            var result = mgr.Add(key, value1);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void MemcachedSetTest()
        {
            var result = mgr.Set(key, value2);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void MemcachedExistTest()
        {
            Assert.IsTrue(mgr.Exist(key));
        }
        [TestMethod]
        public void MemcachedGetTest()
        {
            var result=mgr.Get<string>(key);
            Console.WriteLine(result);
            Assert.IsTrue(result == value1 || result == value2);
        }
        [TestMethod]
        public void MemcachedRemoveTest()
        {
            Assert.IsTrue(mgr.Remove(key));
        }
        [TestMethod]
        public void MemcachedRemoveAllTest()
        {
            Assert.IsTrue(mgr.RemoveAll());
        }
    }
}
