using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;

namespace CommandGraphQL
{

    public class GraphQLRequestHandler 
    {
        private IDocumentExecuter _executer;
        private IDocumentWriter _writer;
        private ISchema _schema;
        private readonly IDictionary<string, string> _namedQueries;
        private List<IValidationRule> _validationRules =>new List<IValidationRule>();
        public GraphQLRequestHandler(
            IDocumentExecuter executer,
            IDocumentWriter writer,
            ISchema schema)
        {

            _executer = executer;
            _writer = writer;
            _schema = schema;
            _namedQueries = new Dictionary<string, string>
            {
                ["a-query"] = @"query foo { hero { name } }"
            };
          
        }
       
        public async Task<dynamic> PostAsync(GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();
            var queryToExecute = query.Query;

            if (!string.IsNullOrWhiteSpace(query.NamedQuery))
            {
                queryToExecute = _namedQueries[query.NamedQuery];
            }

            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = queryToExecute;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
                _.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };
                _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();
                _.ValidationRules = _validationRules.Concat(DocumentValidator.CoreRules());

            }).ConfigureAwait(false);

            var httpResult = result.Errors?.Count > 0
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.OK;

           

            if (httpResult != HttpStatusCode.OK)
            {
                throw new Exception(result.Errors.First().Message);
            }
            var json = _writer.Write(result);
            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
            return obj;
        }
    }
}