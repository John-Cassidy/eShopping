using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductsByTypeHandler : IRequestHandler<GetProductsByTypeQuery, IList<ProductResponse>> {
    private readonly IProductRepository _productRepository;

    public GetProductsByTypeHandler(IProductRepository productRepository) {
        _productRepository = productRepository;
    }
    public async Task<IList<ProductResponse>> Handle(GetProductsByTypeQuery request, CancellationToken cancellationToken) {
        var productList = await _productRepository.GetProductsByType(request.Typename);
        var productResponseList = ProductMapper.Mapper.Map<IList<ProductResponse>>(productList);
        return productResponseList;
    }
}
