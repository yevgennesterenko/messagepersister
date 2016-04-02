namespace MessagePersisterComponent
{
    using System;
    using System.Text;

    /// <summary>
    /// This is the object that the diff. persisters(file persister,  database logger etc.) will operate on. The FormatMessage() method will be called to get the message (formatted) to persist
    /// </summary>
    public class Message
    {
        private Guid guid;
        private string name;
        private string payload;

      
        public Message(Guid guid, string name, string payload)
        {
            this.guid = guid;
            this.name = name;
            this.payload = payload;
        }
      

        /// <summary>
        /// Return a formatted message
        /// </summary>
        /// <returns></returns>
        public virtual string FormatMessage()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.guid);
            sb.Append(". ");

            if (this.name.Length>0)
            {
                sb.Append(this.name);
                sb.Append(". ");
            }

            if (this.payload.Length>0)
            {
                sb.Append(this.payload);
                sb.Append(". ");
            }

            sb.Append(this.CreateMessageFooter());

            return sb.ToString();
        }

        public virtual string CreateMessageFooter()
        {
            return "";
        }

      
        /// <summary>
        /// The Timestamp is initialized when the message is added. 
        /// </summary>
        public virtual DateTime Timestamp { get; set; }

        public string Name
        {
            get { return this.name; }
        }

       
    }
}