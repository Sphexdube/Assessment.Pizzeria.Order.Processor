﻿namespace Order.Domain.Entities
{
    public sealed record Product
    {
        public required string ProductId { get; init; }

        public required string ProductName { get; init; }

        public required decimal Price { get; init; }

        public required decimal Vat { get; init; }
    }
}
