using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using WalletGrpcClient;

// создаем канал для обмена сообщениями с сервером
// параметр - адрес сервера gRPC
using var channel = GrpcChannel.ForAddress("https://localhost:7142");

// создаем клиент
var client = new Trading.TradingClient(channel);

// Здесь должны быть ошибки:
await ReserveMoney("", 0, "", "", "");
await ReserveMoney("11111111-5717-4562-b3fc-111100001111", 0, "", "", "");
await ReserveMoney("11111111-5717-4562-b3fc-111100001111", -100, "", "", "");
await ReserveMoney("11111111-5717-4562-b3fc-111100001111", 100, "", "", "");
await ReserveMoney("11111111-5717-4562-b3fc-111100001111", 100, "22222222-5717-4562-b3fc-777700001111", "", "");
await ReserveMoney("11111111-5717-4562-b3fc-111100001111", 100, "22222222-5717-4562-b3fc-777700001111", "Лот №1", "");
await ReserveMoney("11111111-5717-4562-b3fc-111100001119", 100, "22222222-5717-4562-b3fc-777700001111", "Лот №1", "Описание лота №1");

// Здесь не должно быть ошибок, если в кошельке достаточно денег:
await ReserveMoney("11111111-5717-4562-b3fc-111100001111", 100, "22222222-5717-4562-b3fc-777700001111", "Лот №1", "Описание лота №1");
await RealeaseMoney("11111111-5717-4562-b3fc-111100001111", 100, "22222222-5717-4562-b3fc-777700001111");
await ReserveMoney("11111111-5717-4562-b3fc-111100002222", 200, "22222222-5717-4562-b3fc-777700001111", "Лот №1", "Описание лота №1");
await RealeaseMoney("11111111-5717-4562-b3fc-111100002222", 200, "22222222-5717-4562-b3fc-777700001111");
await ReserveMoney("11111111-5717-4562-b3fc-111100001111", 500, "22222222-5717-4562-b3fc-777700001111", "Лот №1", "Описание лота №1");
await PayForLot("11111111-5717-4562-b3fc-111100001111", 500, "22222222-5717-4562-b3fc-777700001111", "11111111-5717-4562-b3fc-111100003333");

async Task PayForLot(string buyerId, double hammerPrice, string lotId, string sellerId)
{
    var request = new PayForLotCommandGrpc
    {
        BuyerId = buyerId,
        SellerId = sellerId,
        LotId = lotId,
        HammerPrice = hammerPrice
    };

    Console.WriteLine("-------------------------------------------");
    Console.WriteLine("Request:");
    PrintPayForLotCommand(request);
    Console.WriteLine();

    var response = await client.PayForLotAsync(request);

    Console.WriteLine("Response:");
    PrintResponse(response);
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine();
}

async Task RealeaseMoney(string buyerId, double price, string lotId)
{
    var request = new RealeaseMoneyCommandGrpc
    {
        BuyerId = buyerId,
        LotId = lotId,
        Price = price
    };

    Console.WriteLine("-------------------------------------------");
    Console.WriteLine("Request:");
    PrintRealeaseMoneyCommand(request);
    Console.WriteLine();

    var response = await client.RealeaseMoneyAsync(request);

    Console.WriteLine("Response:");
    PrintResponse(response);
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine();
}

async Task ReserveMoney(string buyerId, double price, string lotId, string lotTitle, string lotDescription)
{
    var request = new ReserveMoneyCommandGrpc
    {
        BuyerId = buyerId,
        Price = price,
        Lot = new LotInfoModelGrpc
        {
            Id = lotId,
            Title = lotTitle,
            Description = lotDescription
        }
    };

    Console.WriteLine("-------------------------------------------");
    Console.WriteLine("Request:");
    PrintReserveMoneyCommand(request);
    Console.WriteLine();

    var response = await client.ReserveMoneyAsync(request);

    Console.WriteLine("Response:");
    PrintResponse(response);
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine();
}


void PrintPayForLotCommand(PayForLotCommandGrpc request)
{
    Console.WriteLine($"BuyerId = {request.BuyerId}");
    Console.WriteLine($"SellerId = {request.SellerId}");
    Console.WriteLine($"LotId = {request.LotId}");
    Console.WriteLine($"HammerPrice = {request.HammerPrice}");
}

void PrintRealeaseMoneyCommand(RealeaseMoneyCommandGrpc request)
{
    Console.WriteLine($"BuyerId = {request.BuyerId}");
    Console.WriteLine($"LotId = {request.LotId}");
    Console.WriteLine($"Price = {request.Price}");
}

void PrintReserveMoneyCommand(ReserveMoneyCommandGrpc request)
{
    Console.WriteLine($"BuyerId = {request.BuyerId}");
    Console.WriteLine($"Price = {request.Price}");
    Console.WriteLine($"Lot.Id = {request.Lot.Id}");
    Console.WriteLine($"Lot.Title = {request.Lot.Title}");
    Console.WriteLine($"Lot.Description = {request.Lot.Description}");
}

void PrintResponse(BaseResponseGrpc response)
{
    if (response.IsError)
    {
        Console.WriteLine($"ERROR: {response.Message}");
    }
    else
    {
        Console.WriteLine($"OK: {response.Message}");
    }
}
