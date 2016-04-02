﻿namespace MessagePersisterComponent
{
    public interface IMessagerPersister
    {
        /// <summary>
        /// Stop the persisting. If any outstadning messages these will not be persisted
        /// </summary>
        void StopImmediately();

        /// <summary>
        /// Stop gracefully. The call will not return until all all messages will be persisted.
        /// </summary>
        void Stop();

        /// <summary>
        /// Persist a message to the storage.
        /// </summary>
        /// <param name="message">The message persisted to storage</param>
        void Persist(Message message);


    }
}
