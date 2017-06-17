using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections;

namespace Weaver
{
    public class Spider
    {
        private ThreadManager threadManager { get; set; }

        private Queue<Url> URLQueue { get; set; }
        private HashSet<String> UrlsSeen { get; set; }

        public Spider()
        {
            this.threadManager = new ThreadManager();
            this.URLQueue = new Queue<Url>();
            this.UrlsSeen = new HashSet<String>();
        }

        public void Go()
        {
            foreach (string seed in SpiderController.SeedURLs)
            {
                Url url = new Url(seed, -1);
                this.UrlsSeen.Add(seed);
                threadManager.LaunchThread(FetchNewPage, url);
            }

            if (SpiderController.SeedURLs.Count == 0)
                Console.WriteLine("Need at least one seed URL.");
        }
        Hashtable ht = new Hashtable();
        public void Get(List<string> urls)
        {            
            SpiderController.WhiteListedDomains.Clear();
            foreach (string seed in urls)
            {
                string dm = getDomain(seed);
                if (ht[dm] == null)
                {
                    SpiderController.WhiteListedDomains.Add(dm);
                    ht[dm] = dm;
                }
                Url url = new Url(seed, -1);                
                this.UrlsSeen.Add(seed);
                threadManager.LaunchThread(FetchNewPage, url);
            }

            if (SpiderController.SeedURLs.Count == 0)
                Console.WriteLine("Need at least one seed URL.");
        }

        private string getDomain(string seed)
        {
            string[] temp = seed.Split('/');
            if(temp.Length>2)
            {
                return temp[2].Replace("www.","");
            }
            return "";
        }
        private void FetchNewPage(Url url)
        {
            Log.WriteToLog("Fetching page...", url.uri.AbsoluteUri);

            NetworkConnection connection = new NetworkConnection();
            Page page = new Page(url, connection.Go(url));

            if (!String.IsNullOrEmpty(page.source))
            {
                Log.LoadSuccess(url.uri.AbsoluteUri);
                Crawl(page);
            }
            LoadNextURL();
        }

        private void Crawl(Page page)
        {
            page.FetchAllUrls(page.url.depth);

            if (page.UrlList.Count > 0)
            {
                foreach (Url url in page.UrlList)
                    HandleURL(url);
            }
            else
                Console.WriteLine("No links found.");

            Console.WriteLine("Finished crawling page.");
        }

        private void LoadNextURL()
        {
            while (this.URLQueue.Count > 0)
            {
                if (threadManager.ThreadList.Count >= SpiderController.MaxThreads)
                    break;

                Url url = new Url();

                lock (this.URLQueue)
                {
                    if (this.URLQueue.Count > 0)
                        url = this.URLQueue.Dequeue();
                }

                if (SpiderController.ShouldContinue(url.depth))
                {
                    Thread.Sleep(SpiderController.IdleTime());
                    threadManager.LaunchThread(FetchNewPage, url);
                }
            }
            threadManager.KillThread();
        }

        private void HandleURL(Url url)
        {
            string link = url.uri.AbsoluteUri.ToLower();

            if (this.UrlsSeen.Contains(link))
                Log.SkippedThisQueuedURL(link);
            else if (SpiderController.UseWhiteList == true && !SpiderController.IsWhiteListedDomain(url.uri.Authority))
                Log.WriteToLog("URL domain not on whitelist", link);
            else if (SpiderController.IsExcludedDomain(link))
                Log.SkippedThisExcludedURL(link);
            else if (SpiderController.IsExcludedFileType(link))
                Log.SkippedThisExcludedFileType(link);
            else if (SpiderController.ShouldDownload(link))
            {
                this.UrlsSeen.Add(link);
                url.Download();
            }
            else
            {
                lock (this.URLQueue)
                {
                    this.UrlsSeen.Add(link);
                    this.URLQueue.Enqueue(url);
                }

                Log.EngueuedURL(link);
            }
        }
    }
}
