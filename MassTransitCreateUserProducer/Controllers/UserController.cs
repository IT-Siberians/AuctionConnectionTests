using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Otus.QueueDto.User;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MassTransitCreateUserProducer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateUserEvent request)
    {
        await publishEndpoint.Publish(request);
        return Ok();
    }
}
