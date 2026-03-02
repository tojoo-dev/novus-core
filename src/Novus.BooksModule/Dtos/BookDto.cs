using System.ComponentModel.DataAnnotations;

namespace Novus.BooksModule.Dtos;

public record struct BookDto(
    int? Id,

    [property: Required]
    string Title
);
