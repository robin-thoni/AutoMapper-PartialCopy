using System.Collections.Generic;
using System.Linq;
using PartialResponse.Core;

namespace AutoMapper_PartialCopy
{
    public class FieldsFilterPartialResponse : IFieldsFilter
    {
        private IList<Field> Fields = new List<Field>();

        public FieldsFilterPartialResponse(params string[] paths)
        {
            Add(paths);
        }

        public bool Matches(string path, bool ignoreCase = true)
        {
            string[] parts = path.Split('/');
            return Fields.Any(field => field.Matches(parts, ignoreCase));
        }

        public FieldsFilterPartialResponse Add(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (PartialResponse.Core.Fields.TryParse(path, out var tempFields))
                {
                    foreach (var field in tempFields.Values)
                    {
                        Fields.Add(field);
                    }
                }
                else
                {
                    // TODO error management
                }
            }
            return this;
        }
    }
}
