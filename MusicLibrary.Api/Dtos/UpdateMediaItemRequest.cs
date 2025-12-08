using System.ComponentModel.DataAnnotations;

namespace MusicLibrary.Api.Dtos
{
    public record UpdateMediaItemRequest(
        [Required]
        [MaxLength(200)]
        string Title
    );
}
