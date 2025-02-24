
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [Required]
    [BsonElement("name")]
    [BsonRepresentation(BsonType.String)]
    [MinLength(2)]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    [BsonElement("email")]
    [BsonRepresentation(BsonType.String)]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    [BsonElement("password")]
    [BsonRepresentation(BsonType.String)]
    public string Password { get; set; } = null!;

    [Required]
    [BsonElement("role")]
    [BsonRepresentation(BsonType.String)]
    public string Role { get; set; } = "user";
}
