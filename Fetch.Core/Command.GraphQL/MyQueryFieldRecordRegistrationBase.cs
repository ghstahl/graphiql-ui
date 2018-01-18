using System;
using System.Globalization;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace CommandGraphQL
{
    public class MyQueryFieldRecordRegistrationBase : IQueryFieldRecordRegistration
    {
        public MyQueryFieldRecordRegistrationBase()
        {
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            var fieldName = "echo";

            var fieldType = queryCore.FieldAsync<VersionQueryOutput>(name: fieldName,
                description: null,
                arguments: new QueryArguments(new QueryArgument<VersionQueryInput> { Name = "input" }),
                resolve: async context =>
                {

                    var input = context.GetArgument<VersionQueryHandle>("input");
                    return input;
                },
                deprecationReason: null);
        }
    }
}