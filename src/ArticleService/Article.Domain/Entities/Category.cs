﻿namespace Article.Domain.Entities;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public int Level { get; set; }
}
