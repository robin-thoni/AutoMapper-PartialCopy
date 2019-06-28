using System;

namespace AutoMapper_PartialCopy
{
    public class ModelParent
    {
        public Guid id { get; set; }

        public ModelChild child { get; set; }
    }
}
