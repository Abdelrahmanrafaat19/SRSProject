using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Domain.Entities
{
    public class BasicEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
