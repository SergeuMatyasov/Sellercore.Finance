using System.Text.Json.Serialization;
using MediatR;
using Sellercore.Finance.Ozon.Application.Interfaces.Repositories;
using Shared.Domain.Interfaces.CQRS;

namespace Sellercore.Finance.Ozon.Application.UseCases.InternalOzonSeller.Commands.AddInternalOzonSeller;

public class AddInternalOzonSellerCommand(int sellerId, string token) : ICommand<Unit>
{
    /// <summary>
    /// Идентификатор продавца.
    /// </summary>
    [JsonPropertyName("seller_id")]
    public int SellerId { get; set; } = sellerId;

    /// <summary>
    /// Токен.
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; } = token;
}

public class AddInternalOzonSellerCommandHandler(IInternalOzonSellerRepository internalOzonSellerRepository)
    : ICommandHandler<AddInternalOzonSellerCommand, Unit>
{
    public async Task<Unit> Handle(AddInternalOzonSellerCommand request, CancellationToken cancellationToken)
    {
        await internalOzonSellerRepository.AddEncryptedSellerAsync(request.SellerId, request.Token, cancellationToken);
        return default;
    }
}