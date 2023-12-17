using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetProductsByBrandQuery : IRequest<IList<ProductResponse>> {
    public string Brandname { get; set; }

    public GetProductsByBrandQuery(string brandname) {
        Brandname = brandname;
    }
}
