using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Otus.QueueDto.Lot;
using System.Threading.Tasks;

namespace MassTransitCompletedLotProducer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LotsController(IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CancelLot([FromBody] CancelLotEvent request)
    {
        await publishEndpoint.Publish(request);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> WonLot([FromBody] WonLotEvent request)
    {
        await publishEndpoint.Publish(request);
        return Ok();
    }
}

