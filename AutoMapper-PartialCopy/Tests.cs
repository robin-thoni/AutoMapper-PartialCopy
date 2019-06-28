using System;
using AutoMapper;
using Xunit;

namespace AutoMapper_PartialCopy
{
    public class Tests
    {
        public IMapper Mapper { get; set; }

        public Tests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ModelChild, DtoChild>()
                    .ForMember(d => d.Id, m => m.MapFrom(s => s.id))
                    .ForMember(d => d.Name1, m => m.MapFrom(s => s.name_1))
                    .ForMember(d => d.Name2, m => m.MapFrom(s => s.name_2))
                    .ForMember(d => d.FullName, m => m.MapFrom(s => s.name_1 + " (" + s.name_2 + ")"));
                cfg.CreateMap<ModelParent, DtoParent>()
                    .ForMember(d => d.Id, m => m.MapFrom(s => s.id))
                    .ForMember(d => d.Child, m => m.MapFrom(s => s.child));

                cfg.AddPartialCopy();
            });

            config.AssertConfigurationIsValid();

            Mapper = config.CreateMapper();
        }

        [Fact]
        public void TestPartialOutput()
        {
            var fieldsOutput = new FieldsFilterPartialResponse("Child/Fullname", "Child/Id");

            var model = new ModelParent
            {
                id = Guid.NewGuid(),
                child = new ModelChild
                {
                    id = Guid.NewGuid(),
                    name_1 = "foo",
                    name_2 = "bar"
                }
            };
            var dto = Mapper.MapPartial<DtoParent>(fieldsOutput, null, model);

            Assert.Equal(Guid.Empty, dto.Id); // Not requested in output, provided in input
            Assert.NotNull(dto.Child); // Implicitly requested in output, provided in input
            Assert.Equal(model.child.id, dto.Child.Id); // Requested in output, provided in input
            Assert.Null(dto.Child.Name1); // Not requested in output, provided in input
            Assert.Null(dto.Child.Name2); // Not requested in output, provided in input
            Assert.Equal("foo (bar)", dto.Child.FullName); // Requested in output, all inputs are provided
        }

        [Fact]
        public void TestPartialInput()
        {
            var fieldsInput = new FieldsFilterPartialResponse("child/name_1");

            var model = new ModelParent
            {
                child = new ModelChild
                {
                    name_1 = "foo"
                }
            };
            var dto = Mapper.MapPartial<DtoParent>(null, fieldsInput, model);

            Assert.Equal(Guid.Empty, dto.Id); // Requested in output, not provided in input
            Assert.NotNull(dto.Child); // Implicitly requested in output, partially provided in input
            Assert.Equal(Guid.Empty, dto.Child.Id); // Requested in output, not provided in input
            Assert.Equal("foo", dto.Child.Name1); // Requested in output, provided in input
            Assert.Null(dto.Child.Name2); // Requested in output, not provided in input
            Assert.Null(dto.Child.FullName); // Requested in output, not fully provided in input
        }

        [Fact]
        public void TestPartialInputOutput()
        {
            var fieldsInput = new FieldsFilterPartialResponse("child/name_1", "chile/name_2");
            var fieldsOutput = new FieldsFilterPartialResponse("Child/Fullname", "Child/Id", "Id");

            var model = new ModelParent
            {
                id = Guid.NewGuid(),
                child = new ModelChild
                {
                    name_1 = "foo",
                    name_2 = "bar"
                }
            };
            var dto = Mapper.MapPartial<DtoParent>(fieldsOutput, fieldsInput, model);

            Assert.Equal(Guid.Empty, dto.Id);
            Assert.NotNull(dto.Child);
            Assert.Equal(Guid.Empty, dto.Child.Id);
            Assert.Null(dto.Child.Name1);
            Assert.Null(dto.Child.Name2);
            Assert.Equal("foo (bar)", dto.Child.FullName);
        }
    }
}
