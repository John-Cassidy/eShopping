﻿using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponse>> {
    private readonly ITypesRepository _typesRepository;

    public GetAllTypesHandler(ITypesRepository typesRepository) {
        _typesRepository = typesRepository;
    }

    public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken) {
        var typesList = await _typesRepository.GetAllTypes();
        // create IList<TypesResponse> from IList<ProductType> using ProductMapper.Mapper
        var typesListResponse = ProductMapper.Mapper.Map<IList<ProductType>, IList<TypesResponse>>(typesList.ToList());
        return typesListResponse;
    }
}
