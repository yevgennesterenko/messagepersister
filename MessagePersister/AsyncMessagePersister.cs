namespace MessagePersisterComponent
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class AsyncMessagePersister : IMessagerPersister
    {
        private readonly Thread runThread;
        private List<Message> messages = new List<Message>();

        private StreamWriter writer; 

        private bool _exit;

        public AsyncMessagePersister()
        {
            if (!Directory.Exists(@"C:\Messages")) 
                Directory.CreateDirectory(@"C:\Messages");

            if (!Directory.Exists(@"C:\Messages\Persisted " + DateTime.Now.ToString("yyyyMMdd")))           
                Directory.CreateDirectory(@"C:\Messages\Persisted " + DateTime.Now.ToString("yyyyMMdd"));
            

            this.runThread = new Thread(this.Processing);
            this.runThread.Start();
        }

        private bool stop = false;


        DateTime _curDate = DateTime.Now;

        private void Processing()
        {
            while (!this._exit)
            {
                if (this.messages.Count > 0)
                {
                    int i = 0;
                    List<Message> _handled = new List<Message>();

                    foreach (Message message in this.messages)
                    {
                        i++;

                        if (i > 5)
                            continue;
                        
                        if (!this._exit || this.stop)
                        {
                            _handled.Add(message);

                            this.writer = File.AppendText(@"C:\Messages\Persisted "+ DateTime.Now.ToString("yyyyMMdd")+@"\"+message.Name + DateTime.Now.ToString(" yyyyMMdd HHmmss fff") + ".log");

                            this.writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

                            this.writer.AutoFlush = true;

                            StringBuilder stringBuilder = new StringBuilder();

                            if ((DateTime.Now - _curDate).Days != 0)
                            {
                                _curDate = DateTime.Now;

                                if (!Directory.Exists(@"C:\Messages\Persisted " + DateTime.Now.ToString("yyyyMMdd")))
                                    Directory.CreateDirectory(@"C:\Messages\Persisted " + DateTime.Now.ToString("yyyyMMdd"));

                            }

                            stringBuilder.Append(message.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                            stringBuilder.Append("\t");
                            stringBuilder.Append(message.FormatMessage());
                            stringBuilder.Append("\t");

                            stringBuilder.Append(Environment.NewLine);

                            this.writer.Write(stringBuilder.ToString());
                            this.writer.Close();
                        }
                    }

                    for (int y = 0; y < _handled.Count; y++)
                    {
                        this.messages.Remove(_handled[y]);   
                    }

                    if (this.stop == true && this.messages.Count == 0) 
                        this._exit = true;

                    Thread.Sleep(50);
                }
            }
        }

        public void StopImmediately()
        {
            this._exit = true;
        }
        
        public void Stop()
        {
            this.stop = true;
        }

        public void Persist(Message message)
        {
            message.Timestamp = DateTime.Now;
            this.messages.Add(message);
        }
    }
}