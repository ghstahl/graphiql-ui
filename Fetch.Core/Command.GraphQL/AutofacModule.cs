using Autofac;
using GraphQL.Types;
using Module = Autofac.Module;

namespace CommandGraphQL
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VersionQueryInput>();
            builder.RegisterType<VersionQueryOutput>();
            builder.RegisterType<GraphQLRequestHandler>()
                .SingleInstance();
        }
    }
    public class VersionQueryOutput : ObjectGraphType
    {
        public VersionQueryOutput()
        {
            Name = "VersionQueryOutput";
            Field<NonNullGraphType<StringGraphType>>("id");
            Field<NonNullGraphType<StringGraphType>>("treatment");
            Field<StringGraphType>("culture");
        }
    }
    public class VersionQueryInput : InputObjectGraphType
    {
        public VersionQueryInput()
        {
            Name = "VersionQueryInput";
            Field<NonNullGraphType<StringGraphType>>("id");
            Field<NonNullGraphType<StringGraphType>>("treatment");
            Field<StringGraphType>("culture");
        }
    }
    public class VersionQueryHandle
    {
        public string Id { get; set; }
        public string Treatment { get; set; }
        public string Culture { get; set; }
        public VersionQueryHandle()
        {
        }

        public VersionQueryHandle(VersionQueryHandle doc)
        {
            this.Id = doc.Id;
            this.Treatment = doc.Treatment;
            this.Culture = doc.Culture;
        }

        public override bool Equals(object obj)
        {
            var other = obj as VersionQueryHandle;
            if (other == null)
            {
                return false;
            }

            return Id.Equals(other.Id)
                   && Treatment.Equals(other.Treatment)
                   && Culture.Equals(other.Culture);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}