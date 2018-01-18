using System.Collections.Generic;
using Autofac;
using GraphQL.Types;
using Programs.Models;
using Module = Autofac.Module;

namespace CommandGraphQL
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProgramQueryInput>();
            builder.RegisterType<ProgramQueryOutput>();
            builder.RegisterType<InstalledAppType>();
            builder.RegisterType<ProgramPageQueryInput>();
            builder.RegisterType<InstalledPageType>();
            
        }
    }
    public class ProgramQueryOutput : ObjectGraphType
    {
        public ProgramQueryOutput()
        {
            Name = "ProgramQueryOutput";
            Field<NonNullGraphType<StringGraphType>>("displayName");
            Field<NonNullGraphType<BooleanGraphType>>("isInstalled");
           
        }
    }
    public class InstalledAppType : ObjectGraphType
    {
        public InstalledAppType()
        {
            Name = "InstalledAppType";
            Field<NonNullGraphType<StringGraphType>>("displayName");
            Field<NonNullGraphType<StringGraphType>>("unInstallPath");
        }
    }
    public class InstalledPage
    {
        public int CurrentOffset { get; set; }
        public int NextOffset { get; set; }
        public int Count { get; set; }
        public List<InstalledApp> InstalledApps { get; set; }
    }
    public class InstalledPageType : ObjectGraphType<InstalledPage>
    {
        public InstalledPageType()
        {
            Name = "installedPage";
            Field(x => x.CurrentOffset).Description("The current paging offset of the this request.");
            Field(x => x.NextOffset).Description("The next paging offset of the this request.");
            Field(x => x.Count).Description("The count of the current request.");
            Field<ListGraphType<InstalledAppType>>("installedApps", "The installed apps.");

        }
    }
    public class ProgramPageQueryInput : InputObjectGraphType
    {
        public ProgramPageQueryInput()
        {
            Name = "ProgramPageQueryInput";
            Field<NonNullGraphType<IntGraphType>>("offset");
            Field<NonNullGraphType<IntGraphType>>("count");
        }
    }
    public class ProgramQueryInput : InputObjectGraphType
    {
        public ProgramQueryInput()
        {
            Name = "ProgramQueryInput";
            Field<NonNullGraphType<StringGraphType>>("displayName");
        }
    }

}