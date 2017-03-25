namespace HPSManagement{    
    using System.Collections.Generic;    
    using System;    
    using System.Threading;    
    public delegate void UserWorkEventHandler<T>(object sender, WorkQueue<T>.EnqueueEventArgs e);    
    public class WorkQueue<T>    {  
        private bool IsWorking; //表明处理线程是否正在工作        
        private object lockIsWorking = new object();//对IsWorking的同步对象        
        private Queue<T> queue; //实际的队列        
        private object lockObj = new object(); //队列同步对象               
        /// <summary>        
        /// 绑定用户需要对队列中的item对象       
        /// 
        /// 施加的操作的事件        
        /// </summary>        
        /// 
        public event UserWorkEventHandler<T> UserWork;        
        public WorkQueue(int n)        
        {            
            queue = new Queue<T>(n);        
        }        
        public WorkQueue()        
        {            
            queue = new Queue<T>();        
        }        
        /// <summary>        
        /// 谨慎使用此函数，        
        /// 只保证此瞬间，队列值为空        
        /// </summary>        
        /// <returns></returns>        
        public bool IsEmpty()        
        {            
            lock (lockObj)            
            {                
                return queue.Count == 0;            
            }        
        }
        /// <summary> 
        /// 取回队列长度
        /// </summary>        
        /// <returns></returns>        
        public int getCount()
        {
            lock (lockObj)
            {
                return queue.Count;
            }
        }
        private bool isOneThread;        
        /// <summary>        
        /// 队列处理是否需要单线程顺序执行        
        /// ture表示单线程处理队列的T对象        
        /// 默认为false，表明按照顺序出队，但是多线程处理item        
        /// *****注意不要频繁改变此项****        
        /// </summary>        
        public bool WorkSequential        
        {            
            get            
            {                
                return isOneThread;            
            }            
            set            
            {                
                isOneThread = value;            
            }        
        }        
        /// <summary>        
        /// 向工作队列添加对象，        
        /// 对象添加以后，如果已经绑定工作的事件        
        /// 会触发事件处理程序，对item对象进行处理        
        /// </summary>        
        /// <param name="item">添加到队列的对象</param>        
        public void EnqueueItem(T item)        
        {            
            lock (lockObj)            
            {                
                queue.Enqueue(item);            
            }            
            lock (lockIsWorking)            
            {                
                if (!IsWorking)                
                {                    
                    IsWorking = true;                    
                    ThreadPool.QueueUserWorkItem(doUserWork);                
                }            
            }        
        }        
        /// <summary>        
        /// 处理队列中对象的函数        
        /// </summary>        
        /// <param name="o"></param>        
        private void doUserWork(object o)        
        {            
            try            
            {                
                T item;                
                while (true)                
                {                    
                    lock (lockObj)                    
                    {                        
                        if (queue.Count > 0)                        
                        {                            
                            item = queue.Dequeue();                        
                        }                        
                        else                        
                        {                            
                            return;                        
                        }                    
                    }                    
                    if (!item.Equals(default(T)))                    
                    {                        
                        if (isOneThread)                        
                        {                            
                            if (UserWork != null)                            
                            {                                
                                UserWork(this, new EnqueueEventArgs(item));                            
                            }                        
                        }                        
                        else                        
                        {                            
                            ThreadPool.QueueUserWorkItem(obj =>                                                             
                            {                                                                 
                                if (UserWork != null)                                                                 
                                {                                                                     
                                    UserWork(this, new EnqueueEventArgs(obj));                                                                 
                                }                                                             
                            }, item);                        
                        }                                            
                    }                
                }            
            }            
            finally            
            {                
                lock (lockIsWorking)                
                {                    
                    IsWorking = false;                
                }            
            }        
        }        
        /// <summary>        
        /// UserWork事件的参数，包含item对象        
        /// </summary>        
        public class EnqueueEventArgs : EventArgs        
        {            
            public T Item 
            { 
                get; 
                private set; 
            }            
            public EnqueueEventArgs(object item)            
            {                
                try                
                {                    
                    Item = (T)item;                
                }                
                catch (Exception)                
                {                    
                    throw new InvalidCastException("object to T 转换失败");                
                }            
            }        
        }    
    }
}