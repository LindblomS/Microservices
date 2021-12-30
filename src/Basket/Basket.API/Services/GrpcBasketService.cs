namespace Basket.API.Services;

using Basket.API.Protos;
using Grpc.Core;
using System.Threading.Tasks;

public class GrpcBasketService : Basket.BasketBase
{

    public override Task<GetBasketReply> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<UpdateBasketReply> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<CreateCheckoutReply> CreateCheckout(CreateCheckoutRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}
