﻿namespace Identity.Presentation.Options.Models;

public class JwtOptions
{
    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public required string SecretKey { get; set; }
}