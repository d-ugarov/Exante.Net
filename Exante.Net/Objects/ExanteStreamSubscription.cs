using System;

namespace Exante.Net.Objects
{
    public class ExanteStreamSubscription
    {
        public Guid Id { get; }
        public DateTime CreateDate { get; }
        
        public ExanteStreamSubscription(Guid id)
        {
            Id = id;
            CreateDate = DateTime.UtcNow;
        }
    }
}