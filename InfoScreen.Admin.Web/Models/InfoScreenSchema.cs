using GraphQL;
using GraphQL.Types;

namespace InfoScreen.Admin.Web.Models
{
    public class InfoScreenSchema : Schema
    {
        public InfoScreenSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<InfoScreenQuery>();
            Mutation = resolver.Resolve<InfoScreenMutation>();
        }
    }
}