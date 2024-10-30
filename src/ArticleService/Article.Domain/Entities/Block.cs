﻿namespace Article.Domain.Entities;

public class Block
{
    public Guid Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public required string Type { get; set; }
}
