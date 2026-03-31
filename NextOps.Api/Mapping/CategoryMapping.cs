using NextOps.Api.Entities;
using NextOps.Api.Dtos.Categories;

namespace NextOps.Api.Mapping;

public static class CategoryMapping
{
    public static CategoryItemDto ToCategoryDto(this Category category)
    {
        return new CategoryItemDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Status = category.Status
        };
    }
}