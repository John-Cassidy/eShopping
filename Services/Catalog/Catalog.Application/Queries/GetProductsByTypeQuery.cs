using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetProductsByTypeQuery : IRequest<IList<ProductResponse>> {
    public string Typename { get; set; }

    public GetProductsByTypeQuery(string typename) {
        Typename = typename;
    }
}
