using Ninject;
using System;

namespace FingerPrintManagerApp.IoC
{
    public class Container
    {
        public IKernel Kernel
        {
            get;
            private set;
        }

        private static volatile Container instance = null;
        private static object syncRoot = new Object();

        private Container()
        {
            this.Kernel = new Ninject.StandardKernel();
            this.Kernel.Load(new Module());
        }

        public static Container Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Container();
                    }
                }
                return instance;
            }
        }
    }
}
