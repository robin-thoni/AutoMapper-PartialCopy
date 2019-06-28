using System;

namespace AutoMapper_PartialCopy
{
    public class DtoParent
    {
        public Guid Id { get; set; }

        public DtoChild Child { get; set; }
    }
}
