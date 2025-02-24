using backend.Data;
using backend.entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMongoCollection<User> _users;

    public UserController(MongoDbService mongoDbService)
    {
        _users = mongoDbService.Database?.GetCollection<User>("Users") 
            ?? throw new ArgumentNullException(nameof(mongoDbService.Database));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var users = await _users.Find(_ => true).ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> Post(User user)
    {
        try
        {
            user.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            await _users.InsertOneAsync(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> Put(string id, User user)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid ID");
        }

        user.Id = id;
        var result = await _users.ReplaceOneAsync(u => u.Id == id, user);
        
        if (result.MatchedCount == 0)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _users.DeleteOneAsync(u => u.Id == id);
        
        if (result.DeletedCount == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}