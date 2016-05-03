using L.Study.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Console
{
    using Common.Crypt;
    using System;
    using System.ComponentModel;
    using System.Threading;
    class Program
    {
        static string key = "key1";
        static string value1 = "lihanhan";
        //static string value2 = "lihanhan_Set_result";
        static void Main(string[] args)
        {
            //MemcachedTest();
            //string key = "jwykjwyk";
            //var sss=Cryptor.DesEncrypt("eric lee today is a nice rainy day and sunny anf and funny day happy new years", key);
            //Console.WriteLine(sss);
            //var s1 = Cryptor.DesDecrypt(sss, key);
            //Console.WriteLine(s1);
            //var s2 = Cryptor.DesDecrypt2(sss, key);
            //Console.WriteLine(s2);
            //RedisCachTest();
            Console.WriteLine("main is start");
            Thread.CurrentThread.Name = "主线程";
            Task<string> s = AsyncTest.GetStringAsync();
            //Task<string> s = Task.Run(() => { return AsyncTest.GetString(); });
            //Task<string> s = Task.Factory.StartNew(() => { return AsyncTest.GetString(); }, TaskCreationOptions.LongRunning);//这个与一个的区别仅在于是否使用池化线程呃
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(i);
            //    Thread.Sleep(1000);
            //}
            var str=AsyncTest.GetString();
            Console.WriteLine(str);
            Console.WriteLine("main function is going to complete");
            Console.WriteLine(s.Result);
            Console.Read();
        }

        private static void MemcachedTest()
        {            
            ICacheMgr mgr = new MemcachedMgr();
            Console.WriteLine(mgr.RemoveAll());
            Console.WriteLine(mgr.Add<string>(key, value1));
            Console.WriteLine(mgr.Set<string>(key, value1));
            Console.WriteLine(mgr.Exist(key));
            Console.WriteLine(mgr.Get<string>(key));
            Console.WriteLine(mgr.Remove(key));
        }
        public static void RedisCachTest()
        {
            ICacheMgr mgr = new RedisMgr();
            //Console.WriteLine(mgr.Add<string>(key, value1));
            Role r1 = new Role { ID = "1", Name = "1" };
            r1.Childs.Add(new Role { ID = "2", Name = "2" });
            r1.Childs.Add(new Role { ID = "3", Name = "3" });
            r1.Childs.Add(new Role { ID = "4", Name = "4" });
            r1.Childs.Add(new Role { ID = "5", Name = "5" });
            
            mgr.Add(key, r1);
        }
    }
    public class Role
    {
        public Role()
        {
            Childs = new List<Role>();
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public List<Role> Childs { get; set; }
    }

    // The following example demonstrates how to create
    // a resource class that implements the IDisposable interface
    // and the IDisposable.Dispose method.

    public class DisposeExample
    {
        // A base class that implements IDisposable.
        // By implementing IDisposable, you are announcing that
        // instances of this type allocate scarce resources.
        public class MyResource : IDisposable
        {
            // Pointer to an external unmanaged resource.指针指向一个外部的非托管资源
            private IntPtr handle;
            // Other managed resource this class uses.这个类使用的其他托管资源。
            private Component component = new Component();
            // Track whether Dispose has been called.
            private bool disposed = false;

            // The class constructor.类构造函数
            public MyResource(IntPtr handle)
            {
                this.handle = handle;
            }

            // Implement IDisposable.
            // Do not make this method virtual.
            // A derived class should not be able to override this method.
            public void Dispose()
            {
                Dispose(true);
                // This object will be cleaned up by the Dispose method.
                // Therefore, you should call GC.SupressFinalize to
                // take this object off the finalization queue
                // and prevent finalization code for this object
                // from executing a second time.
                GC.SuppressFinalize(this);
            }

            // Dispose(bool disposing) executes in two distinct scenarios.
            // If disposing equals true, the method has been called directly
            // or indirectly by a user's code. Managed and unmanaged resources
            // can be disposed.
            // If disposing equals false, the method has been called by the
            // runtime from inside the finalizer and you should not reference
            // other objects. Only unmanaged resources can be disposed.
            protected virtual void Dispose(bool disposing)
            {
                // Check to see if Dispose has already been called.
                if (!this.disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // Dispose managed resources.清理托管资源
                        component.Dispose();
                    }

                    // Call the appropriate methods to clean up
                    // unmanaged resources here.
                    // If disposing is false,
                    // only the following code is executed.清理非托管资源，如数据库连接等
                    CloseHandle(handle);
                    handle = IntPtr.Zero;

                    // Note disposing has been done.
                    disposed = true;

                }
            }

            // Use interop to call the method necessary
            // to clean up the unmanaged resource.
            [System.Runtime.InteropServices.DllImport("Kernel32")]
            private extern static Boolean CloseHandle(IntPtr handle);

            // Use C# destructor syntax for finalization code.
            // This destructor will run only if the Dispose method
            // does not get called.
            // It gives your base class the opportunity to finalize.
            // Do not provide destructors in types derived from this class.
            ~MyResource()
            {
                // Do not re-create Dispose clean-up code here.
                // Calling Dispose(false) is optimal in terms of
                // readability and maintainability.
                Dispose(false);
            }
        }
    }

    public interface IMydisposable
    {
        // 摘要: 
        //     执行与释放或重置非托管资源相关的应用程序定义的任务。
        void MyDispose();
    }
    public class Mydisposeclass:IMydisposable
    {
        public Mydisposeclass()
        {
            Console.WriteLine("构造中");
        }
        public virtual void MyDispose()
        {
            Console.WriteLine("调用Mydisposeclass的MyDispose方法了");
        }
    }
    public class TwoDispose : Mydisposeclass
    {

    }
}
