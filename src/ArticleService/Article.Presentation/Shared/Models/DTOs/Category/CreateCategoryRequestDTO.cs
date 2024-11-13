namespace Article.Presentation.Shared.Models.DTOs.Category;

public record class CreateCategoryRequestDTO(string Name, Guid? ParentId);
