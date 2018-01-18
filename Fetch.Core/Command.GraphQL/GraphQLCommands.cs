using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Synoptic;

namespace CommandGraphQL
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public string Variables { get; set; }
    }

    [Command(RouteBase = "v1/graphQL")]
    public class GraphQLCommands { 
    
        public static IContainer TheContainer { get; set; }

        [CommandAction(Route = "post", Method = "POST")]
        public async Task<dynamic> PostAsync([CommandParameter(FromBody = true)]GraphQLQuery body)
        {
            var graphQLRequestHandler = TheContainer.Resolve<GraphQLRequestHandler>();

            return await graphQLRequestHandler.PostAsync(body);
        }
    }
}