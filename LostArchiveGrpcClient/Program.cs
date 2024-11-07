using Grpc.Net.Client;
using LotsArchiveGrpcClient;
using System;
using System.Threading.Tasks;

// создаем канал для обмена сообщениями с сервером
// параметр - адрес сервера gRPC
using var channel = GrpcChannel.ForAddress("https://localhost:7100");

// создаем клиент
var client = new Copying.CopyingClient(channel);

(string, string)[] requests =
[
    ("", ""),
    ("123", "123"),
    ("", "22222222-5717-4562-b3fc-222200001111"),
    ("11111111-5717-4562-b3fc-111100001111", ""),
    ("11111111-5717-4562-b3fc-999900001111", "22222222-5717-4562-b3fc-999900001111"),
    ("11111111-5717-4562-b3fc-111100001111", "22222222-5717-4562-b3fc-222200001111"),
    ("11111111-5717-4562-b3fc-111100001111", "22222222-5717-4562-b3fc-222200005555"),
    ("11111111-5717-4562-b3fc-111100004444", "22222222-5717-4562-b3fc-22220000aaaa"),
];

foreach (var item in requests)
{
    await TestLotGetting(client, item.Item1, item.Item2);
}

async Task TestLotGetting(Copying.CopyingClient client, string sellerId, string lotId)
{
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine("Request:");
    Console.WriteLine($"SellerId = {sellerId}");
    Console.WriteLine($"LotId = {lotId}");
    Console.WriteLine();

    var response = await GetLotCopy(client, sellerId, lotId);

    Console.WriteLine("Response:");

    if (response.IsError)
    {
        Console.WriteLine($"ERROR: {response.ErrorMessage}");
    }
    else
    {
        PrintLotInfo(response.LotCopy);
    }

    Console.WriteLine("-------------------------------------------");
    Console.WriteLine();
}

async Task<GetLotCopyResponseGrpc> GetLotCopy(Copying.CopyingClient client, string sellerGuid, string lotGuid)
{
    var request = new GetLotCopyQueryGrpc
    {
        SellerId = sellerGuid,
        LotId = lotGuid
    };

    return await client.GetLotCopyAsync(request);
}

void PrintLotInfo(LotCopyModelGrpc lot)
{
    Console.WriteLine($"Id = {lot.LotInfo.Id}");
    Console.WriteLine($"Title = {lot.LotInfo.Title}");
    Console.WriteLine($"Description = {lot.LotInfo.Description}");
    Console.WriteLine($"StartingPrice = {lot.LotParams.StartingPrice}");
    Console.WriteLine($"BidIncrement = {lot.LotParams.BidIncrement}");
    Console.WriteLine($"RepurchasePrice = {lot.LotParams.RepurchasePrice?.ToString() ?? "null"}");
}
