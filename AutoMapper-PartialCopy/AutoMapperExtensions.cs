using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions.Impl;

namespace AutoMapper_PartialCopy
{
    public static class AutoMapperExtensions
    {
        public const string PartialInput = "luPartialInput";
        public const string PartialOutput = "luPartialOutput";
        public const string PartialOutputPath = "luPartialOutputPath";

        public static void AddPartialCopy(this IMapperConfigurationExpression cfg)
        {
            cfg.ForAllMaps((map, expression) =>
                {
                    expression.BeforeMap((o, o1, context) =>
                    {
                        if (context.Items.ContainsKey(PartialOutputPath) && context.Items[PartialOutputPath] is List<string> pathList)
                        {
                            pathList.Add("");
                        }
                    });
                    expression.ForAllMembers(m =>
                    {
                        m.PreCondition((src, context) =>
                        {
                            var shouldCopy = true;

                            if (context.Items.ContainsKey(PartialOutput) && context.Items[PartialOutput] is IFieldsFilter partialOutput)
                            {
                                if (context.Items.ContainsKey(PartialOutputPath) && context.Items[PartialOutputPath] is List<string> pathList)
                                {
                                    pathList[pathList.Count - 1] = m.DestinationMember.Name;
                                    var path = string.Join("/", pathList);
                                    shouldCopy = shouldCopy && partialOutput.Matches(path);
                                }
                            }

                            if (context.Items.ContainsKey(PartialInput) && context.Items[PartialInput] is IFieldsFilter partialInput)
                            {
                                var propertyMap = map.GetPropertyMapByDestinationProperty(m.DestinationMember.Name);
                                if (propertyMap.CustomMapExpression != null)
                                {
                                    // TODO visit propertyMap.CustomMapExpression and look for MemberExpression going to lambda parameter
                                }
                                else if (propertyMap.SourceMembers != null && propertyMap.SourceMembers.Any())
                                {
                                    var pathList = new List<string>();
                                    foreach (var sourceMember in propertyMap.SourceMembers)
                                    {
                                        pathList.Add(sourceMember.Name); // TODO check order (append or prepend)
                                    }
                                    var path = string.Join("/", pathList);
                                    shouldCopy = shouldCopy && partialInput.Matches(path);
                                }
                            }

                            return shouldCopy;
                        });
                    });
                    expression.AfterMap((o, o1, context) =>
                    {
                        if (context.Items.ContainsKey(PartialOutputPath) && context.Items[PartialOutputPath] is List<string> pathList)
                        {
                            pathList.RemoveAt(pathList.Count - 1);
                        }
                    });
                });
        }
        
        private static void PartialCopySetup(IMappingOperationOptions options, IFieldsFilter partialOutput, IFieldsFilter partialInput)
        {
            if (partialOutput != null)
            {
                options.Items[PartialOutput] = partialOutput;
                options.Items[PartialOutputPath] = new List<string>();
            }

            if (partialInput != null)
            {
                options.Items[PartialInput] = partialInput;
            }
        }

        public static TDestination MapPartial<TDestination>(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, object source)
        {
            var convertedObject = mapper.Map<TDestination>(source, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
            });

            return convertedObject;
        }

        public static TDestination MapPartial<TDestination>(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, object source, Action<IMappingOperationOptions> opts)
        {
            var convertedObject = mapper.Map<TDestination>(source, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
                opts(options);
            });

            return convertedObject;
        }

        public static TDestination MapPartial<TSource, TDestination>(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, TSource source)
        {
            var convertedObject = mapper.Map<TSource, TDestination>(source, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
            });

            return convertedObject;
        }

        public static TDestination MapPartial<TSource, TDestination>(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, TSource source, Action<IMappingOperationOptions> opts)
        {
            var convertedObject = mapper.Map<TSource, TDestination>(source, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
                opts(options);
            });

            return convertedObject;
        }

        public static TDestination MapPartial<TSource, TDestination>(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, TSource source, TDestination destination)
        {
            var convertedObject = mapper.Map(source, destination, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
            });

            return convertedObject;
        }

        public static TDestination MapPartial<TSource, TDestination>(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, TSource source, TDestination destination, Action<IMappingOperationOptions> opts)
        {
            var convertedObject = mapper.Map(source, destination, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
                opts(options);
            });

            return convertedObject;
        }

        public static object MapPartial(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, object source, Type sourceType, Type destinationType)
        {
            var convertedObject = mapper.Map(source, sourceType, destinationType, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
            });

            return convertedObject;
        }

        public static object MapPartial(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            var convertedObject = mapper.Map(source, sourceType, destinationType, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
                opts(options);
            });

            return convertedObject;
        }

        public static object MapPartial(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, object source, object destination, Type sourceType, Type destinationType)
        {
            var convertedObject = mapper.Map(source, destination, sourceType, destinationType, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
            });

            return convertedObject;
        }

        public static object MapPartial(this IMapper mapper, IFieldsFilter partialOutput, IFieldsFilter partialInput, object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            var convertedObject = mapper.Map(source, destination, sourceType, destinationType, options =>
            {
                PartialCopySetup(options, partialOutput, partialInput);
                opts(options);
            });

            return convertedObject;
        }
    }
}
